namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 椭圆。
/// </summary>
/// <param name="Center">椭圆的中心。</param>
/// <param name="A">椭圆的 A 方向半轴。</param>
/// <param name="B">椭圆的 B 方向半轴。</param>
/// <param name="Angle">椭圆的旋转角。</param>
public readonly record struct Ellipse2D(Point2D Center, double A, double B, AngularMeasure Angle) : IAffineTransformable2D<Ellipse2D>
{
    #region 构造函数

    /// <summary>
    /// 通过圆创建椭圆。
    /// </summary>
    /// <param name="circle"></param>
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
}
