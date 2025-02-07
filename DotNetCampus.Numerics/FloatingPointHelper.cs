using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotNetCampus.Numerics;

/// <summary>
/// 浮点数帮助类。对浮点数泛型补充一些方法。
/// </summary>
internal static class FloatingPointHelper
{
    #region 静态方法

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Multiply<TNum>(this TNum a, int b)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (typeof(TNum) == typeof(float))
            return Unsafe.BitCast<float, TNum>(Unsafe.BitCast<TNum, float>(a) * b);

        if (typeof(TNum) == typeof(double))
            return Unsafe.BitCast<double, TNum>(Unsafe.BitCast<TNum, double>(a) * b);

        if (typeof(TNum) == typeof(NFloat))
            return Unsafe.BitCast<NFloat, TNum>(Unsafe.BitCast<TNum, NFloat>(a) * b);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Multiply<TNum>(this int a, TNum b)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        return b.Multiply(a);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Divide<TNum>(this TNum a, int b)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (typeof(TNum) == typeof(float))
            return Unsafe.BitCast<float, TNum>(Unsafe.BitCast<TNum, float>(a) / b);

        if (typeof(TNum) == typeof(double))
            return Unsafe.BitCast<double, TNum>(Unsafe.BitCast<TNum, double>(a) / b);

        if (typeof(TNum) == typeof(NFloat))
            return Unsafe.BitCast<NFloat, TNum>(Unsafe.BitCast<TNum, NFloat>(a) / b);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Sqrt<TNum>(this TNum a)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (typeof(TNum) == typeof(float))
            return Unsafe.BitCast<float, TNum>(MathF.Sqrt(Unsafe.BitCast<TNum, float>(a)));

        if (typeof(TNum) == typeof(double))
            return Unsafe.BitCast<double, TNum>(Math.Sqrt(Unsafe.BitCast<TNum, double>(a)));

        if (typeof(TNum) == typeof(NFloat))
            return Unsafe.BitCast<NFloat, TNum>(NFloat.Sqrt(Unsafe.BitCast<TNum, NFloat>(a)));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Clamp<TNum>(this TNum value, TNum min, TNum max)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (typeof(TNum) == typeof(float))
            return Unsafe.BitCast<float, TNum>(Math.Clamp(Unsafe.BitCast<TNum, float>(value), Unsafe.BitCast<TNum, float>(min), Unsafe.BitCast<TNum, float>(max)));

        if (typeof(TNum) == typeof(double))
            return Unsafe.BitCast<double, TNum>(Math.Clamp(Unsafe.BitCast<TNum, double>(value), Unsafe.BitCast<TNum, double>(min), Unsafe.BitCast<TNum, double>(max)));

        if (typeof(TNum) == typeof(NFloat))
            return Unsafe.BitCast<NFloat, TNum>(NFloat.Clamp(Unsafe.BitCast<TNum, NFloat>(value), Unsafe.BitCast<TNum, NFloat>(min), Unsafe.BitCast<TNum, NFloat>(max)));

        return value <= min
            ? min
            : value >= max
                ? max
                : value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Abs<TNum>(this TNum a)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (typeof(TNum) == typeof(float))
            return Unsafe.BitCast<float, TNum>(MathF.Abs(Unsafe.BitCast<TNum, float>(a)));

        if (typeof(TNum) == typeof(double))
            return Unsafe.BitCast<double, TNum>(Math.Abs(Unsafe.BitCast<TNum, double>(a)));

        if (typeof(TNum) == typeof(NFloat))
            return Unsafe.BitCast<NFloat, TNum>(NFloat.Abs(Unsafe.BitCast<TNum, NFloat>(a)));

        return a > TNum.Zero ? a : -a;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostEqual<TNum>(this TNum a, TNum b, TNum normalizationFactor)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (typeof(TNum) == typeof(float))
            return NumericsEqualHelper.IsCloseEqual(Unsafe.BitCast<TNum, float>(a), Unsafe.BitCast<TNum, float>(b),
                Unsafe.BitCast<TNum, float>(normalizationFactor), NumericsEqualHelper.NearlyToleranceF);

        if (typeof(TNum) == typeof(double))
            return NumericsEqualHelper.IsCloseEqual(Unsafe.BitCast<TNum, double>(a), Unsafe.BitCast<TNum, double>(b),
                Unsafe.BitCast<TNum, double>(normalizationFactor), NumericsEqualHelper.AlmostTolerance);

        if (typeof(TNum) == typeof(NFloat))
            return NumericsEqualHelper.IsCloseEqual(Unsafe.BitCast<TNum, NFloat>(a), Unsafe.BitCast<TNum, NFloat>(b),
                Unsafe.BitCast<TNum, NFloat>(normalizationFactor), NumericsEqualHelper.NearlyTolerance);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostZero<TNum>(this TNum a, TNum normalizationFactor)
        where TNum : unmanaged, IFloatingPoint<TNum>
    {
        if (typeof(TNum) == typeof(float))
            return Unsafe.BitCast<TNum, float>(a).IsCloseZero(NumericsEqualHelper.NearlyToleranceF, Unsafe.BitCast<TNum, float>(normalizationFactor));

        if (typeof(TNum) == typeof(double))
            return Unsafe.BitCast<TNum, double>(a).IsCloseZero(NumericsEqualHelper.AlmostTolerance, Unsafe.BitCast<TNum, double>(normalizationFactor));

        if (typeof(TNum) == typeof(NFloat))
            return NumericsEqualHelper.IsCloseZero(Unsafe.BitCast<TNum, NFloat>(a), NumericsEqualHelper.NearlyTolerance, Unsafe.BitCast<TNum, NFloat>(normalizationFactor));

        throw new NotSupportedException();
    }

    #endregion
}
