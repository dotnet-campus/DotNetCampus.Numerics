using System.Runtime.CompilerServices;

namespace DotNetCampus.Numerics;

/// <summary>
/// 浮点数帮助类。对浮点数泛型补充一些方法。
/// </summary>
internal static class FloatingPointHelper
{
    #region 静态方法

    /// <summary>
    /// 按位重解释。两个类型的大小必须相同。
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TTo BitCast<TFrom, TTo>(TFrom value)
        where TFrom : unmanaged
        where TTo : unmanaged
    {
        if (Unsafe.SizeOf<TFrom>() != Unsafe.SizeOf<TTo>())
            throw new NotSupportedException();

        return Unsafe.As<TFrom, TTo>(ref value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum One<TNum>()
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(1f);

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(1d);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Pi<TNum>()
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(MathF.PI);

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(Math.PI);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Tau<TNum>()
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(MathF.Tau);

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(Math.Tau);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Add<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(BitCast<TNum, float>(a) + BitCast<TNum, float>(b));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(BitCast<TNum, double>(a) + BitCast<TNum, double>(b));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Subtract<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(BitCast<TNum, float>(a) - BitCast<TNum, float>(b));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(BitCast<TNum, double>(a) - BitCast<TNum, double>(b));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Multiply<TNum>(this TNum a, int b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(BitCast<TNum, float>(a) * b);

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(BitCast<TNum, double>(a) * b);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Multiply<TNum>(this int a, TNum b)
        where TNum : unmanaged
    {
        return b.Multiply(a);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Multiply<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(BitCast<TNum, float>(a) * BitCast<TNum, float>(b));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(BitCast<TNum, double>(a) * BitCast<TNum, double>(b));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Divide<TNum>(this TNum a, int b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(BitCast<TNum, float>(a) / b);

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(BitCast<TNum, double>(a) / b);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Divide<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(BitCast<TNum, float>(a) / BitCast<TNum, float>(b));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(BitCast<TNum, double>(a) / BitCast<TNum, double>(b));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Remainder<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(BitCast<TNum, float>(a) % BitCast<TNum, float>(b));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(BitCast<TNum, double>(a) % BitCast<TNum, double>(b));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Negate<TNum>(this TNum a)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(-BitCast<TNum, float>(a));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(-BitCast<TNum, double>(a));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEqual<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<TNum, float>(a) == BitCast<TNum, float>(b);

        if (typeof(TNum) == typeof(double))
            return BitCast<TNum, double>(a) == BitCast<TNum, double>(b);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThan<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<TNum, float>(a) > BitCast<TNum, float>(b);

        if (typeof(TNum) == typeof(double))
            return BitCast<TNum, double>(a) > BitCast<TNum, double>(b);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThan<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<TNum, float>(a) < BitCast<TNum, float>(b);

        if (typeof(TNum) == typeof(double))
            return BitCast<TNum, double>(a) < BitCast<TNum, double>(b);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanOrEqual<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<TNum, float>(a) >= BitCast<TNum, float>(b);

        if (typeof(TNum) == typeof(double))
            return BitCast<TNum, double>(a) >= BitCast<TNum, double>(b);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanOrEqual<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<TNum, float>(a) <= BitCast<TNum, float>(b);

        if (typeof(TNum) == typeof(double))
            return BitCast<TNum, double>(a) <= BitCast<TNum, double>(b);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Sin<TNum>(this TNum a)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(MathF.Sin(BitCast<TNum, float>(a)));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(Math.Sin(BitCast<TNum, double>(a)));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Cos<TNum>(this TNum a)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(MathF.Cos(BitCast<TNum, float>(a)));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(Math.Cos(BitCast<TNum, double>(a)));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Min<TNum>(TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(MathF.Min(BitCast<TNum, float>(a), BitCast<TNum, float>(b)));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(Math.Min(BitCast<TNum, double>(a), BitCast<TNum, double>(b)));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Max<TNum>(TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(MathF.Max(BitCast<TNum, float>(a), BitCast<TNum, float>(b)));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(Math.Max(BitCast<TNum, double>(a), BitCast<TNum, double>(b)));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Sqrt<TNum>(this TNum a)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(MathF.Sqrt(BitCast<TNum, float>(a)));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(Math.Sqrt(BitCast<TNum, double>(a)));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Clamp<TNum>(this TNum value, TNum min, TNum max)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(Math.Clamp(BitCast<TNum, float>(value), BitCast<TNum, float>(min), BitCast<TNum, float>(max)));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(Math.Clamp(BitCast<TNum, double>(value), BitCast<TNum, double>(min), BitCast<TNum, double>(max)));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum Abs<TNum>(this TNum a)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(MathF.Abs(BitCast<TNum, float>(a)));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(Math.Abs(BitCast<TNum, double>(a)));

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostEqual<TNum>(this TNum a, TNum b, TNum normalizationFactor)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return NumericsEqualHelper.IsCloseEqual(BitCast<TNum, float>(a), BitCast<TNum, float>(b),
                BitCast<TNum, float>(normalizationFactor), NumericsEqualHelper.NearlyToleranceF);

        if (typeof(TNum) == typeof(double))
            return NumericsEqualHelper.IsCloseEqual(BitCast<TNum, double>(a), BitCast<TNum, double>(b),
                BitCast<TNum, double>(normalizationFactor), NumericsEqualHelper.AlmostTolerance);

        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostZero<TNum>(this TNum a, TNum normalizationFactor)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<TNum, float>(a).IsCloseZero(NumericsEqualHelper.NearlyToleranceF, BitCast<TNum, float>(normalizationFactor));

        if (typeof(TNum) == typeof(double))
            return BitCast<TNum, double>(a).IsCloseZero(NumericsEqualHelper.AlmostTolerance, BitCast<TNum, double>(normalizationFactor));

        throw new NotSupportedException();
    }

    #endregion
}
