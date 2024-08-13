using System;
using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using SeWzc.Numerics.Matrix;

namespace SeWzc.Numerics.Tests;

internal static class MatrixFactory<TMatrix, TRow, TColumn, TNum, TTranspose>
    where TMatrix : IMatrix<TMatrix, TRow, TColumn, TNum, TTranspose>
    where TRow : unmanaged, IVector<TRow, TNum>
    where TColumn : unmanaged, IVector<TColumn, TNum>
    where TNum : unmanaged, IFloatingPoint<TNum>
    where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TMatrix>
{
    #region 静态变量

    public static ImmutableArray<TMatrix> RandomMatrixes1 { get; } = NumFactory.RandomCreateRange([TMatrix.Zero], CreateRandomMatrix, new Random(0x5f93c44a));
    public static ImmutableArray<TMatrix> RandomMatrixes2 { get; } = NumFactory.RandomCreateRange([TMatrix.Zero], CreateRandomMatrix, new Random(0x7722cd05));

    #endregion

    #region 静态方法

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TMatrix> action)
    {
        for (var i = 0; i < NumFactory.Count; i++)
            action(RandomMatrixes1[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TMatrix, TNum> action)
    {
        for (var i = 0; i < NumFactory.Count; i++)
            action(RandomMatrixes1[i], NumFactory<TNum>.RandomNum[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TMatrix, TRow> action)
    {
        for (var i = 0; i < NumFactory.Count; i++)
            action(RandomMatrixes1[i], VectorFactory<TRow, TNum>.RandomVectors1[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TColumn, TMatrix> action)
    {
        for (var i = 0; i < NumFactory.Count; i++)
            action(VectorFactory<TColumn, TNum>.RandomVectors1[i], RandomMatrixes2[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TMatrix, TMatrix> action)
    {
        for (var i = 0; i < NumFactory.Count; i++)
            action(RandomMatrixes1[i], RandomMatrixes2[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TMatrix CreateRandomMatrix(Random random)
    {
        var rowCount = TMatrix.RowCount;
        var columnCount = TMatrix.ColumnCount;

        var objects = new object[rowCount * columnCount];
        for (var i = 0; i < rowCount; i++)
        {
            for (var j = 0; j < columnCount; j++)
                objects[i * columnCount + j] = NumFactory<TNum>.NextNum(random);
        }

        return (TMatrix)Activator.CreateInstance(typeof(TMatrix), objects)!;
    }

    #endregion
}
