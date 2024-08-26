namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 2 维直线。
/// </summary>
/// <param name="PointBase"></param>
/// <param name="UnitDirectionVector"></param>
public readonly record struct Line2D(Point2D PointBase, Vector2D UnitDirectionVector) : IAffineTransformable2D<Line2D>
{
    #region 静态方法

    /// <summary>
    /// 通过两个点创建直线。
    /// </summary>
    /// <param name="point1">直线上的第一个点。</param>
    /// <param name="point2">直线上的第二个点。</param>
    /// <returns>创建的直线。</returns>
    public static Line2D Create(Point2D point1, Point2D point2)
    {
        return new Line2D(point1, (point2 - point1).Normalized);
    }

    #endregion

    #region 私有字段

    private readonly Vector2D _unitDirectionVector = UnitDirectionVector.Normalized;

    #endregion

    #region 属性

    /// <summary>
    /// 单位方向向量。
    /// </summary>
    public Vector2D UnitDirectionVector
    {
        get => _unitDirectionVector;
        init => _unitDirectionVector = value.Normalized;
    }

    #endregion

    #region 成员方法

    /// <summary>
    /// 根据位置获取点。
    /// </summary>
    /// <param name="position">点在直线上的位置。</param>
    /// <returns>直线上的点。</returns>
    public Point2D GetPoint(double position)
    {
        return PointBase + UnitDirectionVector * position;
    }

    /// <summary>
    /// 获取点在直线上的投影位置。
    /// </summary>
    /// <param name="point">要投影的点。</param>
    /// <returns>点在直线上的投影位置。</returns>
    public double Projection(Point2D point)
    {
        var vector = point - PointBase;
        return vector.GetProjectionOn(UnitDirectionVector);
    }

    /// <summary>
    /// 获取点到直线的距离。
    /// </summary>
    /// <param name="point">要计算距离的点。</param>
    /// <returns>点到直线的距离。</returns>
    public double Distance(Point2D point)
    {
        var vector = point - PointBase;
        return vector.Det(UnitDirectionVector);
    }

    /// <summary>
    /// 获取两条直线的交点。
    /// </summary>
    /// <param name="other">另一条直线。</param>
    /// <returns>两条直线的交点。</returns>
    public Point2D? Intersection(Line2D other)
    {
        var det = UnitDirectionVector.Det(other.UnitDirectionVector);
        if (det.IsAlmostZero())
            return null;

        var vector = other.PointBase - PointBase;
        var position = vector.Det(other.UnitDirectionVector) / det;
        return GetPoint(position);
    }

    /// <inheritdoc />
    public Line2D Transform(AffineTransformation2D transformation)
    {
        var point1 = PointBase.Transform(transformation);
        var point2 = (PointBase + UnitDirectionVector).Transform(transformation);
        return Create(point1, point2);
    }

    #endregion
}
