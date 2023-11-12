namespace SeWzc.Numerics.Geometry;

public readonly record struct Segment2D(Line2D Line, double Length)
{
    public Point2D StartPoint => Line.PointBase;

    public Point2D EndPoint => StartPoint + UnitDirectionVector * Length;

    public Vector2D UnitDirectionVector => Line.UnitDirectionVector;

    /// <summary>
    /// 获取两条线段的交点。
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Point2D? Intersection(Segment2D other)
    {
        var det = UnitDirectionVector.Det(other.UnitDirectionVector);
        if (Math.Abs(det) < 1e-10)
            return null;

        var vector = other.StartPoint - StartPoint;
        var position = vector.Det(other.UnitDirectionVector) / det;
        var radio = position / Length;
        var radio2 = vector.Det(UnitDirectionVector) / det / other.Length;
        if (radio is < -1e-10 or > 1 + 1e-10 || radio2 is < -1e-10 or > 1 + 1e-10)
            return null;

        return Line.GetPoint(position);
    }

    /// <summary>
    /// 通过两个点创建线段。
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    public static Segment2D Create(Point2D point1, Point2D point2)
    {
        var lineVector = point2 - point1;
        var lineVectorLength = lineVector.Length;
        return new Segment2D(new Line2D(point1, lineVector / lineVectorLength), lineVectorLength);
    }
}