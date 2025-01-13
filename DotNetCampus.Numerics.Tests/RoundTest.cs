using JetBrains.Annotations;
using Xunit;

namespace DotNetCampus.Numerics.Tests;

[TestSubject(typeof(FloatingPointNumberExtensions))]
public class RoundTest
{
    #region 私有字段

    /// <summary>
    /// 包含中点舍入测试时通用的数据。
    /// </summary>
    public static TheoryData<double, double> RoundHalfCommonData { get; } = new()
    {
        { 0, 0 },
        { 1.2, 1 },
        { -1.4, -1 },
        { 2.8, 3 },
        { -3.2, -3 },
        { 4.7, 5 },
        // { 0.5, 0 },
        // { -0.5, 0 },
        // { 1.5, 2 },
        // { -1.5, -2 },
        // { 2.5, 2 },
        // { -2.5, -2 },
        // { 3.5, 4 },
        // { -3.5, -4 },
        { 3.4999999999999996, 3 },
        { -3.4999999999999996, -3 },
        { 3.5000000000000004, 4 },
        { -3.5000000000000004, -4 },
        { 0.9999999999999999, 1 },
        { -0.9999999999999999, -1 },
        { 1.0000000000000002, 1 },
        { -1.0000000000000002, -1 },
    };

    /// <summary>
    /// 正数向上直接舍入测试数据。
    /// </summary>
    /// <returns></returns>
    public static TheoryData<double, double> PositiveUpData { get; } = new()
    {
        { 1.2, 2 },
        { 2.8, 3 },
        { 4.7, 5 },
        { 0.5, 1 },
        { 1.5, 2 },
        { 2.5, 3 },
        { 3.5, 4 },
        { 0.5, 1 },
        { 1.5, 2 },
        { 2.5, 3 },
        { 3.5, 4 },
        { 3.4999999999999996, 4 },
        { 3.5000000000000004, 4 },
        { 0.9999999999999999, 1 },
        { 1.0000000000000002, 2 },
    };

    /// <summary>
    /// 负数向上直接舍入测试数据。
    /// </summary>
    public static TheoryData<double, double> NegativeUpData { get; } = new()
    {
        { -1.4, -1 },
        { -3.2, -3 },
        { -3.5, -3 },
        { -0.5, 0 },
        { -1.5, -1 },
        { -2.5, -2 },
        { -3.5, -3 },
        { -0.5, 0 },
        { -1.5, -1 },
        { -2.5, -2 },
        { -3.5, -3 },
        { -3.4999999999999996, -3 },
        { -3.5000000000000004, -3 },
        { -0.9999999999999999, 0 },
        { -1.0000000000000002, -1 },
    };

    /// <summary>
    /// 正数向下直接舍入测试数据。
    /// </summary>
    public static TheoryData<double, double> PositiveDownData { get; } = new()
    {
        { 1.2, 1 },
        { 2.8, 2 },
        { 4.7, 4 },
        { 0.5, 0 },
        { 1.5, 1 },
        { 2.5, 2 },
        { 3.5, 3 },
        { 0.5, 0 },
        { 1.5, 1 },
        { 2.5, 2 },
        { 3.5, 3 },
        { 3.4999999999999996, 3 },
        { 3.5000000000000004, 3 },
        { 0.9999999999999999, 0 },
        { 1.0000000000000002, 1 },
    };

    /// <summary>
    /// 负数向下直接舍入测试数据。
    /// </summary>
    public static TheoryData<double, double> NegativeDownData { get; } = new()
    {
        { -1.4, -2 },
        { -3.2, -4 },
        { -3.5, -4 },
        { -0.5, -1 },
        { -1.5, -2 },
        { -2.5, -3 },
        { -3.5, -4 },
        { -0.5, -1 },
        { -1.5, -2 },
        { -2.5, -3 },
        { -3.5, -4 },
        { -3.4999999999999996, -4 },
        { -3.5000000000000004, -4 },
        { -0.9999999999999999, -1 },
        { -1.0000000000000002, -2 },
    };

    #endregion

    #region 成员方法

