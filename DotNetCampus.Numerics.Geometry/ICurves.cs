namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 曲线接口。
/// </summary>
public interface ICurve
{
    /// <summary>
    /// 获取曲线的包围盒。
    /// </summary>
    /// <returns>曲线的包围盒。</returns>
    BoundingBox2D GetBoundingBox();
}
