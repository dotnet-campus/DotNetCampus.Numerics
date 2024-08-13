using JetBrains.Annotations;
using Xunit;

namespace SeWzc.Numerics.Tests;

[TestSubject(typeof(Interval<>))]
public class IntervalTest
{
    #region 成员方法

    [Theory(DisplayName = "区间长度测试。")]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 0)]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 0)]
    public void LengthTest(double start, double end, double expected)
    {
        var interval = new Interval<double>(start, end);
        Assert.Equal(expected, interval.Length);
    }

    [Theory(DisplayName = "区间包含测试。")]
    [InlineData(0, 1, 0, true)]
    [InlineData(0, 1, 1, true)]
    [InlineData(0, 1, 0.5, true)]
    [InlineData(0, 1, -1, false)]
    [InlineData(0, 1, 2, false)]
    public void ContainsTest(double start, double end, double value, bool expected)
    {
        var interval = new Interval<double>(start, end);
        Assert.Equal(expected, interval.Contains(value));
    }

    [Theory(DisplayName = "区间是否为空集测试。")]
    [InlineData(0, 1, false)]
    [InlineData(1, 0, true)]
    [InlineData(0, 0, false)]
    [InlineData(1, 1, false)]
    [InlineData(1, 0.99, true)]
    public void IsEmptyTest(double start, double end, bool expected)
    {
        var interval = new Interval<double>(start, end);
        Assert.Equal(expected, interval.IsEmpty);
    }

    [Theory(DisplayName = "区间创建测试。")]
    [InlineData(0, 1, 0, 1)]
    [InlineData(1, 0, 0, 1)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(1, 1, 1, 1)]
    public void CreateTest(double a, double b, double start, double end)
    {
        var interval = Interval<double>.Create(a, b);
        Assert.Equal(start, interval.Start);
        Assert.Equal(end, interval.End);
    }

    [Theory(DisplayName = "区间子集测试。")]
    [InlineData(0, 1, 0, 1, true)]
    [InlineData(0, 1, 0, 0.5, true)]
    [InlineData(0, 1, 0.5, 1, true)]
    [InlineData(0, 1, 0.5, 0.5, true)]
    [InlineData(0, 1, -1, 2, false)]
    [InlineData(0, 1, 0, 2, false)]
    [InlineData(0, 1, -1, 1, false)]
    [InlineData(0, 1, 0, 1.1, false)]
    [InlineData(0, 1, 1, 0, true)]
    [InlineData(0, 0, 0, 1, false)]
    [InlineData(0, 0, 0, 0, true)]
    [InlineData(0, 0, 1, 1, false)]
    [InlineData(0, 0, 1, 0, true)]
    public void SubsetTest(double start, double end, double start2, double end2, bool expected)
    {
        var interval = new Interval<double>(start, end);
        var interval2 = new Interval<double>(start2, end2);
        Assert.Equal(expected, interval2.IsSubsetOf(interval));
        Assert.Equal(expected, interval.IsSupersetOf(interval2));
    }

    [Theory(DisplayName = "区间真子集测试。")]
    [InlineData(0, 1, 0, 1, false)]
    [InlineData(0, 1, 0, 0.5, true)]
    [InlineData(0, 1, 0.5, 1, true)]
    [InlineData(0, 1, 0.5, 0.5, true)]
    [InlineData(0, 1, -1, 2, false)]
    [InlineData(0, 1, 0, 2, false)]
    [InlineData(0, 1, -1, 1, false)]
    [InlineData(0, 1, 0, 1.1, false)]
    public void ProperSubsetTest(double start, double end, double start2, double end2, bool expected)
    {
        var interval = new Interval<double>(start, end);
        var interval2 = new Interval<double>(start2, end2);
        Assert.Equal(expected, interval2.IsProperSubsetOf(interval));
        Assert.Equal(expected, interval.IsProperSupersetOf(interval2));
    }

    #endregion
}
