using System.Runtime.CompilerServices;

namespace SeWzc.Numerics;

/// <summary>
/// 数值比较的帮助类。
/// </summary>
/// <remarks>
/// 在对比是否相等时，最好能够进行归一化处理，然后再进行比较。
/// </remarks>
public static class NumericsEqualHelper
{
    #region 静态变量

    /// <summary>
    /// 接近相等的容差。
    /// </summary>
    public const double NearlyTolerance = 1e-6;

    /// <summary>
    /// 接近相等的容差。
    /// </summary>
    public const float NearlyToleranceF = 1e-6f;

    /// <summary>
    /// 几乎相等的容差。
    /// </summary>
    /// <remarks>
    /// 适合用在单纯因为浮点数精度问题导致的不相等的情况。
    /// </remarks>
    public const double AlmostTolerance = 1e-10;

    #endregion

    #region 静态方法

    /// <summary>
    /// 判断浮点数是否在指定范围内。
    /// </summary>
    /// <param name="value">要判断的浮点数。</param>
    /// <param name="min">范围的最小值。</param>
    /// <param name="max">范围的最大值。</param>
    /// <param name="tolerance">容差。</param>
    /// <returns>
    /// 如果 <paramref name="value" /> 在 <paramref name="min" /> 和 <paramref name="max" /> 之间（包括边界），则返回
    /// <see langword="true" />；否则返回 <see langword="false" />。
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInRange(this double value, double min, double max, double tolerance = AlmostTolerance)
    {
        return value >= min - tolerance && value <= max + tolerance;
    }

    /// <summary>
    /// 判断浮点数是否在 0 到 1 之间。
    /// </summary>
    /// <param name="value">要判断的浮点数。</param>
    /// <param name="tolerance">容差。</param>
    /// <returns>
    /// 如果 <paramref name="value" /> 在 0 到 1 之间（包括边界），则返回
    /// <see langword="true" />；否则返回 <see langword="false" />。
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInZeroToOne(this double value, double tolerance = AlmostTolerance)
    {
        return value.IsInRange(0, 1, tolerance);
    }

    #endregion

    #region 相等判断

    /// <summary>
    /// 判断两个浮点数是否近似相等，并且要求进行归一化处理。
    /// </summary>
    /// <param name="a">要比较的第一个浮点数。</param>
    /// <param name="b">要比较的第二个浮点数。</param>
    /// <param name="normalizationFactor">归一化因子。</param>
    /// <param name="tolerance">容差。</param>
    /// <returns>如果两个浮点数近似相等，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCloseEqual(double a, double b, double normalizationFactor, double tolerance)
    {
        return (a - b).IsCloseZero(tolerance, normalizationFactor);
    }

    /// <summary>
    /// 判断两个浮点数是否近似相等。将两个浮点数中的绝对值较大值作为归一化因子。
    /// </summary>
    /// <param name="a">要比较的第一个浮点数。</param>
    /// <param name="b">要比较的第二个浮点数。</param>
    /// <returns>如果两个浮点数近似相等，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyEqual(this double a, double b)
    {
        return a.Equals(b) || ((a - b) / Math.MaxMagnitude(a, b)).IsNearlyZero();
    }

    /// <summary>
    /// 判断两个浮点数是否近似相等。将两个浮点数中的绝对值较大值作为归一化因子。
    /// </summary>
    /// <param name="a">要比较的第一个浮点数。</param>
    /// <param name="b">要比较的第二个浮点数。</param>
    /// <returns>如果两个浮点数近似相等，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyEqual(this float a, float b)
    {
        return a.Equals(b) || ((a - b) / MathF.MaxMagnitude(a, b)).IsNearlyZero();
    }

    /// <summary>
    /// 判断两个浮点数是否几乎相等。将两个浮点数中的绝对值较大值作为归一化因子。
    /// </summary>
    /// <param name="a">要比较的第一个浮点数。</param>
    /// <param name="b">要比较的第二个浮点数。</param>
    /// <returns>如果两个浮点数几乎相等，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostEqual(this double a, double b)
    {
        return a.Equals(b) || ((a - b) / Math.MaxMagnitude(a, b)).IsAlmostZero();
    }

    /// <summary>
    /// 判断两个角是否近似相等。
    /// </summary>
    /// <param name="a">要比较的第一个角。</param>
    /// <param name="b">要比较的第二个角。</param>
    /// <returns>如果两个角近似相等，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyEqual(this AngularMeasure a, AngularMeasure b)
    {
        return (a - b).Radian.IsNearlyZero(Math.Tau);
    }

    /// <summary>
    /// 判断两个角是否几乎相等。
    /// </summary>
    /// <param name="a">要比较的第一个角。</param>
    /// <param name="b">要比较的第二个角。</param>
    /// <returns>如果两个角几乎相等，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostEqual(this AngularMeasure a, AngularMeasure b)
    {
        return (a - b).Radian.IsAlmostZero(Math.Tau);
    }

    /// <summary>
    /// 判断两个向量是否近似相等。
    /// </summary>
    /// <param name="a">要比较的第一个向量。</param>
    /// <param name="b">要比较的第二个向量。</param>
    /// <returns>如果两个向量近似相等，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyEqual(this Vector2D a, Vector2D b)
    {
        return (a - b).LengthSquared <= NearlyTolerance * NearlyTolerance * (a.LengthSquared + b.LengthSquared);
    }

