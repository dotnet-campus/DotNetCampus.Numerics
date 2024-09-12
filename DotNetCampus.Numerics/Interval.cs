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
    where TNum : unmanaged
#if NET8_0_OR_GREATER
    , IFloatingPoint<TNum>
#endif
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
#if NET8_0_OR_GREATER
        return a > b ? new Interval<TNum>(b, a) : new Interval<TNum>(a, b);
#else
        throw new NotSupportedException();
#endif
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
    private readonly bool _isNotEmpty
#if NET8_0_OR_GREATER
        = Start <= End;
#else
     ;
#endif

    #endregion

    #region 属性

    /// <summary>
    /// 是否是空集。
    /// </summary>
    public bool IsEmpty => !_isNotEmpty;

    /// <summary>
    /// 区间长度。
    /// </summary>
    public TNum Length
#if NET8_0_OR_GREATER
        => Start >= End ? TNum.Zero : End - Start;
#else
        => throw new NotSupportedException();
#endif

    #endregion

    #region 成员方法

    /// <summary>
    /// 判断一个值是否在区间内。
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(TNum value)
    {
#if NET8_0_OR_GREATER
        return Start <= value && value <= End;
#else
        throw new NotSupportedException();
#endif
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
#if NET8_0_OR_GREATER
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
#else
        where TNum : unmanaged
        => throw new NotSupportedException();
#endif

    /// <summary>
    /// 是否是一个区间的子集。
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsSubsetOf<TNum>(this Interval<TNum> interval, Interval<TNum> other)
#if NET8_0_OR_GREATER
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (interval.IsEmpty)
            return true;

        if (other.IsEmpty)
            return false;

        return interval.Start >= other.Start && interval.End <= other.End;
    }
#else
        where TNum : unmanaged
        => throw new NotSupportedException();
#endif

#if NET8_0_OR_GREATER
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
#endif

    #endregion
}