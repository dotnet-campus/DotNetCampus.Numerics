using System.Runtime.CompilerServices;

namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 几何数值比较的帮助类。
/// </summary>
public static class GeometryNumericsEqualHelper
{
    #region Point2D

    /// <summary>
    /// 判断两个点是否近似相同。
    /// </summary>
    /// <param name="a">要比较的第一个点。</param>
    /// <param name="b">要比较的第二个点。</param>
    /// <returns>如果两个点是否近似相同，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyEqual(this Point2D a, Point2D b)
    {
        return a.ToVector().IsNearlyEqual(b.ToVector());
    }

    /// <summary>
    /// 判断两个点是否几乎相同。
    /// </summary>
    /// <param name="a">要比较的第一个点。</param>
    /// <param name="b">要比较的第二个点。</param>
    /// <returns>如果两个点是否几乎相同，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostEqual(this Point2D a, Point2D b)
    {
        return a.ToVector().IsAlmostEqual(b.ToVector());
    }

    /// <summary>
    /// 判断两个缩放是否近似相同。
    /// </summary>
    /// <param name="a">要比较的第一个缩放。</param>
    /// <param name="b">要比较的第二个缩放。</param>
    /// <returns>如果两个缩放是否近似相同，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNearlyEqual(this Scaling2D a, Scaling2D b)
    {
        return a.ScaleX.IsNearlyEqual(b.ScaleX) && a.ScaleY.IsNearlyEqual(b.ScaleY);
    }

    /// <summary>
    /// 判断两个缩放是否几乎相同。
    /// </summary>
    /// <param name="a">要比较的第一个缩放。</param>
    /// <param name="b">要比较的第二个缩放。</param>
    /// <returns>如果两个缩放是否几乎相同，则返回 <see langword="true" />；否则返回 <see langword="false" />。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlmostEqual(this Scaling2D a, Scaling2D b)
    {
        return a.ScaleX.IsAlmostEqual(b.ScaleX) && a.ScaleY.IsAlmostEqual(b.ScaleY);
    }

    #endregion
}
