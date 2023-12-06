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
    /// 获取点在直线上的投影位置。
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public double Projection(Point2D point)
    {
        var vector = point - PointBase;
        return vector.GetProjectionOn(UnitDirectionVector);
    }

    /// <summary>
    /// 获取点到直线的距离。
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public double Distance(Point2D point)
    {
        var vector = point - PointBase;
        return vector.Det(UnitDirectionVector);
    }

    /// <summary>
    /// 获取两条直线的交点。
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Point2D? Intersection(Line2D other)
    {
        var det = UnitDirectionVector.Det(other.UnitDirectionVector);
        if (Math.Abs(det) < 1e-10)
            return null;

        var vector = other.PointBase - PointBase;
        var position = vector.Det(other.UnitDirectionVector) / det;
        return GetPoint(position);
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