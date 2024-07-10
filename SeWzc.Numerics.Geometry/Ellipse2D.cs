namespace SeWzc.Numerics.Geometry;

public readonly record struct Ellipse2D(Point2D Center, double A, double B, AngularMeasure Angle) : IAffineTransformable2D<Ellipse2D>
{
    #region 构造函数

    public Ellipse2D(Circle2D circle) : this(circle.Center, circle.Radius, circle.Radius, AngularMeasure.Zero)
    {
    }

    #endregion

    /// <summary>
    /// 椭圆上轴端点 A。
    /// </summary>
    public Point2D AxisEndpointA => Center + A * Angle.UnitVector;

    /// <summary>
    /// 椭圆上轴端点 B。
    /// </summary>
    public Point2D AxisEndpointB => Center + B * (Angle + AngularMeasure.HalfPi).UnitVector;

    /// <inheritdoc />
    public Ellipse2D Transform(AffineTransformation2D transformation)
    {
        throw new NotImplementedException();
    }

    public static Ellipse2D Create(Point2D center, Point2D pointA, Point2D pointB)
    {
        var v1 = pointA - center;
        var v2 = pointB - center;
        var d = v1.Det(v2).Square();
        var m11 = (v1.Y.Square() + v2.Y.Square()) / d;
        var m12 = -(v1.X * v1.Y + v2.X * v2.Y) / d;
        var m21 = m12;
        var m22 = (v1.X.Square() + v2.X.Square()) / d;
        var affineTransformation2D = new AffineTransformation2D(m11, m12, m21, m22, 0, 0);

        var a = (pointA - center).Length;
        var b = (pointB - center).Length;
        var angle = (pointA - center).Angle;
        return new Ellipse2D(center, a, b, angle);
    }
}