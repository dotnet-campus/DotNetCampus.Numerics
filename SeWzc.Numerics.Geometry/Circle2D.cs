namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 圆。
/// </summary>
/// <param name="Center">圆心。</param>
/// <param name="Radius">圆半径。</param>
public readonly record struct Circle2D(Point2D Center, double Radius)
{
    #region 成员方法

    /// <summary>
    /// 获取圆上指定角方向上的点。
    /// </summary>
    /// <param name="angle">角度。</param>
    /// <returns>圆上的点。</returns>
    public Point2D GetPoint(AngularMeasure angle)
    {
        return Center + Radius * angle.UnitVector;
    }

    /// <summary>
    /// 计算圆与直线的交点。
    /// </summary>
    /// <param name="line">直线。</param>
    /// <param name="index">交点索引。只有 0 和 1 两种选择。</param>
    /// <returns>如果没有对应的交点，则返回 <see langword="null" />，否则返回该交点。</returns>
    public Point2D? Intersection(Line2D line, int index)
    {
        var projection = line.Projection(Center);
        var distance = line.Distance(Center);
        if (distance > Radius)
            return null;

        var offset = Math.Sqrt(Radius * Radius - distance * distance);
        var position1 = projection - offset;
        var position2 = projection + offset;
        return line.GetPoint(index == 0 ? position1 : position2);
    }

    /// <summary>
    /// 计算圆与线段的交点。
    /// </summary>
    /// <param name="segment">线段。</param>
    /// <param name="index">交点索引。只有 0 和 1 两种选择。</param>
    /// <returns>如果没有对应的交点，则返回 <see langword="null" />，否则返回该交点。</returns>
    public Point2D? Intersection(Segment2D segment, int index)
    {
        var intersection = Intersection(segment.Line, index);
        if (intersection == null)
            return null;

        var radio = segment.Line.Projection(intersection.Value) / segment.Length;
        if (radio.IsInZeroToOne())
            return null;

        return intersection;
    }

    /// <summary>
    /// 计算圆与另一个圆的交点。
    /// </summary>
    /// <param name="other">另一个圆。</param>
    /// <param name="index">交点索引。只有 0 和 1 两种选择。</param>
    /// <returns>如果没有对应的交点，则返回 <see langword="null" />，否则返回该交点。</returns>
    public Point2D? Intersection(Circle2D other, int index)
    {
        var centerVector = other.Center - Center;
        var centerDistance = centerVector.Length;
        if (centerDistance > Radius + other.Radius || centerDistance < Math.Abs(Radius - other.Radius))
            return null;

        var centerUnitVector = centerVector / centerDistance;
        var position = (Radius * Radius - other.Radius * other.Radius + centerDistance * centerDistance) / (2 * centerDistance);
        var h = Math.Sqrt(Radius * Radius - position * position);
        return index == 0
            ? Center + position * centerUnitVector + h * centerUnitVector.NormalVector
            : Center + position * centerUnitVector - h * centerUnitVector.NormalVector;
    }

    /// <summary>
    /// 获取指定点相对于圆心的角度。
    /// </summary>
    /// <param name="point">指定点。</param>
    /// <returns>角度。</returns>
    public AngularMeasure GetAngle(Point2D point)
    {
        return AngularMeasure.FromRadian(Math.Atan2(point.Y - Center.Y, point.X - Center.X));
    }

    #endregion
}