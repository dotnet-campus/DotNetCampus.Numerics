namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 直线。
/// </summary>
/// <param name="PointBase"></param>
/// <param name="UnitDirectionVector"></param>
public readonly record struct Line2D(Point2D PointBase, Vector2D UnitDirectionVector)
{
    /// <summary>
    /// 根据位置获取点。
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Point2D GetPoint(double position)
    {
        return PointBase + UnitDirectionVector * position;
    }

    /// <summary>
    /// 通过两个点创建直线。
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    public static Line2D Create(Point2D point1, Point2D point2)
    {
        return new Line2D(point1, (point2 - point1).Normalized);
    }
}