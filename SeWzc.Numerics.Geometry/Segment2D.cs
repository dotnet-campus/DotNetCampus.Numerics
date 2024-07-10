namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 2 维线段。
/// </summary>
/// <param name="Line"></param>
/// <param name="Length"></param>
public readonly record struct Segment2D(Line2D Line, double Length) : IAffineTransformable2D<Segment2D>
{
    #region 静态方法

    /// <summary>
    /// 通过两个点创建线段。
    /// </summary>
    /// <param name="point1">线段的起点。</param>
    /// <param name="point2">线段的终点。</param>
    /// <returns>创建的线段。</returns>
    public static Segment2D Create(Point2D point1, Point2D point2)
    {
        var lineVector = point2 - point1;
        var lineVectorLength = lineVector.Length;
        return new Segment2D(new Line2D(point1, lineVector / lineVectorLength), lineVectorLength);
    }

    #endregion

    #region 属性

    /// <summary>
    /// 获取线段的起点。
    /// </summary>
    public Point2D StartPoint => Line.PointBase;

    /// <summary>
    /// 获取线段的终点。
    /// </summary>
    public Point2D EndPoint => StartPoint + UnitDirectionVector * Length;

    /// <summary>
    /// 获取线段的单位方向向量。
    /// </summary>
    public Vector2D UnitDirectionVector => Line.UnitDirectionVector;

    #endregion

    #region 成员方法

    /// <summary>
    /// 获取两条线段的交点。
    /// </summary>
    /// <param name="other">另一条线段。</param>
    /// <returns>两条线段的交点，如果不存在交点则返回 <see langword="null" />。</returns>
    public Point2D? Intersection(Segment2D other)
    {
        var det = UnitDirectionVector.Det(other.UnitDirectionVector);
        if (det.IsAlmostZero())
            return null;

        var vector = other.StartPoint - StartPoint;
        var position = vector.Det(other.UnitDirectionVector) / det;
        var radio = position / Length;
        var radio2 = vector.Det(UnitDirectionVector) / det / other.Length;
        if (radio.IsInZeroToOne() || radio2.IsInZeroToOne())
            return null;

        return Line.GetPoint(position);
    }

    #endregion

    /// <inheritdoc />
    public Segment2D Transform(AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);
        return new Segment2D(Line.Transform(transformation), Length);
    }
}