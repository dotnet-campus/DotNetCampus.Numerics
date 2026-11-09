using System;
using System.Runtime.CompilerServices;

namespace DotNetCampus.Numerics.Tests;

/// <summary>
/// 测试中对 <c>float</c> / <c>double</c> 做元素运算。
/// </summary>
internal static class TestNum
{
    #region 静态方法

    public static TNum One<TNum>()
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(1f);

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(1d);

        throw new NotSupportedException();
    }

    public static TNum Add<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
        => Binary(a, b, static (x, y) => x + y, static (x, y) => x + y);

    public static TNum Subtract<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
        => Binary(a, b, static (x, y) => x - y, static (x, y) => x - y);

    public static TNum Multiply<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
        => Binary(a, b, static (x, y) => x * y, static (x, y) => x * y);

    public static TNum Divide<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
        => Binary(a, b, static (x, y) => x / y, static (x, y) => x / y);

    public static TNum Negate<TNum>(this TNum a)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(-BitCast<TNum, float>(a));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(-BitCast<TNum, double>(a));

        throw new NotSupportedException();
    }

    public static bool IsNaN<TNum>(this TNum a)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return float.IsNaN(BitCast<TNum, float>(a));

        if (typeof(TNum) == typeof(double))
            return double.IsNaN(BitCast<TNum, double>(a));

        throw new NotSupportedException();
    }

    public static bool IsEqual<TNum>(this TNum a, TNum b)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<TNum, float>(a) == BitCast<TNum, float>(b);

        if (typeof(TNum) == typeof(double))
            return BitCast<TNum, double>(a) == BitCast<TNum, double>(b);

        throw new NotSupportedException();
    }

    public static TTo BitCast<TFrom, TTo>(TFrom value)
        where TFrom : unmanaged
        where TTo : unmanaged
        => Unsafe.As<TFrom, TTo>(ref value);

    private static TNum Binary<TNum>(TNum a, TNum b, Func<float, float, float> floatOp, Func<double, double, double> doubleOp)
        where TNum : unmanaged
    {
        if (typeof(TNum) == typeof(float))
            return BitCast<float, TNum>(floatOp(BitCast<TNum, float>(a), BitCast<TNum, float>(b)));

        if (typeof(TNum) == typeof(double))
            return BitCast<double, TNum>(doubleOp(BitCast<TNum, double>(a), BitCast<TNum, double>(b)));

        throw new NotSupportedException();
    }

    #endregion
}
