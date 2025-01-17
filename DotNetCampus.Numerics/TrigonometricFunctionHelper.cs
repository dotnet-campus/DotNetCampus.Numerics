using System.Numerics;
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
        where TNum : unmanaged, IFloatingPoint<TNum>, ITrigonometricFunctions<TNum>
    {
        var length = interval.Length;
        if (length >= TNum.Tau)
        {
            return TNum.One;
        }

        var start = interval.Start > TNum.Zero
            ? interval.Start % TNum.Tau
            : TNum.Tau + interval.Start % TNum.Tau;
        var end = start + length;

        return end >= TNum.Tau
            ? TNum.One
            : TNum.Cos(start >= TNum.Tau - end ? end : start);
    }

    /// <summary>
    /// 获取余弦函数在区间内的最小值。
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum GetMinValueForCos<TNum>(Interval<TNum> interval)
        where TNum : unmanaged, IFloatingPoint<TNum>, ITrigonometricFunctions<TNum>
    {
        return -GetMaxValueForCos(interval + TNum.Pi);
    }

    /// <summary>
    /// 获取正弦函数在区间内的最大值。
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum GetMaxValueForSin<TNum>(Interval<TNum> interval)
        where TNum : unmanaged, IFloatingPoint<TNum>, ITrigonometricFunctions<TNum>
    {
        return GetMaxValueForCos(interval - TNum.Pi / (TNum.One + TNum.One));
    }

    /// <summary>
    /// 获取正弦函数在区间内的最小值。
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum GetMinValueForSin<TNum>(Interval<TNum> interval)
        where TNum : unmanaged, IFloatingPoint<TNum>, ITrigonometricFunctions<TNum>
    {
        return -GetMaxValueForCos(interval + TNum.Pi / (TNum.One + TNum.One));
    }

    #endregion
}
