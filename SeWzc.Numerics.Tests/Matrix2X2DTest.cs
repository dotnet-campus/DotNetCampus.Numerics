using System;
using JetBrains.Annotations;
using SeWzc.Numerics.Matrix;
using Xunit;

namespace SeWzc.Numerics.Tests;

[TestSubject(typeof(Matrix2X2D))]
public class Matrix2X2DTest
{
    #region 静态变量

    public static readonly TheoryData<Matrix2X2D> InvertibleMatrix = new([
        new Matrix2X2D(1, 2, 3, 4),
        new Matrix2X2D(1, 0, 0, 1),
        new Matrix2X2D(0, 1, 1, 0),
    ]);

    public static readonly TheoryData<Matrix2X2D> NonInvertibleMatrix = new([
        new Matrix2X2D(0, 0, 0, 0),
        new Matrix2X2D(1, 1, 1, 1),
        new Matrix2X2D(2, 4, 1, 2),
    ]);

    #endregion

    #region 成员方法

    [Theory(DisplayName = "可逆矩阵求逆测试。")]
    [MemberData(nameof(InvertibleMatrix))]
    public void TestInverse(Matrix2X2D matrix)
    {
        ArgumentNullException.ThrowIfNull(matrix);

        var inverse = matrix.Inverse();
        var actual = inverse * matrix;

        NumAssert.NotCloseZero(matrix.Determinant);

        NumAssert.CloseEqual(1, actual.M11);
        NumAssert.CloseZero(actual.M12);
        NumAssert.CloseZero(actual.M21);
        NumAssert.CloseEqual(1, actual.M22);
    }

    [Theory(DisplayName = "不可逆矩阵求逆测试。")]
    [MemberData(nameof(NonInvertibleMatrix))]
    public void TestNonInvertible(Matrix2X2D matrix)
    {
        Assert.Throws<MatrixNonInvertibleException>(() => matrix.Inverse());
    }

    #endregion
}