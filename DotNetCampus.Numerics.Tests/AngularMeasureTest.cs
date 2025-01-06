using System;
using JetBrains.Annotations;
using Xunit;

namespace DotNetCampus.Numerics.Tests;

[TestSubject(typeof(AngularMeasure))]
public class AngularMeasureTest
{
    #region 静态变量

    public static readonly TheoryData<AngularMeasure> AngleData = new([
        AngularMeasure.Zero,
        AngularMeasure.HalfPi,
        AngularMeasure.Pi,
        AngularMeasure.HalfPi + AngularMeasure.Pi,
        AngularMeasure.Tau,
        AngularMeasure.FromRadian(1),
        AngularMeasure.FromDegree(45),
        AngularMeasure.FromDegree(1),
        AngularMeasure.FromDegree(100),
    ]);

    public static readonly TheoryData<AngularMeasure, AngularMeasure, AngularMeasure> AngleAddTestData = new()
    {
        { AngularMeasure.Degree90, AngularMeasure.Degree90, AngularMeasure.Degree180 },
        { AngularMeasure.Degree90, AngularMeasure.Degree180, AngularMeasure.Degree270 },
        { AngularMeasure.Degree180, AngularMeasure.Degree180, AngularMeasure.Degree360 },
        { AngularMeasure.Degree270, AngularMeasure.Degree90, AngularMeasure.Degree360 },
        { AngularMeasure.HalfPi, AngularMeasure.Pi, AngularMeasure.OneAndHalfPi },
    };

    public static readonly TheoryData<AngularMeasure, double, AngularMeasure> AngleMultiplyTestData = new()
    {
        { AngularMeasure.HalfPi, 2, AngularMeasure.Pi },
        { AngularMeasure.Pi, 2, AngularMeasure.Tau },
        { AngularMeasure.HalfPi, 4, AngularMeasure.Tau },
    };

    // (角1, 角度2, 角度1.CompareTo(角度2))
    public static readonly TheoryData<AngularMeasure, AngularMeasure, int> AngleCompareTestData = new()
    {
        { AngularMeasure.Zero, AngularMeasure.HalfPi, -1 },
        { AngularMeasure.HalfPi, AngularMeasure.Pi, -1 },
        { AngularMeasure.Pi, AngularMeasure.HalfPi + AngularMeasure.Pi, -1 },
        { AngularMeasure.HalfPi + AngularMeasure.Pi, AngularMeasure.Tau, -1 },
        { AngularMeasure.Zero, AngularMeasure.Zero, 0 },
        { AngularMeasure.HalfPi, AngularMeasure.HalfPi, 0 },
        { AngularMeasure.Pi, AngularMeasure.Pi, 0 },
        { AngularMeasure.HalfPi + AngularMeasure.Pi, AngularMeasure.HalfPi + AngularMeasure.Pi, 0 },
        { AngularMeasure.Tau, AngularMeasure.Tau, 0 },
        { AngularMeasure.HalfPi, AngularMeasure.Zero, 1 },
        { AngularMeasure.Pi, AngularMeasure.HalfPi, 1 },
        { AngularMeasure.HalfPi + AngularMeasure.Pi, AngularMeasure.Pi, 1 },
        { AngularMeasure.Tau, AngularMeasure.HalfPi + AngularMeasure.Pi, 1 },
    };

    #endregion

    #region 成员方法

