namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 2 维线段。
/// </summary>
/// <param name="StartPoint">线段的起点。</param>
/// <param name="EndPoint">线段的终点。</param>
public readonly record struct Segment2D(Point2D StartPoint, Point2D EndPoint) : IAffineTransformable2D<Segment2D>, IGeometry2D
{
    #region 静态方法

    /// <summary>
    /// 通过两个点创建线段。
    /// </summary>
    /// <param name="startPoint">线段的起点。</param>
    /// <param name="endPont">线段的终点。</param>
    /// <returns>创建的线段。</returns>
    public static Segment2D Create(Point2D startPoint, Point2D endPont)
    {
        return new Segment2D(startPoint, endPont);
    }

    #endregion

    #region 属性

    /// <summary>
    /// 获取线段的向量。
    /// </summary>
    public Vector2D DirectionVector => EndPoint - StartPoint;

    /// <summary>
    /// 获取线段的单位方向向量。
    /// </summary>
    public Vector2D UnitDirectionVector => DirectionVector.Normalized;

    /// <summary>
    /// 获取以线段起点为 <see cref="Line2D.PointBase" />，以线段方向向量为方向向量的直线。
    /// </summary>
    public Line2D Line => new(StartPoint, DirectionVector);

    /// <summary>
    /// 获取线段的长度。
    /// </summary>
    public double Length => DirectionVector.Length;

    #endregion

    #region 构造函数

    /// <summary>
    /// 以直线的 <see cref="Line2D.PointBase" /> 作为起始点，与直线相同的方向向量，创建指定长度的线段。
    /// </summary>
    /// <param name="Line">线段所在的直线。</param>
    /// <param name="Length">线段的长度。</param>
    public Segment2D(Line2D Line, double Length) : this(Line.PointBase, Line.GetPoint(Length))
    {
    }

    /// <summary>
    /// 以直线指定区间截取一个线段。截取区间对应的点通过 <see cref="Line2D.GetPoint(double)" /> 获取。
    /// </summary>
    /// <param name="Line">线段所在的直线。</param>
    /// <param name="interval">截取的区间。</param>
    public Segment2D(Line2D Line, Interval<double> interval) : this(Line.GetPoint(interval.Start), Line.GetPoint(interval.End))
    {
    }

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
        {
            return null;
        }

        var vector = other.StartPoint - StartPoint;
        var position = vector.Det(other.UnitDirectionVector) / det;
        var radio = position / Length;
        var radio2 = vector.Det(UnitDirectionVector) / det / other.Length;
        if (!radio.IsInZeroToOne() || !radio2.IsInZeroToOne())
        {
            return null;
        }

        return Line.GetPoint(position);
    }

    /// <inheritdoc />
    public Segment2D Transform(AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);
        return new Segment2D(Line.Transform(transformation), Length);
    }

    /// <inheritdoc />
    public BoundingBox2D GetBoundingBox()
    {
        return BoundingBox2D.Create(StartPoint, EndPoint);
    }

    #endregion
}
