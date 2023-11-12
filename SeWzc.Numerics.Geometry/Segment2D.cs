namespace SeWzc.Numerics.Geometry;

public readonly record struct Segment2D(Line2D Line, double Length)
{
    public Point2D StartPoint => Line.PointBase;

    public Point2D EndPoint => StartPoint + UnitDirectionVector * Length;

    public Vector2D UnitDirectionVector => Line.UnitDirectionVector;

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