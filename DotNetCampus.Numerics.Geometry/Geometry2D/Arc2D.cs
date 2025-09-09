using DotNetCampus.Numerics.Functions;

namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 圆弧。
/// </summary>
/// <param name="Circle">圆弧对应的圆。</param>
/// <param name="StartAngle">开始角。</param>
/// <param name="AngleSize">圆心角。</param>
public readonly record struct Arc2D(Circle2D Circle, AngularMeasure StartAngle, AngularMeasure AngleSize) : IGeometry2D, ISimilarityTransformable2D<Arc2D>
{
    #region 属性

    /// <summary>
    /// 圆心角。
    /// </summary>
    public AngularMeasure EndAngle => StartAngle + AngleSize;

    /// <summary>
    /// 开始点坐标。
    /// </summary>
    public Point2D StartPoint => Circle.GetPoint(StartAngle);

    /// <summary>
    /// 结束点坐标。
    /// </summary>
    public Point2D EndPoint => Circle.GetPoint(EndAngle);

    #endregion

    #region 成员方法

    /// <summary>
    /// 计算圆弧与直线的交点。
    /// </summary>
    /// <param name="line">直线。</param>
    /// <param name="index">交点索引。值只能是 0 或 1。</param>
    /// <returns>交点坐标。如果没有获取到对应的交点，则返回 <see langword="null" />。</returns>
    public Point2D? Intersection(Line2D line, int index)
    {
        var intersections = Circle.Intersection(line, index);
        if (intersections == null)
        {
            return null;
        }

        var intersection = intersections.Value;
        var angleRadio = ((intersection - Circle.Center).Angle - StartAngle).Normalized / AngleSize;
        return angleRadio.IsInZeroToOne() ? intersection : null;
    }

    /// <summary>
    /// 计算圆弧与线段的交点。
    /// </summary>
    /// <param name="segment">线段。</param>
    /// <param name="index">交点索引。值只能是 0 或 1。</param>
    /// <returns>交点坐标。如果没有获取到对应的交点，则返回 <see langword="null" />。</returns>
    public Point2D? Intersection(Segment2D segment, int index)
    {
        var intersections = Circle.Intersection(segment.Line, index);
        if (intersections == null)
        {
            return null;
        }

        var intersection = intersections.Value;
        var angleRadio = ((intersection - Circle.Center).Angle - StartAngle).Normalized / AngleSize;
        var lengthRadio = segment.Line.Projection(intersection) / segment.Length;
        return angleRadio.IsInZeroToOne() && lengthRadio.IsInZeroToOne() ? intersection : null;
    }

    /// <summary>
    /// 计算圆弧与圆的交点。
    /// </summary>
    /// <param name="circle">要计算交点的圆。</param>
    /// <param name="index">交点索引。值只能是 0 或 1。</param>
    /// <returns>交点坐标。如果没有获取到对应的交点，则返回 <see langword="null" />。</returns>
    public Point2D? Intersection(Circle2D circle, int index)
    {
        var intersections = Circle.Intersection(circle, index);
        if (intersections == null)
        {
            return null;
        }

        var intersection = intersections.Value;
        var angleRadio = ((intersection - Circle.Center).Angle - StartAngle).Normalized / AngleSize;
        return angleRadio.IsInZeroToOne() ? intersection : null;
    }

    /// <summary>
    /// 计算圆弧与圆弧的交点。
    /// </summary>
    /// <param name="other">要计算交点的另一个圆弧。</param>
    /// <param name="index">交点索引。值只能是 0 或 1。</param>
    /// <returns>交点坐标。如果没有获取到对应的交点，则返回 <see langword="null" />。</returns>
    public Point2D? Intersection(Arc2D other, int index)
    {
        var intersections = Circle.Intersection(other.Circle, index);
        if (intersections == null)
        {
            return null;
        }

        var intersection = intersections.Value;
        var angleRadio = ((intersection - Circle.Center).Angle - StartAngle).Normalized / AngleSize;
        var angleRadio2 = ((intersection - other.Circle.Center).Angle - other.StartAngle).Normalized / AngleSize;
        return angleRadio.IsInZeroToOne() && angleRadio2.IsInZeroToOne() ? intersection : null;
    }

    /// <summary>
    /// 获取点在圆弧上的角度。以 <see cref="StartAngle" /> 为 0 角。
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public AngularMeasure GetAngle(Point2D point)
    {
        return ((point - Circle.Center).Angle - StartAngle).Normalized;
    }

    /// <inheritdoc />
    public BoundingBox2D GetBoundingBox()
    {
        var cos = new CosFunction<double>(Circle.Radius, Circle.Center.X, 1, 0);
        var sin = new SinFunction<double>(Circle.Radius, Circle.Center.Y, 1, 0);
        var range = new Interval<double>(StartAngle.Radian, EndAngle.Radian);
        return BoundingBox2D.Create(
            new Point2D(cos.GetMin(range), sin.GetMin(range)),
            new Point2D(cos.GetMax(range), sin.GetMax(range)));
    }

    #endregion

    #region Transforms

    /// <inheritdoc />
    public Arc2D Transform(SimilarityTransformation2D transformation)
    {
        return new Arc2D(Circle.Transform(transformation), StartAngle + transformation.Rotation, AngleSize);
    }

    /// <inheritdoc />
    public Arc2D ScaleTransform(double scaling)
    {
        return new Arc2D(Circle.ScaleTransform(scaling), StartAngle, AngleSize);
    }

    /// <inheritdoc />
    public Arc2D RotateTransform(AngularMeasure rotation)
    {
        return new Arc2D(Circle.RotateTransform(rotation), StartAngle + rotation, AngleSize);
    }

    /// <inheritdoc />
    public Arc2D TranslateTransform(Vector2D translation)
    {
        return new Arc2D(Circle.TranslateTransform(translation), StartAngle, AngleSize);
    }

    #endregion
}
