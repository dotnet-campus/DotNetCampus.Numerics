using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using SeWzc.SourceGenerator.CodeText;

namespace SeWzc.Numerics.InternalSourceGenerators;

[Generator]
public class SampleIncrementalSourceGenerator : IIncrementalGenerator
{
    #region 静态变量

    private const int MaxDimension = 4;
    private static readonly ImmutableArray<string> AxisNames = ImmutableArray.Create("X", "Y", "Z", "W");
    private static readonly ImmutableArray<NumberType> NumberTypes = [new NumberType("float", "F", "MathF.Sqrt"), new NumberType("double", "D", "Math.Sqrt")];

    #endregion

    #region 静态方法

    /// <summary>
    /// 生成向量。
    /// </summary>
    /// <param name="context"></param>
    private static void GenerateVector(IncrementalGeneratorInitializationContext context)
    {
        for (var dimension = 2; dimension <= MaxDimension; dimension++)
        {
            foreach (var numberType in NumberTypes)
            {
                var typeName = $"Vector{dimension}{numberType.Suffix}";
                var code = GetVectorCode(typeName, dimension, numberType);
                context.RegisterPostInitializationOutput(ctx => ctx.AddSource($"{typeName}.g.cs", code.ToString()));
            }
        }
    }

    private static CodeBase GetVectorCode(string typeName, int dimension, NumberType numberType)
    {
        var parameters = AxisNames[..dimension];
        var parameterList = string.Join(", ", parameters.Select(name => $"{numberType.Name} {name}"));
        CodeBase code = new CodeCombination([
            "// <auto-generated/>",
            "namespace SeWzc.Numerics;",
            $"""
            /// <summary>
            /// {parameters.Length} 维的向量。
            /// </summary>
            """,
            new CodeBlock($"public readonly partial record struct {typeName}({parameterList}) : IVector<{typeName}, {numberType.Name}>",
            [
                // 抑制警告
                "#pragma warning disable CS1591",

                "public static int Dimension => " + dimension + ";",
                $"public static {typeName} Zero => new({string.Join(", ", parameters.Select(_ => "0"))});",
                $"public {numberType.Name} Length => {numberType.SqrtMethod}(LengthSquared);",
                $"public {numberType.Name} LengthSquared => {string.Join("+ ", parameters.Select(p => $"{p} * {p}"))};",
                $"public {typeName} Normalized => this / Length;",
                new CodeCombination(
                [
                    $"public {numberType.Name} this[int index] => index switch",
                    "{",
                    ..RangeSelect(dimension, i => $"    {i} => {parameters[i]},"),
                    "    _ => throw new ArgumentOutOfRangeException(nameof(index)),",
                    "};",
                ]),
                $"public {numberType.Name} Dot({typeName} other) => {string.Join(" + ", parameters.Select(p => $"{p} * other.{p}"))};",
                $"public static {typeName} operator +({typeName} vector1, {typeName} vector2) => new ({string.Join(", ", parameters.Select(p => $"vector1.{p} + vector2.{p}"))});",
                $"public static {typeName} operator -({typeName} vector1, {typeName} vector2) => new ({string.Join(", ", parameters.Select(p => $"vector1.{p} - vector2.{p}"))});",
                $"public static {numberType.Name} operator *({typeName} vector1, {typeName} vector2) => vector1.Dot(vector2);",
                $"public static {typeName} operator *({typeName} vector, {numberType.Name} scalar) => new ({string.Join(", ", parameters.Select(p => $"vector.{p} * scalar"))});",
                $"public static {typeName} operator *({numberType.Name} scalar, {typeName} vector) => new ({string.Join(", ", parameters.Select(p => $"vector.{p} * scalar"))});",
                $"public static {typeName} operator /({typeName} vector, {numberType.Name} scalar) => new ({string.Join(", ", parameters.Select(p => $"vector.{p} / scalar"))});",
                $"public static {typeName} operator -({typeName} vector) => new ({string.Join(", ", parameters.Select(p => $"-vector.{p}"))});",
                $"public override string ToString() => $\"({string.Join(", ", parameters.Select(p => $"{{{p}}}"))})\";",
            ]),
        ]);
        return code;
    }

