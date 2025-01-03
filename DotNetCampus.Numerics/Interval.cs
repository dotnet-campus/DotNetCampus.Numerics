using System.Numerics;

namespace DotNetCampus.Numerics;

/// <summary>
/// 区间。
/// </summary>
/// <remarks>
/// 表示一个 [Start, End] 的闭区间。
/// </remarks>
/// <param name="Start">区间的左端点。</param>
/// <param name="End">区间的右端点。</param>
public readonly record struct Interval<TNum>(TNum Start, TNum End)
    where TNum : unmanaged, IFloatingPoint<TNum>
{
    #region 静态方法

    /// <summary>
    /// 通过区间端点创建一个区间。
    /// </summary>
    /// <param name="a">区间的一个端点。</param>
    /// <param name="b">区间的另一个端点。</param>
    /// <returns></returns>
    public static Interval<TNum> Create(TNum a, TNum b)
    {
        return a > b ? new Interval<TNum>(b, a) : new Interval<TNum>(a, b);
    }

    #endregion

    #region 私有字段

    /// <summary>
    /// 是否是非空集。
    /// </summary>
    /// <remarks>
    /// 未使用构造函数创建的区间是空集。
    /// 区间起点大于等于终点的区间是空集。
    /// </remarks>
    private readonly bool _isNotEmpty = Start <= End;

    #endregion

    #region 属性

    /// <summary>
    /// 是否是空集。
    /// </summary>
    public bool IsEmpty => !_isNotEmpty;

    /// <summary>
    /// 区间长度。
    /// </summary>
    public TNum Length => Start >= End ? TNum.Zero : End - Start;

    #endregion

    #region 成员方法

    /// <summary>
    /// 判断一个值是否在区间内。
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(TNum value)
    {
        return Start <= value && value <= End;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return _isNotEmpty ? $"[{Start}, {End}]" : "Empty";
    }

    #endregion
}

/// <summary>
/// 区间的扩展方法。
/// </summary>
public static class IntervalExtensions
{
    #region 静态方法

    /// <summary>
    /// 是否是一个区间的真子集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsProperSubsetOf<TNum>(this Interval<TNum> interval, Interval<TNum> other)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (other.IsEmpty)
            return false;

        if (interval.IsEmpty)
            return true;

        return interval.Start >= other.Start
               && interval.End <= other.End
               && (!interval.Start.Equals(other.Start) || !interval.End.Equals(other.End));
    }

    /// <summary>
    /// 是否是一个区间的子集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsSubsetOf<TNum>(this Interval<TNum> interval, Interval<TNum> other)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (interval.IsEmpty)
            return true;

        if (other.IsEmpty)
            return false;

        return interval.Start >= other.Start && interval.End <= other.End;
    }

    /// <summary>
    /// 是否是一个区间的真超集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsProperSupersetOf<TNum>(this Interval<TNum> interval, Interval<TNum> other)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        return other.IsProperSubsetOf(interval);
    }

    /// <summary>
    /// 是否是一个区间的超集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsSupersetOf<TNum>(this Interval<TNum> interval, Interval<TNum> other)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        return other.IsSubsetOf(interval);
    }

    /// <summary>
    /// 取两个区间的交集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <typeparam name="TNum"></typeparam>
    /// <returns></returns>
    public static Interval<TNum> Intersect<TNum>(this Interval<TNum> interval, Interval<TNum> other)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (interval.IsEmpty || other.IsEmpty)
            return new Interval<TNum>(TNum.Zero, TNum.Zero);

        var start = interval.Start > other.Start ? interval.Start : other.Start;
        var end = interval.End < other.End ? interval.End : other.End;

        return start > end ? new Interval<TNum>(TNum.Zero, TNum.Zero) : new Interval<TNum>(start, end);
    }

    #endregion
}
