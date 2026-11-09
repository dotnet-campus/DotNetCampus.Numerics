using System.Runtime.CompilerServices;

namespace DotNetCampus.Numerics;

internal static class TrigonometricFunctionHelper
{
    #region 静态方法

    /// <summary>
    /// 获取余弦函数在区间内的最大值。
    /// </summary>
    /// <returns></returns>
    public static TNum GetMaxValueForCos<TNum>(Interval<TNum> interval)
        where TNum : unmanaged
    {
        var length = interval.Length;
        var tau = FloatingPointHelper.Tau<TNum>();
        if (length.GreaterThanOrEqual(tau))
        {
            return FloatingPointHelper.One<TNum>();
        }

        var start = interval.Start.GreaterThan(default)
            ? interval.Start.Remainder(tau)
            : tau.Add(interval.Start.Remainder(tau));
        var end = start.Add(length);
        var one = FloatingPointHelper.One<TNum>();

        return end.GreaterThanOrEqual(tau)
            ? one
            : (start.GreaterThanOrEqual(tau.Subtract(end)) ? end : start).Cos();
    }

    /// <summary>
    /// 获取余弦函数在区间内的最小值。
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum GetMinValueForCos<TNum>(Interval<TNum> interval)
        where TNum : unmanaged
    {
        return GetMaxValueForCos(interval + FloatingPointHelper.Pi<TNum>()).Negate();
    }

    /// <summary>
    /// 获取正弦函数在区间内的最大值。
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum GetMaxValueForSin<TNum>(Interval<TNum> interval)
        where TNum : unmanaged
    {
        var halfPi = FloatingPointHelper.Pi<TNum>().Divide(FloatingPointHelper.One<TNum>().Add(FloatingPointHelper.One<TNum>()));
        return GetMaxValueForCos(interval - halfPi);
    }

    /// <summary>
    /// 获取正弦函数在区间内的最小值。
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum GetMinValueForSin<TNum>(Interval<TNum> interval)
        where TNum : unmanaged
    {
        var halfPi = FloatingPointHelper.Pi<TNum>().Divide(FloatingPointHelper.One<TNum>().Add(FloatingPointHelper.One<TNum>()));
        return GetMaxValueForCos(interval + halfPi).Negate();
    }

    #endregion
}
