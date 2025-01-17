namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 2 维几何接口。
/// </summary>
public interface IGeometry2D
{
    /// <summary>
    /// 获取曲线的包围盒。
    /// </summary>
    /// <returns>曲线的包围盒。</returns>
    BoundingBox2D GetBoundingBox();
}
