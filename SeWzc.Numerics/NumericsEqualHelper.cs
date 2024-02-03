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
    /// <remarks>
    /// 适合用在
    /// </remarks>
    public const double NearlyTolerance = 1e-6;

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
    /// 判断两个浮点数是否近似相等，并且要求进行归一化处理。
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="normalizationFactor">归一化因子。</param>
    /// <param name="tolerance">容差。</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCloseEqual(double a, double b, double normalizationFactor, double tolerance)
    {
        return (a - b).IsCloseZero(tolerance, normalizationFactor);
    }

    /// <summary>
    /// 判断两个浮点数是否近似相等。
    /// </summary>
    public static bool IsNearlyEqual(this double a, double b)
    {
        return (a - b).IsNearlyZero();
    }

    /// <summary>
    /// 判断两个浮点数是否几乎相等。
    /// </summary>
    public static bool IsAlmostEqual(this double a, double b)
    {
        return (a - b).IsAlmostZero();
    }

    /// <summary>
    /// 判断两个角是否近似相等。
    /// </summary>
    public static bool IsNearlyEqual(this AngularMeasure a, AngularMeasure b)
    {
        return (a - b).Radian.IsNearlyZero(Math.Tau);
    }

    /// <summary>
    /// 判断两个角是否几乎相等。
    /// </summary>
    public static bool IsAlmostEqual(this AngularMeasure a, AngularMeasure b)
    {
        return (a - b).Radian.IsAlmostZero(Math.Tau);
    }

    /// <summary>
    /// 判断两个向量是否近似相等。
    /// </summary>
    public static bool IsNearlyEqual(this Vector2D a, Vector2D b)
    {
        return (a - b).SquaredLength < NearlyTolerance * NearlyTolerance;
    }

    /// <summary>
    /// 判断两个向量是否几乎相等。
    /// </summary>
    public static bool IsAlmostEqual(this Vector2D a, Vector2D b)
    {
        return (a - b).SquaredLength < AlmostTolerance * AlmostTolerance;
    }

    /// <summary>
    /// 判断两个向量是否近似相等。
    /// </summary>
    public static bool IsNearlyEqual(this Vector3D a, Vector3D b)
    {
        return (a - b).SquaredLength < NearlyTolerance * NearlyTolerance;
    }

    /// <summary>
    /// 判断两个向量是否几乎相等。
    /// </summary>
    public static bool IsAlmostEqual(this Vector3D a, Vector3D b)
    {
        return (a - b).SquaredLength < AlmostTolerance * AlmostTolerance;
    }

    #endregion

    #region 是否接近 0

    /// <summary>
    /// 判断浮点数是否近似为 0，并且要求进行归一化处理。
    /// </summary>
    /// <param name="a"></param>
    /// <param name="tolerance">容差</param>
    /// <param name="normalizationFactor">归一化因子。</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCloseZero(this double a, double tolerance, double normalizationFactor)
    {
        return Math.Abs(a) < tolerance * normalizationFactor;
    }

    /// <summary>
    /// 判断浮点数是否近似为 0。
    /// </summary>
    /// <param name="a"></param>
    /// <param name="tolerance">容差。</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCloseZero(this double a, double tolerance)
    {
        return Math.Abs(a) < tolerance;
    }

    /// <summary>
    /// 判断浮点数是否近似为 0，并且要求进行归一化处理。
    /// </summary>
    /// <param name="a"></param>
    /// <param name="normalizationFactor">归一化因子。</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyZero(this double a, double normalizationFactor)
    {
        return a.IsCloseZero(NearlyTolerance, normalizationFactor);
    }

    /// <summary>
    /// 判断浮点数是否几乎为 0，并且要求进行归一化处理。
    /// </summary>
    /// <param name="a"></param>
    /// <param name="normalizationFactor">归一化因子。</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostZero(this double a, double normalizationFactor)
    {
        return a.IsCloseZero(AlmostTolerance, normalizationFactor);
    }

    /// <summary>
    /// 判断浮点数是否近似为 0。
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyZero(this double a)
    {
        return Math.Abs(a) < NearlyTolerance;
    }

    /// <summary>
    /// 判断浮点数是否几乎为 0。
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostZero(this double a)
    {
        return Math.Abs(a) < AlmostTolerance;
    }

    #endregion
}