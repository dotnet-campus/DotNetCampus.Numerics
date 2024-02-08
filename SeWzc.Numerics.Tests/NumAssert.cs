using System;
using System.Globalization;
using System.Numerics;
using Xunit;

namespace SeWzc.Numerics.Tests;

internal static class NumAssert
{
    #region 静态方法

    public static void CloseEqual<TNum>(TNum expected, TNum actual)
        where TNum : unmanaged, INumber<TNum>
    {
        if (typeof(TNum) == typeof(double))
            Assert.Equal(Convert.ToDouble(expected, CultureInfo.InvariantCulture), Convert.ToDouble(actual, CultureInfo.InvariantCulture),
                (a, b) => a.IsAlmostEqual(b));
        if (typeof(TNum) == typeof(float))
            Assert.Equal(Convert.ToSingle(expected, CultureInfo.InvariantCulture), Convert.ToSingle(actual, CultureInfo.InvariantCulture),
                (a, b) => a.IsNearlyEqual(b));
        throw new NotSupportedException($"暂时不支持类型 {typeof(TNum).FullName}。");
    }

    public static void Equal<TNum>(TNum expected, TNum actual)
        where TNum : unmanaged, INumber<TNum>
    {
        Assert.Equal(expected, actual);
    }

    #endregion
}