    /// <summary>
    /// 判断两个向量是否几乎相等。
    /// </summary>
    /// <param name="a">要比较的第一个向量。</param>
    /// <param name="b">要比较的第二个向量。</param>
    /// <returns>如果两个向量几乎相等，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostEqual(this Vector2D a, Vector2D b)
    {
        return (a - b).LengthSquared <= AlmostTolerance * AlmostTolerance * (a.LengthSquared + b.LengthSquared);
    }

    /// <summary>
    /// 判断两个向量是否近似相等。
    /// </summary>
    /// <param name="a">要比较的第一个向量。</param>
    /// <param name="b">要比较的第二个向量。</param>
    /// <returns>如果两个向量近似相等，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyEqual(this Vector3D a, Vector3D b)
    {
        return (a - b).LengthSquared <= NearlyTolerance * NearlyTolerance * (a.LengthSquared + b.LengthSquared);
    }

    /// <summary>
    /// 判断两个向量是否几乎相等。
    /// </summary>
    /// <param name="a">要比较的第一个向量。</param>
    /// <param name="b">要比较的第二个向量。</param>
    /// <returns>如果两个向量几乎相等，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostEqual(this Vector3D a, Vector3D b)
    {
        return (a - b).LengthSquared <= AlmostTolerance * AlmostTolerance * (a.LengthSquared + b.LengthSquared);
    }

    #endregion

    #region 是否接近 0

    /// <summary>
    /// 判断浮点数是否近似为 0，并且要求进行归一化处理。
    /// </summary>
    /// <param name="a">要判断的浮点数。</param>
    /// <param name="tolerance">容差。</param>
    /// <param name="normalizationFactor">归一化因子。</param>
    /// <returns>如果浮点数近似为 0，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCloseZero(this double a, double tolerance, double normalizationFactor)
    {
        return Math.Abs(a) <= tolerance * normalizationFactor;
    }

    /// <summary>
    /// 判断浮点数是否近似为 0，并且要求进行归一化处理。
    /// </summary>
    /// <param name="a">要判断的浮点数。</param>
    /// <param name="tolerance">容差。</param>
    /// <param name="normalizationFactor">归一化因子。</param>
    /// <returns>如果浮点数近似为 0，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCloseZero(this float a, float tolerance, float normalizationFactor)
    {
        return Math.Abs(a) <= tolerance * normalizationFactor;
    }

    /// <summary>
    /// 判断浮点数是否近似为 0。
    /// </summary>
    /// <param name="a">要判断的浮点数。</param>
    /// <param name="tolerance">容差。</param>
    /// <returns>如果浮点数近似为 0，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCloseZero(this double a, double tolerance)
    {
        return Math.Abs(a) <= tolerance;
    }

    /// <summary>
    /// 判断浮点数是否近似为 0。
    /// </summary>
    /// <param name="a">要判断的浮点数。</param>
    /// <param name="tolerance">容差。</param>
    /// <returns>如果浮点数近似为 0，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCloseZero(this float a, float tolerance)
    {
        return Math.Abs(a) <= tolerance;
    }

    /// <summary>
    /// 判断浮点数是否近似为 0，并且要求进行归一化处理。
    /// </summary>
    /// <param name="a">要判断的浮点数。</param>
    /// <param name="normalizationFactor">归一化因子。</param>
    /// <returns>如果浮点数近似为 0，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyZero(this double a, double normalizationFactor)
    {
        return a.IsCloseZero(NearlyTolerance, normalizationFactor);
    }

    /// <summary>
    /// 判断浮点数是否几乎为 0，并且要求进行归一化处理。
    /// </summary>
    /// <param name="a">要判断的浮点数。</param>
    /// <param name="normalizationFactor">归一化因子。</param>
    /// <returns>如果浮点数几乎为 0，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyZero(this float a, float normalizationFactor)
    {
        return a.IsCloseZero(NearlyToleranceF, normalizationFactor);
    }

    /// <summary>
    /// 判断浮点数是否几乎为 0，并且要求进行归一化处理。
    /// </summary>
    /// <param name="a">要判断的浮点数。</param>
    /// <param name="normalizationFactor">归一化因子。</param>
    /// <returns>如果浮点数几乎为 0，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostZero(this double a, double normalizationFactor)
    {
        return a.IsCloseZero(AlmostTolerance, normalizationFactor);
    }

    /// <summary>
    /// 判断浮点数是否近似为 0。
    /// </summary>
    /// <param name="a">要判断的浮点数。</param>
    /// <returns>如果浮点数近似为 0，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyZero(this double a)
    {
        return Math.Abs(a) <= NearlyTolerance;
    }

    /// <summary>
    /// 判断浮点数是否近似为 0。
    /// </summary>
    /// <param name="a">要判断的浮点数。</param>
    /// <returns>如果浮点数近似为 0，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    public static bool IsNearlyZero(this float a)
    {
        return Math.Abs(a) <= NearlyToleranceF;
    }

    /// <summary>
    /// 判断浮点数是否几乎为 0。
    /// </summary>
    /// <param name="a">要判断的浮点数。</param>
    /// <returns>如果浮点数几乎为 0，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostZero(this double a)
    {
        return Math.Abs(a) <= AlmostTolerance;
    }

    #endregion
}
