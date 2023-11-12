namespace SeWzc.Numerics;

/// <summary>
/// 区间。
/// </summary>
/// <remarks>
/// 表示一个 [Start, End] 的区间。
/// </remarks>
/// <param name="Start"></param>
/// <param name="End"></param>
public readonly record struct Interval(double Start, double End)
{
    /// <summary>
    /// 区间长度。
    /// </summary>
    public double Length => End - Start;

    /// <summary>
    /// 判断一个值是否在区间内。
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(double value)
    {
        return Start <= value && value <= End;
    }

    public static Interval Create(double a, double b)
    {
        return a > b ? new Interval(b, a) : new Interval(a, b);
    }
}

public static class IntervalExtensions
{
    /// <summary>
    /// 是否是空集。
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    public static bool IsEmpty(this Interval interval)
    {
        return interval.End <= interval.Start;
    }

    /// <summary>
    /// 是否是空集。
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    public static bool IsEmpty(this Interval? interval)
    {
        return interval is not { } intervalNotNull || intervalNotNull.IsEmpty();
    }

    /// <summary>
    /// 是否是一个区间的真子集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsProperSubsetOf(this Interval interval, Interval other)
    {
        if (other.IsEmpty())
            return false;

        if (interval.IsEmpty())
            return true;

        return interval.Start > other.Start && interval.End < other.End;
    }

    /// <summary>
    /// 是否是一个区间的真子集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsProperSubsetOf(this Interval? interval, Interval? other)
    {
        if (other.IsEmpty())
            return false;

        if (interval.IsEmpty())
            return true;

        return interval.GetValueOrDefault().Start > other.GetValueOrDefault().Start && interval.GetValueOrDefault().End < other.GetValueOrDefault().End;
    }

    /// <summary>
    /// 是否是一个区间的子集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsSubsetOf(this Interval interval, Interval other)
    {
        if (interval.IsEmpty())
            return true;

        if (other.IsEmpty())
            return false;

        return interval.Start >= other.Start && interval.End <= other.End;
    }

    /// <summary>
    /// 是否是一个区间的子集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsSubsetOf(this Interval? interval, Interval? other)
    {
        if (interval.IsEmpty())
            return true;

        if (other.IsEmpty())
            return false;

        return interval.GetValueOrDefault().Start >= other.GetValueOrDefault().Start && interval.GetValueOrDefault().End <= other.GetValueOrDefault().End;
    }

    /// <summary>
    /// 是否是一个区间的真超集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsProperSupersetOf(this Interval interval, Interval other)
    {
        return other.IsProperSubsetOf(interval);
    }

    /// <summary>
    /// 是否是一个区间的真超集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsProperSupersetOf(this Interval? interval, Interval? other)
    {
        return other.IsProperSubsetOf(interval);
    }

    /// <summary>
    /// 是否是一个区间的超集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsSupersetOf(this Interval interval, Interval other)
    {
        return other.IsSubsetOf(interval);
    }

    /// <summary>
    /// 是否是一个区间的超集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsSupersetOf(this Interval? interval, Interval? other)
    {
        return other.IsSubsetOf(interval);
    }

    /// <summary>
    /// 获取与一个区间的交集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static Interval? Intersect(this Interval interval, Interval other)
    {
        var start = Math.Max(other.Start, interval.Start);
        var end = Math.Min(other.End, interval.End);
        return start >= end ? null : new Interval(start, end);
    }

    /// <summary>
    /// 获取与一个区间的交集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static Interval? Intersect(this Interval? interval, Interval? other)
    {
        if (interval.IsEmpty())
            return null;

        if (other.IsEmpty())
            return null;

        var start = Math.Max(other.GetValueOrDefault().Start, interval.GetValueOrDefault().Start);
        var end = Math.Min(other.GetValueOrDefault().End, interval.GetValueOrDefault().End);
        return start >= end ? null : new Interval(start, end);
    }
}