    private static void GenerateMatrix(IncrementalGeneratorInitializationContext context)
    {
        for (var rowDimension = 2; rowDimension <= MaxDimension; rowDimension++)
        {
            for (var columnDimension = 2; columnDimension <= MaxDimension; columnDimension++)
            {
                if (rowDimension is 1 && columnDimension is 1)
                    continue;

                foreach (var numberType in NumberTypes)
                {
                    var typeName = $"Matrix{rowDimension}X{columnDimension}{numberType.Suffix}";
                    var code = GetMatrixCode(typeName, rowDimension, columnDimension, numberType, $"Matrix{columnDimension}X{rowDimension}{numberType.Suffix}");
                    context.RegisterPostInitializationOutput(ctx => ctx.AddSource($"{typeName}.g.cs", code.ToString()));
                }
            }
        }
    }

    private static CodeBase GetMatrixCode(string typeName, int rowDimension, int columnDimension, NumberType numberType, string transposeTypeName)
    {
        var sqrtMethod = numberType.SqrtMethod;
        var rowVectorType = $"Vector{columnDimension}{numberType.Suffix}";
        var columnVectorType = $"Vector{rowDimension}{numberType.Suffix}";

        CodeBase code = new CodeCombination([
            "// <auto-generated/>",
            "namespace SeWzc.Numerics.Matrix;",
            $"""
            /// <summary>
            /// {columnDimension} 行 {rowDimension} 列的矩阵。
            /// </summary>
            """,
            new CodeBlock(
                $"""
                public partial record {typeName}({string.Join(", ", RangeSelect(rowDimension, columnDimension, (i, j) => $"{numberType.Name} M{i + 1}{j + 1}"))})
                   : IMatrix<{typeName}, {rowVectorType}, {columnVectorType}, {numberType.Name}, {transposeTypeName}>
                """,
                [
                    // 抑制警告
                    "#pragma warning disable CS1591",

                    // 零矩阵
                    $"public static {typeName} Zero => new({string.Join(", ", RangeSelect(rowDimension, columnDimension, (_, _) => "0"))});",

                    // 行向量
                    ..RangeSelect<CodeBase>(rowDimension,
                        i => $"public {rowVectorType} Row{i + 1} => new({string.Join(", ", RangeSelect(columnDimension, j => $"M{i + 1}{j + 1}"))});"),

                    // 列向量
                    ..RangeSelect<CodeBase>(columnDimension,
                        i => $"public {columnVectorType} Column{i + 1} => new({string.Join(", ", RangeSelect(rowDimension, j => $"M{j + 1}{i + 1}"))});"),

                    new CodeCombination([
                        $"public {numberType.Name} this[int row, int column] => (row, column) switch",
                        "{",
                        ..RangeSelect(rowDimension, columnDimension, (i, j) => $"    ({i}, {j}) => M{i + 1}{j + 1},"),
                        $"    (< 0 or >= {rowDimension}, _) => throw new ArgumentOutOfRangeException(nameof(row)),",
                        "    _ => throw new ArgumentOutOfRangeException(nameof(column)),",
                        "};",
                    ]),
                    new CodeCombination(
                    [
                        $"public {rowVectorType} GetRow(int row) => row switch",
                        "{",
                        ..RangeSelect(rowDimension, i => $"    {i} => Row{i + 1},"),
                        "    _ => throw new ArgumentOutOfRangeException(nameof(row)),",
                        "};",
                    ]),
                    new CodeCombination(
                    [
                        $"public {columnVectorType} GetColumn(int column) => column switch",
                        "{",
                        ..RangeSelect(columnDimension, i => $"    {i} => Column{i + 1},"),
                        "    _ => throw new ArgumentOutOfRangeException(nameof(column)),",
                        "};",
                    ]),

                    // 转置矩阵
                    $"public {transposeTypeName} Transpose => {transposeTypeName}.CreateFromColumnVectors({string.Join(", ", RangeSelect(rowDimension, i => $"Row{i + 1}"))});",

                    // Frobenius范数
                    $"public {numberType.Name} FrobeniusNorm => {sqrtMethod}({string.Join("+ ", RangeSelect(rowDimension, i => $"Row{i + 1}.LengthSquared"))});",

                    $"public override string ToString() => $\"({string.Join(", ", RangeSelect(rowDimension, i => $"{{Row{i + 1}}}"))})\";",

                    "#region 静态函数",

                    // 从行向量创建
                    new CodeBlock(
                        $"public static {typeName} CreateFromRowVectors({string.Join(", ", RangeSelect(rowDimension, i => $"{rowVectorType} row{i + 1}"))})",
                        [$"return new({string.Join(", ", RangeSelect(rowDimension, columnDimension, (i, j) => $"row{i + 1}.{AxisNames[j]}"))});"]),
                    // 从列向量创建
                    new CodeBlock(
                        $"public static {typeName} CreateFromColumnVectors({string.Join(", ", RangeSelect(columnDimension, i => $"{columnVectorType} column{i + 1}"))})",
                        [$"return new({string.Join(", ", RangeSelect(rowDimension, columnDimension, (i, j) => $"column{j + 1}.{AxisNames[i]}"))});"]),

                    "#endregion 静态函数",

                    "#region 运算符重载",

                    // 运算符重载
                    $"public static {typeName} operator +({typeName} matrix1, {typeName} matrix2) => CreateFromRowVectors({string.Join(", ", RangeSelect(rowDimension, i => $"matrix1.Row{i + 1} + matrix2.Row{i + 1}"))});",
                    $"public static {typeName} operator -({typeName} matrix1, {typeName} matrix2) => CreateFromRowVectors({string.Join(", ", RangeSelect(rowDimension, i => $"matrix1.Row{i + 1} - matrix2.Row{i + 1}"))});",
                    $"public static {typeName} operator *({typeName} matrix, {numberType.Name} scalar) => CreateFromRowVectors({string.Join(", ", RangeSelect(rowDimension, i => $"matrix.Row{i + 1} * scalar"))});",
                    $"public static {typeName} operator *({numberType.Name} scalar, {typeName} matrix) => CreateFromRowVectors({string.Join(", ", RangeSelect(rowDimension, i => $"matrix.Row{i + 1} * scalar"))});",
                    $"public static {rowVectorType} operator *({columnVectorType} vector, {typeName} matrix) => new({string.Join(", ", RangeSelect(columnDimension, i => $"vector.Dot(matrix.Column{i + 1})"))});",
                    $"public static {columnVectorType} operator *({typeName} matrix, {rowVectorType} vector) => new({string.Join(", ", RangeSelect(rowDimension, i => $"vector.Dot(matrix.Row{i + 1})"))});",
                    $"public static {typeName} operator /({typeName} matrix, {numberType.Name} scalar) => CreateFromRowVectors({string.Join(", ", RangeSelect(rowDimension, i => $"matrix.Row{i + 1} / scalar"))});",
                    $"public static {typeName} operator -({typeName} matrix) => CreateFromRowVectors({string.Join(", ", RangeSelect(rowDimension, i => $"-matrix.Row{i + 1}"))});",

                    ..Enumerable.Range(2, MaxDimension - 2)
                        .Select(newColumnDimension =>
                            $"public static Matrix{rowDimension}X{newColumnDimension}{numberType.Suffix} operator *({typeName} matrix1, Matrix{columnDimension}X{newColumnDimension}{numberType.Suffix} matrix2) => " +
                            $"new({string.Join(", ", RangeSelect(rowDimension, newColumnDimension, (i, j) => $"matrix1.Row{i + 1} * matrix2.Column{j + 1}"))});"),

                    "#endregion 运算符重载",
                ]),
        ]);
        return code;
    }

    private static ImmutableArray<TResult> RangeSelect<TResult>(int count, Func<int, TResult> selector)
    {
        var result = new TResult[count];
        for (var i = 0; i < count; i++)
            result[i] = selector(i);

        return ImmutableCollectionsMarshal.AsImmutableArray(result);
    }

    private static ImmutableArray<TResult> RangeSelect<TResult>(int count1, int count2, Func<int, int, TResult> selector)
    {
        var result = new TResult[count1 * count2];
        for (var i = 0; i < count1; i++)
        {
            for (var j = 0; j < count2; j++)
                result[i * count2 + j] = selector(i, j);
        }

        return ImmutableCollectionsMarshal.AsImmutableArray(result);
    }

    #endregion

    #region 成员方法

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        GenerateVector(context);
        GenerateMatrix(context);
    }

    #endregion

    #region 内部类

    private sealed record NumberType(string Name, string Suffix, string SqrtMethod);

    #endregion
}
