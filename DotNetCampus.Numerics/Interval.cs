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
    where TNum : unmanaged
{
    #region 静态变量

    /// <summary>
    /// 空区间。
    /// </summary>
    public static Interval<TNum> Empty => new(default, default);

    #endregion

    #region 静态方法

    /// <summary>
    /// 通过区间端点创建一个区间。
    /// </summary>
    /// <param name="a">区间的一个端点。</param>
    /// <param name="b">区间的另一个端点。</param>
    /// <returns></returns>
    public static Interval<TNum> Create(TNum a, TNum b)
    {
        return a.GreaterThan(b) ? new Interval<TNum>(b, a) : new Interval<TNum>(a, b);
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
    private readonly bool _isNotEmpty = Start.LessThanOrEqual(End);

    #endregion

    #region 属性

    /// <summary>
    /// 是否是空集。
    /// </summary>
    public bool IsEmpty => !_isNotEmpty;

    /// <summary>
    /// 区间长度。
    /// </summary>
    public TNum Length => Start.GreaterThanOrEqual(End) ? default : End.Subtract(Start);

    #endregion

    #region 成员方法

    /// <summary>
    /// 判断一个值是否在区间内。
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(TNum value)
    {
        return Start.LessThanOrEqual(value) && value.LessThanOrEqual(End);
    }

    /// <summary>
    /// 将目标值限制在区间内。
    /// </summary>
    /// <param name="value">需要限制的值。</param>
    /// <returns>限制后的值。</returns>
    public TNum Clamp(TNum value)
    {
        return value.Clamp(Start, End);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return _isNotEmpty ? $"[{Start}, {End}]" : "Empty";
    }

    #endregion

    #region 运算符重载

    /// <summary>
    /// 将区间进行缩放。
    /// </summary>
    public static Interval<TNum> operator *(Interval<TNum> interval, TNum value)
    {
        return interval.IsEmpty ? new Interval<TNum>(default, default) : Create(interval.Start.Multiply(value), interval.End.Multiply(value));
    }

    /// <summary>
    /// 将区间进行缩放。
    /// </summary>
    public static Interval<TNum> operator *(TNum value, Interval<TNum> interval)
    {
        return interval * value;
    }

    /// <summary>
    /// 将区间进行偏移。
    /// </summary>
    public static Interval<TNum> operator +(Interval<TNum> interval, TNum value)
    {
        return new Interval<TNum>(interval.Start.Add(value), interval.End.Add(value));
    }

    /// <summary>
    /// 将区间进行偏移。
    /// </summary>
    public static Interval<TNum> operator +(TNum value, Interval<TNum> interval)
    {
        return interval + value;
    }

    /// <summary>
    /// 将区间进行偏移。
    /// </summary>
    public static Interval<TNum> operator -(Interval<TNum> interval, TNum value)
    {
        return new Interval<TNum>(interval.Start.Subtract(value), interval.End.Subtract(value));
    }

    /// <summary>
    /// 将区间进行偏移。
    /// </summary>
    public static Interval<TNum> operator -(TNum value, Interval<TNum> interval)
    {
        return new Interval<TNum>(value.Subtract(interval.End), value.Subtract(interval.Start));
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
        where TNum : unmanaged
    {
        if (other.IsEmpty)
        {
            return false;
        }

        if (interval.IsEmpty)
        {
            return true;
        }

        return interval.Start.GreaterThanOrEqual(other.Start)
               && interval.End.LessThanOrEqual(other.End)
               && (!interval.Start.Equals(other.Start) || !interval.End.Equals(other.End));
    }

    /// <summary>
    /// 是否是一个区间的子集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsSubsetOf<TNum>(this Interval<TNum> interval, Interval<TNum> other)
        where TNum : unmanaged
    {
        if (interval.IsEmpty)
        {
            return true;
        }

        if (other.IsEmpty)
        {
            return false;
        }

        return interval.Start.GreaterThanOrEqual(other.Start) && interval.End.LessThanOrEqual(other.End);
    }

    /// <summary>
    /// 是否是一个区间的真超集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsProperSupersetOf<TNum>(this Interval<TNum> interval, Interval<TNum> other)
        where TNum : unmanaged
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
        where TNum : unmanaged
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
        where TNum : unmanaged
    {
        if (interval.IsEmpty || other.IsEmpty)
        {
            return new Interval<TNum>(default, default);
        }

        var start = interval.Start.GreaterThan(other.Start) ? interval.Start : other.Start;
        var end = interval.End.LessThan(other.End) ? interval.End : other.End;

        return start.GreaterThan(end) ? new Interval<TNum>(default, default) : new Interval<TNum>(start, end);
    }

    #endregion
}
