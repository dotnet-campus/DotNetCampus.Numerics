namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 2 维直线。
/// </summary>
public readonly record struct Line2D : IAffineTransformable2D<Line2D>
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
        return new Line2D(point1, point2 - point1);
    }

    #endregion

    #region 属性

    /// <summary>
    /// 单位方向向量。两个相同的直线的单位方向向量相同或相反。
    /// </summary>
    public Vector2D UnitDirectionVector
    {
        get;
        init => field = value.Normalized;
    }

    /// <summary>
    /// 直线上的一个点，会作为计算投影位置等值的参考位置。
    /// </summary>
    public Point2D PointBase { get; init; }

    #endregion

    #region 构造函数

    /// <summary>
    /// 2 维直线。
    /// </summary>
    /// <param name="pointBase">直线上的一个点，会作为计算投影位置等值的参考位置。</param>
    /// <param name="directionVector">方向向量。</param>
    public Line2D(Point2D pointBase, Vector2D directionVector)
    {
        PointBase = pointBase;
        UnitDirectionVector = directionVector.Normalized;
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
        {
            return null;
        }

        var vector = other.PointBase - PointBase;
        var position = vector.Det(other.UnitDirectionVector) / det;
        return GetPoint(position);
    }

    #endregion

    #region Transforms

    /// <inheritdoc />
    public Line2D Transform(AffineTransformation2D transformation)
    {
        var point1 = PointBase.Transform(transformation);
        var point2 = (PointBase + UnitDirectionVector).Transform(transformation);
        return Create(point1, point2);
    }

    /// <inheritdoc />
    public Line2D ScaleTransform(Scaling2D scaling)
    {
        var point1 = PointBase.ScaleTransform(scaling);
        var point2 = (PointBase + UnitDirectionVector).ScaleTransform(scaling);
        return Create(point1, point2);
    }

    /// <inheritdoc />
    public Line2D Transform(SimilarityTransformation2D transformation)
    {
        var point1 = PointBase.Transform(transformation);
        var point2 = (PointBase + UnitDirectionVector).Transform(transformation);
        return Create(point1, point2);
    }

    /// <inheritdoc />
    public Line2D ScaleTransform(double scaling)
    {
        var point1 = PointBase.ScaleTransform(scaling);
        var point2 = (PointBase + UnitDirectionVector).ScaleTransform(scaling);
        return Create(point1, point2);
    }

    /// <inheritdoc />
    public Line2D RotateTransform(AngularMeasure rotation)
    {
        var point1 = PointBase.RotateTransform(rotation);
        var point2 = (PointBase + UnitDirectionVector).RotateTransform(rotation);
        return Create(point1, point2);
    }

    /// <inheritdoc />
    public Line2D TranslateTransform(Vector2D translation)
    {
        var point1 = PointBase.TranslateTransform(translation);
        var point2 = (PointBase + UnitDirectionVector).TranslateTransform(translation);
        return Create(point1, point2);
    }

    #endregion
}
