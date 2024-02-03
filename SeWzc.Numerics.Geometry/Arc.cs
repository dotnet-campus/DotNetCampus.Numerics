namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 圆弧。
/// </summary>
/// <param name="Circle">圆弧对应的圆。</param>
/// <param name="StartAngle">开始角。</param>
/// <param name="Angle">圆心角。</param>
public readonly record struct Arc2D(Circle2D Circle, AngularMeasure StartAngle, AngularMeasure Angle)
{
    #region 属性

    /// <summary>
    /// 圆心角。
    /// </summary>
    public AngularMeasure EndAngle => Angle + StartAngle;

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

    public Point2D? Intersection(Line2D line, int index)
    {
        var intersections = Circle.Intersection(line, index);
        if (intersections == null)
            return null;

        var intersection = intersections.Value;
        var angleRadio = ((intersection - Circle.Center).Angle - StartAngle).Normalized / Angle;
        if (angleRadio is < -1e-10 or > 1 + 1e-10)
            return null;

        return intersection;
    }

    public Point2D? Intersection(Segment2D segment, int index)
    {
        var intersections = Circle.Intersection(segment.Line, index);
        if (intersections == null)
            return null;

        var intersection = intersections.Value;
        var angleRadio = ((intersection - Circle.Center).Angle - StartAngle).Normalized / Angle;
        var lengthRadio = segment.Line.Projection(intersection) / segment.Length;
        if (angleRadio is < -1e-10 or > 1 + 1e-10 || lengthRadio is < -1e-10 or > 1 + 1e-10)
            return null;

        return intersection;
    }


    public Point2D? Intersection(Circle2D circle, int index)
    {
        var intersections = Circle.Intersection(circle, index);
        if (intersections == null)
            return null;

        var intersection = intersections.Value;
        var angleRadio = ((intersection - Circle.Center).Angle - StartAngle).Normalized / Angle;
        if (angleRadio is < -1e-10 or > 1 + 1e-10)
            return null;

        return intersection;
    }

    public Point2D? Intersection(Arc2D other, int index)
    {
        var intersections = Circle.Intersection(other.Circle, index);
        if (intersections == null)
            return null;

        var intersection = intersections.Value;
        var angleRadio = ((intersection - Circle.Center).Angle - StartAngle).Normalized / Angle;
        var angleRadio2 = ((intersection - other.Circle.Center).Angle - other.StartAngle).Normalized / Angle;
        if (angleRadio is < -1e-10 or > 1 + 1e-10 || angleRadio2 is < -1e-10 or > 1 + 1e-10)
            return null;

        return intersection;
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

    #endregion
}