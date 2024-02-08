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
        {
            Assert.Equal(Convert.ToDouble(expected, CultureInfo.InvariantCulture), Convert.ToDouble(actual, CultureInfo.InvariantCulture),
                (a, b) => a.IsAlmostEqual(b));
        }
        else if (typeof(TNum) == typeof(float))
        {
            Assert.Equal(Convert.ToSingle(expected, CultureInfo.InvariantCulture), Convert.ToSingle(actual, CultureInfo.InvariantCulture),
                (a, b) => a.IsNearlyEqual(b));
        }
        else
            throw new NotSupportedException($"暂时不支持类型 {typeof(TNum).FullName}。");
    }

    public static void CloseZero<TNum>(TNum actual)
    {
        if (typeof(TNum) == typeof(double))
        {
            if (!Convert.ToDouble(actual, CultureInfo.InvariantCulture).IsAlmostZero())
                Assert.Fail($"The double value is not almost zero.\r\nExpected: Is Almost Zero.\r\nActual: {actual}");
        }
        else if (typeof(TNum) == typeof(float))
        {
            if (!Convert.ToSingle(actual, CultureInfo.InvariantCulture).IsNearlyZero())
                Assert.Fail($"The float value is not nearly zero.\r\nExpected: Is Nearly Zero.\r\nActual: {actual}");
        }
        else
            throw new NotSupportedException($"暂时不支持类型 {typeof(TNum).FullName}。");
    }

    public static void NotCloseZero<TNum>(TNum actual)
    {
        if (typeof(TNum) == typeof(double))
        {
            if (Convert.ToDouble(actual, CultureInfo.InvariantCulture).IsAlmostZero())
                Assert.Fail($"The double value is almost zero.\r\nExpected: Not Almost Zero.\r\nActual: {actual}");
        }
        else if (typeof(TNum) == typeof(float))
        {
            if (Convert.ToSingle(actual, CultureInfo.InvariantCulture).IsNearlyZero())
                Assert.Fail($"The float value is nearly zero.\r\nExpected: Not Nearly Zero.\r\nActual: {actual}");
        }
        else
            throw new NotSupportedException($"暂时不支持类型 {typeof(TNum).FullName}。");
    }

    public static void Equal<TNum>(TNum expected, TNum actual)
        where TNum : unmanaged, INumber<TNum>
    {
        Assert.Equal(expected, actual);
    }

    #endregion
}