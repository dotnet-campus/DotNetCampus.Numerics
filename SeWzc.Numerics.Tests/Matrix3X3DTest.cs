using System;
using JetBrains.Annotations;
using SeWzc.Numerics.Matrix;
using Xunit;

namespace SeWzc.Numerics.Tests;

[TestSubject(typeof(Matrix3X3D))]
public class Matrix3X3DTest
{
    #region 静态变量

    public static readonly TheoryData<Matrix3X3D> InvertibleMatrix = new([
        new Matrix3X3D(1, 0, 0, 0, 1, 0, 0, 0, 1),
        new Matrix3X3D(0, 0, 1, 0, 1, 0, 1, 0, 0),
        new Matrix3X3D(2, -1, 1, 3, 2, -1, 1, 6, -1),
    ]);

    public static readonly TheoryData<Matrix3X3D> NonInvertibleMatrix = new([
        new Matrix3X3D(0, 0, 0, 0, 0, 0, 0, 0, 0),
        new Matrix3X3D(1, 1, 1, 1, 1, 1, 1, 1, 1),
        new Matrix3X3D(1, 2, 3, 4, 5, 6, 7, 8, 9),
        new Matrix3X3D(2, 4, 6, 1, 2, 3, 3, 6, 9),
    ]);

    #endregion

    #region 成员方法

    [Theory(DisplayName = "可逆矩阵求逆测试。")]
    [MemberData(nameof(InvertibleMatrix))]
    public void TestInverse(Matrix3X3D matrix)
    {
        ArgumentNullException.ThrowIfNull(matrix);

        var inverse = matrix.Invert();
        var actual = inverse * matrix;

        NumAssert.NotCloseZero(matrix.Determinant);
        NumAssert.CloseEqual(1, actual.M11);
        NumAssert.CloseZero(actual.M12);
        NumAssert.CloseZero(actual.M13);
        NumAssert.CloseZero(actual.M21);
        NumAssert.CloseEqual(1, actual.M22);
        NumAssert.CloseZero(actual.M23);
        NumAssert.CloseZero(actual.M31);
        NumAssert.CloseZero(actual.M32);
        NumAssert.CloseEqual(1, actual.M33);
    }

    [Theory(DisplayName = "不可逆矩阵求逆测试。")]
    [MemberData(nameof(NonInvertibleMatrix))]
    public void TestNonInvertible(Matrix3X3D matrix)
    {
        ArgumentNullException.ThrowIfNull(matrix);

        Assert.Throws<MatrixNonInvertibleException>(matrix.Invert);
    }

    #endregion
}
