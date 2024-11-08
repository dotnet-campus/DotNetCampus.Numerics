namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 2 维仿射变换的扩展方法。
/// </summary>
public static class AffineTransformation2DExtensions
{
    /// <summary>
    /// 判断仿射变换是否可逆。
    /// </summary>
    /// <param name="transformation">要判断的仿射变换。</param>
    /// <returns>如果可逆，则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    public static bool HasInverse(this AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);
        var det = transformation.M11 * transformation.M22 - transformation.M12 * transformation.M21;
        return det != 0;
    }

    /// <summary>
    /// 判断是否为单位矩阵的仿射变换。
    /// </summary>
    /// <param name="transformation">要判断的仿射变换。</param>
    /// <returns>如果是单位矩阵，则返回 <see langword="true"/>；否则返回 <see langword="false"/>。</returns>
    public static bool IsIdentity(this AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);
        return transformation is { M11: 1, M12: 0, M21: 0, M22: 1, OffsetX: 0, OffsetY: 0 };
    }
}
