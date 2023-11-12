namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 圆。
/// </summary>
/// <param name="Center">圆心。</param>
/// <param name="Radius">圆半径。</param>
public readonly record struct Circle2D(Point2D Center, double Radius)
{
    /// <summary>
    /// 获取圆上的点。
    /// </summary>
    /// <param name="angle">角度。</param>
    /// <returns></returns>
    public Point2D GetPoint(AngularMeasure angle)
    {
        return Center + Radius * angle.UnitVector;
    }
}