    [Theory(DisplayName = "银行家舍入测试")]
    [MemberData(nameof(RoundHalfCommonData))]
    [InlineData(0.5, 0)]
    [InlineData(-0.5, 0)]
    [InlineData(1.5, 2)]
    [InlineData(-1.5, -2)]
    [InlineData(2.5, 2)]
    [InlineData(-2.5, -2)]
    [InlineData(3.5, 4)]
    [InlineData(-3.5, -4)]
    public void RoundHalfToEven(double value, double expected)
    {
        Assert.Equal(expected, value.Round(RoundMode.HalfToEven));
    }

    [Theory(DisplayName = "四舍五入测试")]
    [MemberData(nameof(RoundHalfCommonData))]
    [InlineData(0.5, 1)]
    [InlineData(-0.5, -1)]
    [InlineData(1.5, 2)]
    [InlineData(-1.5, -2)]
    [InlineData(2.5, 3)]
    [InlineData(-2.5, -3)]
    [InlineData(3.5, 4)]
    [InlineData(-3.5, -4)]
    public void RoundHalfAwayFromZero(double value, double expected)
    {
        Assert.Equal(expected, value.Round(RoundMode.HalfAwayFromZero));
    }

    [Theory(DisplayName = "五舍六入测试")]
    [MemberData(nameof(RoundHalfCommonData))]
    [InlineData(0.5, 0)]
    [InlineData(-0.5, 0)]
    [InlineData(1.5, 1)]
    [InlineData(-1.5, -1)]
    [InlineData(2.5, 2)]
    [InlineData(-2.5, -2)]
    [InlineData(3.5, 3)]
    [InlineData(-3.5, -3)]
    public void RoundHalfToZero(double value, double expected)
    {
        Assert.Equal(expected, value.Round(RoundMode.HalfToZero));
    }

    [Theory(DisplayName = "中点向上舍入测试")]
    [MemberData(nameof(RoundHalfCommonData))]
    [InlineData(0.5, 1)]
    [InlineData(-0.5, 0)]
    [InlineData(1.5, 2)]
    [InlineData(-1.5, -1)]
    [InlineData(2.5, 3)]
    [InlineData(-2.5, -2)]
    [InlineData(3.5, 4)]
    [InlineData(-3.5, -3)]
    public void RoundHalfUp(double value, double expected)
    {
        Assert.Equal(expected, value.Round(RoundMode.HalfUp));
    }

    [Theory(DisplayName = "中点向下舍入测试")]
    [MemberData(nameof(RoundHalfCommonData))]
    [InlineData(0.5, 0)]
    [InlineData(-0.5, -1)]
    [InlineData(1.5, 1)]
    [InlineData(-1.5, -2)]
    [InlineData(2.5, 2)]
    [InlineData(-2.5, -3)]
    [InlineData(3.5, 3)]
    [InlineData(-3.5, -4)]
    public void RoundHalfDown(double value, double expected)
    {
        Assert.Equal(expected, value.Round(RoundMode.HalfDown));
    }

    [Theory(DisplayName = "直接向远离零舍入测试")]
    [MemberData(nameof(PositiveUpData))]
    [MemberData(nameof(NegativeDownData))]
    [InlineData(0, 0)]
    public void RoundDirectAwayFromZero(double value, double expected)
    {
        Assert.Equal(expected, value.Round(RoundMode.DirectAwayFromZero));
    }

    [Theory(DisplayName = "直接向零舍入测试")]
    [MemberData(nameof(PositiveDownData))]
    [MemberData(nameof(NegativeUpData))]
    [InlineData(0, 0)]
    public void RoundDirectToZero(double value, double expected)
    {
        Assert.Equal(expected, value.Round(RoundMode.DirectToZero));
    }

    [Theory(DisplayName = "直接向上舍入测试")]
    [MemberData(nameof(PositiveUpData))]
    [MemberData(nameof(NegativeUpData))]
    [InlineData(0, 0)]
    public void RoundDirectUp(double value, double expected)
    {
        Assert.Equal(expected, value.Round(RoundMode.DirectUp));
    }

    [Theory(DisplayName = "直接向下舍入测试")]
    [MemberData(nameof(PositiveDownData))]
    [MemberData(nameof(NegativeDownData))]
    [InlineData(0, 0)]
    public void RoundDirectDown(double value, double expected)
    {
        Assert.Equal(expected, value.Round(RoundMode.DirectDown));
    }

    #endregion
}
