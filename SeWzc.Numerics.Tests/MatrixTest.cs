using System;
using System.Collections.Immutable;
using System.Numerics;
using System.Reflection;
using JetBrains.Annotations;
using SeWzc.Numerics.Matrix;
using Xunit;

namespace SeWzc.Numerics.Tests;

[TestSubject(typeof(IMatrix<,,,,>))]
public class MatrixTest
{
    #region 静态变量

    private static readonly ImmutableArray<Type> MatrixTypes =
    [
        typeof(Matrix2X2D),
        typeof(Matrix2X3D),
        typeof(Matrix2X4D),
        typeof(Matrix3X2D),
        typeof(Matrix3X3D),
        typeof(Matrix3X4D),
        typeof(Matrix4X2D),
        typeof(Matrix4X3D),
        typeof(Matrix4X4D),
        typeof(Matrix2X2F),
        typeof(Matrix2X3F),
        typeof(Matrix2X4F),
        typeof(Matrix3X2F),
        typeof(Matrix3X3F),
        typeof(Matrix3X4F),
        typeof(Matrix4X2F),
        typeof(Matrix4X3F),
        typeof(Matrix4X4F),
    ];

    #endregion

    #region 静态方法

    private static void Test(string methodName)
    {
        foreach (var genericTypeArguments in MatrixTypes)
        {
            var genericArguments = genericTypeArguments.GetInterface(typeof(IMatrix<,,,,>).Name)!.GetGenericArguments();
            typeof(MatrixTest)
                .GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(genericArguments)
                .Invoke(null, Array.Empty<object?>());
        }
    }

    #endregion

    #region 测试方法

    [Fact(DisplayName = "测试矩阵的加法。")]
    public void AddTest()
    {
        Test(nameof(AddTestGeneric));
    }

    private static void AddTestGeneric<TMatrix, TRow, TColumn, TNum, TTranspose>()
        where TMatrix : IMatrix<TMatrix, TRow, TColumn, TNum, TTranspose>
        where TRow : unmanaged, IVector<TRow, TNum>
        where TColumn : unmanaged, IVector<TColumn, TNum>
        where TNum : unmanaged, INumber<TNum>
        where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TMatrix>
    {
        MatrixFactory<TMatrix, TRow, TColumn, TNum, TTranspose>.Test((a, b) =>
        {
            var actual = a + b;
            for (var i = 0; i < TMatrix.RowCount; i++)
            {
                for (var j = 0; j < TMatrix.ColumnCount; j++)
                    Assert.Equal(a[i, j] + b[i, j], actual[i, j]);
            }
        });
    }

    [Fact(DisplayName = "测试矩阵的减法。")]
    public void SubtractTest()
    {
        Test(nameof(SubtractTestGeneric));
    }

    private static void SubtractTestGeneric<TMatrix, TRow, TColumn, TNum, TTranspose>()
        where TMatrix : IMatrix<TMatrix, TRow, TColumn, TNum, TTranspose>
        where TRow : unmanaged, IVector<TRow, TNum>
        where TColumn : unmanaged, IVector<TColumn, TNum>
        where TNum : unmanaged, INumber<TNum>
        where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TMatrix>
    {
        MatrixFactory<TMatrix, TRow, TColumn, TNum, TTranspose>.Test((a, b) =>
        {
            var actual = a - b;
            for (var i = 0; i < TMatrix.RowCount; i++)
            {
                for (var j = 0; j < TMatrix.ColumnCount; j++)
                    Assert.Equal(a[i, j] - b[i, j], actual[i, j]);
            }
        });
    }

    [Fact(DisplayName = "测试矩阵右乘向量。")]
    public void RightMultiplyVectorTest()
    {
        Test(nameof(RightMultiplyVectorTestGeneric));
    }

    private static void RightMultiplyVectorTestGeneric<TMatrix, TRow, TColumn, TNum, TTranspose>()
        where TMatrix : IMatrix<TMatrix, TRow, TColumn, TNum, TTranspose>
        where TRow : unmanaged, IVector<TRow, TNum>
        where TColumn : unmanaged, IVector<TColumn, TNum>
        where TNum : unmanaged, INumber<TNum>
        where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TMatrix>
    {
        MatrixFactory<TMatrix, TRow, TColumn, TNum, TTranspose>.Test((a, b) =>
        {
            var actual = a * b;
            for (var column = 0; column < TMatrix.ColumnCount; column++)
            {
                for (var row = 0; row < TMatrix.RowCount; row++)
                {
                    TNum sum = default;
                    for (var i = 0; i < TColumn.Dimension; i++)
                        sum += a[i] * b[i, column];
                    Assert.Equal(sum, actual[column]);
                }

                Assert.Equal(a * b.GetColumn(column), actual[column]);
            }
        });
    }

    [Fact(DisplayName = "测试矩阵左乘向量。")]
    public void LeftMultiplyVectorTest()
    {
        Test(nameof(LeftMultiplyVectorTestGeneric));
    }

    private static void LeftMultiplyVectorTestGeneric<TMatrix, TRow, TColumn, TNum, TTranspose>()
        where TMatrix : IMatrix<TMatrix, TRow, TColumn, TNum, TTranspose>
        where TRow : unmanaged, IVector<TRow, TNum>
        where TColumn : unmanaged, IVector<TColumn, TNum>
        where TNum : unmanaged, INumber<TNum>
        where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TMatrix>
    {
        MatrixFactory<TMatrix, TRow, TColumn, TNum, TTranspose>.Test((a, b) =>
        {
            var actual = a * b;
            for (var row = 0; row < TMatrix.RowCount; row++)
            {
                for (var column = 0; column < TMatrix.ColumnCount; column++)
                {
                    TNum sum = default;
                    for (var i = 0; i < TRow.Dimension; i++)
                        sum += a[row, i] * b[i];
                    Assert.Equal(sum, actual[row]);
                }

                Assert.Equal(a.GetRow(row) * b, actual[row]);
            }
        });
    }