    [Theory(DisplayName = "测试从角度创建角。")]
    [MemberData(nameof(AngleData))]
    public void FromDegreeTest(AngularMeasure angle)
    {
        Assert.Equal(angle, AngularMeasure.FromDegree(angle.Degree), NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试从弧度创建角。")]
    [MemberData(nameof(AngleData))]
    public void FromRadianTest(AngularMeasure angle)
    {
        Assert.Equal(angle, AngularMeasure.FromRadian(angle.Radian), NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试角的加法。")]
    [MemberData(nameof(AngleAddTestData))]
    public void AddTest(AngularMeasure angle1, AngularMeasure angle2, AngularMeasure expected)
    {
        Assert.Equal(expected, angle1 + angle2, NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试角的减法。")]
    [MemberData(nameof(AngleAddTestData))]
    public void SubtractTest(AngularMeasure expected, AngularMeasure angle1, AngularMeasure angle2)
    {
        Assert.Equal(expected, angle2 - angle1, NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试角的乘法。")]
    [MemberData(nameof(AngleMultiplyTestData))]
    public void MultiplyTest(AngularMeasure angle, double num, AngularMeasure expected)
    {
        Assert.Equal(expected, angle * num, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected, num * angle, NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试角的数除。")]
    [MemberData(nameof(AngleMultiplyTestData))]
    public void DivideNumTest(AngularMeasure expected, double num, AngularMeasure angle)
    {
        Assert.Equal(expected, angle / num, NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试角的除法。")]
    [MemberData(nameof(AngleMultiplyTestData))]
    public void DivideTest(AngularMeasure angle1, double expected, AngularMeasure angle2)
    {
        Assert.Equal(expected, angle2 / angle1, NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试角的比较。")]
    [MemberData(nameof(AngleCompareTestData))]
    public void CompareTest(AngularMeasure angle1, AngularMeasure angle2, int expected)
    {
        Assert.Equal(expected, angle1.CompareTo(angle2));
        Assert.Equal(expected < 0, angle1 < angle2);
        Assert.Equal(expected <= 0, angle1 <= angle2);
        Assert.Equal(expected > 0, angle1 > angle2);
        Assert.Equal(expected >= 0, angle1 >= angle2);
        Assert.Equal(expected == 0, angle1 == angle2);
        Assert.Equal(expected != 0, angle1 != angle2);
    }

    [Theory(DisplayName = "测试角的转换。")]
    [MemberData(nameof(AngleData))]
    public void ConvertTest(AngularMeasure angle)
    {
        Assert.Equal(angle, AngularMeasure.FromDegree(angle.Degree), NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(angle, AngularMeasure.FromRadian(angle.Radian), NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试三角函数。")]
    [MemberData(nameof(AngleData))]
    public void TrigonometricFunctions(AngularMeasure angle)
    {
        Assert.Equal(Math.Sin(angle.Radian), angle.Sin(), NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(Math.Cos(angle.Radian), angle.Cos(), NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(Math.Tan(angle.Radian), angle.Tan(), NumericsEqualHelper.IsAlmostEqual);

        Assert.Equal(1, angle.Sin().Square() + angle.Cos().Square(), NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(angle.Sin() / angle.Cos(), angle.Tan(), NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试角的单位向量。")]
    [MemberData(nameof(AngleData))]
    public void UnitVectorTest(AngularMeasure angle)
    {
        var unitVector = angle.UnitVector;

        Assert.Equal(angle, unitVector.Angle.Normalized, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(1.0, unitVector.Length, NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试角的规范化。")]
    [InlineData(0, 0)]
    [InlineData(Math.Tau, 0)]
    [InlineData(Math.Tau / 2, Math.Tau / 2)]
    [InlineData(-Math.Tau / 4, Math.Tau * 3 / 4)]
    [InlineData(Math.Tau * 3 / 2, Math.Tau / 2)]
    public void NormalizedTest(double radian, double expected)
    {
        var angle = AngularMeasure.FromRadian(radian);
        Assert.Equal(expected, angle.Normalized.Radian, NumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试角是否几乎是 π/2 的整数倍角。")]
    [InlineData(0, true)]
    [InlineData(1e-13, true)]
    [InlineData(-1e-13, true)]
    [InlineData(1e-3, false)]
    [InlineData(-1e-3, false)]
    [InlineData(Math.PI / 2, true)]
    [InlineData(Math.PI / 2 + 1e-13, true)]
    [InlineData(Math.PI / 2 - 1e-13, true)]
    [InlineData(Math.PI / 2 + 1e-3, false)]
    [InlineData(Math.PI / 2 - 1e-3, false)]
    [InlineData(Math.PI, true)]
    [InlineData(Math.PI * 3 / 2, true)]
    public void IsAlmostIntegerMultipleOfHalfPiTest(double radian, bool expected)
    {
        var angle = AngularMeasure.FromRadian(radian);
        Assert.Equal(expected, angle.IsAlmostIntegerMultipleOfHalfPi());
    }

    #endregion
}
