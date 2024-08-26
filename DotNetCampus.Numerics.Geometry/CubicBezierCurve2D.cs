namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 三次贝塞尔曲线。
/// </summary>
/// <param name="Start">曲线的起点。</param>
/// <param name="Control1">曲线的控制点1。</param>
/// <param name="Control2">曲线的控制点2。</param>
/// <param name="End">曲线的终点。</param>
public record CubicBezierCurve2D(Point2D Start, Point2D Control1, Point2D Control2, Point2D End)
    : IBezierCurve<Point2D, Vector2D, double>, IAffineTransformable2D<CubicBezierCurve2D>
{
    #region 成员方法

    /// <inheritdoc />
    public Point2D GetPoint(double t)
    {
        var u = 1 - t;
        return Point2D.WeightedSum(u * u * u, Start, 3 * u * u * t, Control1, 3 * u * t * t, Control2, t * t * t, End);
    }

    /// <inheritdoc />
    public Vector2D GetTangent(double t)
    {
        var u = 1 - t;
        return 3 * u * u * (Control1 - Start) + 6 * u * t * (Control2 - Control1) + 3 * t * t * (End - Control2);
    }

    /// <inheritdoc />
    public Vector2D GetNormal(double t)
    {
        return GetTangent(t).NormalVector;
    }

    /// <inheritdoc />
    public double GetCurvature(double t)
    {
        // 一阶导数
        var tangent = GetTangent(t);
        // 二阶导数
        var u = 1 - t;
        var vector = 6 * (u * (Start - Control1) + (t - u) * (Control1 - Control2) + t * (End - Control2));
        return tangent.Det(vector).Abs() / Math.Pow(tangent.Length, 3);
    }

    /// <inheritdoc />
    public CubicBezierCurve2D Transform(AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        return new CubicBezierCurve2D(
            transformation.Transform(Start),
            transformation.Transform(Control1),
            transformation.Transform(Control2),
            transformation.Transform(End));
    }

    #endregion
}
