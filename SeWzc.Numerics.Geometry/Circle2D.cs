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

    public Point2D? Intersection(Segment2D segment, int index)
    {
        var intersection = Intersection(segment.Line, index);
        if (intersection == null)
            return null;

        var radio = segment.Line.Projection(intersection.Value) / segment.Length;
        if (radio is < -1e-10 or > 1 + 1e-10)
            return null;

        return intersection;
    }

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

    public AngularMeasure GetAngle(Point2D point)
    {
        return AngularMeasure.FromRadian(Math.Atan2(point.Y - Center.Y, point.X - Center.X));
    }
}