    [Fact(DisplayName = "测试矩阵的数乘。")]
    public void MultiplyNumberTest()
    {
        Test(nameof(MultiplyNumberTestGeneric));
    }

    private static void MultiplyNumberTestGeneric<TMatrix, TRow, TColumn, TNum, TTranspose>()
        where TMatrix : IMatrix<TMatrix, TRow, TColumn, TNum, TTranspose>
        where TRow : unmanaged, IVector<TRow, TNum>
        where TColumn : unmanaged, IVector<TColumn, TNum>
        where TNum : unmanaged, INumber<TNum>
        where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TMatrix>
    {
        MatrixFactory<TMatrix, TRow, TColumn, TNum, TTranspose>.Test((a, b) =>
        {
            var actual = a * b;
            for (var i = 0; i < TMatrix.RowCount; i++)
            {
                for (var j = 0; j < TMatrix.ColumnCount; j++)
                    Assert.Equal(a[i, j] * b, actual[i, j]);
            }

            actual = b * a;
            for (var i = 0; i < TMatrix.RowCount; i++)
            {
                for (var j = 0; j < TMatrix.ColumnCount; j++)
                    Assert.Equal(b * a[i, j], actual[i, j]);
            }
        });
    }

    [Fact(DisplayName = "测试矩阵的数除。")]
    public void DivideNumberTest()
    {
        Test(nameof(DivideNumberTestGeneric));
    }

    private static void DivideNumberTestGeneric<TMatrix, TRow, TColumn, TNum, TTranspose>()
        where TMatrix : IMatrix<TMatrix, TRow, TColumn, TNum, TTranspose>
        where TRow : unmanaged, IVector<TRow, TNum>
        where TColumn : unmanaged, IVector<TColumn, TNum>
        where TNum : unmanaged, INumber<TNum>
        where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TMatrix>
    {
        MatrixFactory<TMatrix, TRow, TColumn, TNum, TTranspose>.Test((a, b) =>
        {
            var actual = a / b;
            for (var i = 0; i < TMatrix.RowCount; i++)
            {
                for (var j = 0; j < TMatrix.ColumnCount; j++)
                    Assert.Equal(a[i, j] / b, actual[i, j]);
            }
        });
    }

    [Fact(DisplayName = "测试矩阵的转置。")]
    public void TransposeTest()
    {
        Test(nameof(TransposeTestGeneric));
    }

    private static void TransposeTestGeneric<TMatrix, TRow, TColumn, TNum, TTranspose>()
        where TMatrix : IMatrix<TMatrix, TRow, TColumn, TNum, TTranspose>
        where TRow : unmanaged, IVector<TRow, TNum>
        where TColumn : unmanaged, IVector<TColumn, TNum>
        where TNum : unmanaged, INumber<TNum>
        where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TMatrix>
    {
        MatrixFactory<TMatrix, TRow, TColumn, TNum, TTranspose>.Test(a =>
        {
            var actual = a.Transpose;
            for (var i = 0; i < TMatrix.RowCount; i++)
            {
                for (var j = 0; j < TMatrix.ColumnCount; j++)
                    Assert.Equal(a[i, j], actual[j, i]);
            }
        });
    }

    [Fact(DisplayName = "测试矩阵的Frobenius范数。")]
    public void FrobeniusNormTest()
    {
        Test(nameof(FrobeniusNormTestGeneric));
    }

    private static void FrobeniusNormTestGeneric<TMatrix, TRow, TColumn, TNum, TTranspose>()
        where TMatrix : IMatrix<TMatrix, TRow, TColumn, TNum, TTranspose>
        where TRow : unmanaged, IVector<TRow, TNum>
        where TColumn : unmanaged, IVector<TColumn, TNum>
        where TNum : unmanaged, INumber<TNum>
        where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TMatrix>
    {
        MatrixFactory<TMatrix, TRow, TColumn, TNum, TTranspose>.Test(m =>
        {
            var actual = m.FrobeniusNorm;
            var sum = default(TNum);
            for (var i = 0; i < TMatrix.RowCount; i++)
            {
                for (var j = 0; j < TMatrix.ColumnCount; j++)
                    sum += m[i, j] * m[i, j];
            }

            NumAssert.CloseEqual(sum, actual * actual);
        });
    }

    [Fact(DisplayName = "测试矩阵的负运算。")]
    public void UnaryMinusTest()
    {
        Test(nameof(UnaryMinusTestGeneric));
    }

    private static void UnaryMinusTestGeneric<TMatrix, TRow, TColumn, TNum, TTranspose>()
        where TMatrix : IMatrix<TMatrix, TRow, TColumn, TNum, TTranspose>
        where TRow : unmanaged, IVector<TRow, TNum>
        where TColumn : unmanaged, IVector<TColumn, TNum>
        where TNum : unmanaged, INumber<TNum>
        where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TMatrix>
    {
        MatrixFactory<TMatrix, TRow, TColumn, TNum, TTranspose>.Test(m =>
        {
            var actual = -m;
            for (var i = 0; i < TMatrix.RowCount; i++)
            {
                for (var j = 0; j < TMatrix.ColumnCount; j++)
                    Assert.Equal(-m[i, j], actual[i, j]);
            }
        });
    }

    #endregion
}