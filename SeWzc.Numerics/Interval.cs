namespace SeWzc.Numerics;

/// <summary>
/// 区间。
/// </summary>
/// <remarks>
/// 表示一个 [Start, End] 的闭区间。
/// </remarks>
/// <param name="Start">区间的左端点。</param>
/// <param name="End">区间的右端点。</param>
public readonly record struct Interval(double Start, double End)
{
    #region 静态方法

    /// <summary>
    /// 通过区间端点创建一个区间。
    /// </summary>
    /// <param name="a">区间的一个端点。</param>
    /// <param name="b">区间的另一个端点。</param>
    /// <returns></returns>
    public static Interval Create(double a, double b)
    {
        return a > b ? new Interval(b, a) : new Interval(a, b);
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
    public double Length => Start >= End ? 0 : End - Start;

    #endregion

    #region 成员方法

    /// <summary>
    /// 判断一个值是否在区间内。
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(double value)
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
    public static bool IsProperSubsetOf(this Interval interval, Interval other)
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
    public static bool IsSubsetOf(this Interval interval, Interval other)
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
    public static bool IsProperSupersetOf(this Interval interval, Interval other)
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

    #endregion
}
