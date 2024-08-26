namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 二次贝塞尔曲线。
/// </summary>
/// <param name="Start">曲线的起点。</param>
/// <param name="Control">曲线的控制点。</param>
/// <param name="End">曲线的终点。</param>
public record QuadraticBezierCurve2D(Point2D Start, Point2D Control, Point2D End)
    : IBezierCurve<Point2D, Vector2D, double>, IAffineTransformable2D<QuadraticBezierCurve2D>
{
    #region 成员方法

    /// <inheritdoc />
    public Point2D GetPoint(double t)
    {
        var u = 1 - t;
        return Point2D.WeightedSum(u * u, Start, 2 * u * t, Control, t * t, End);
    }

    /// <inheritdoc />
    public Vector2D GetTangent(double t)
    {
        var u = 1 - t;
        return 2 * u * (Control - Start) + 2 * t * (End - Control);
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
        var vector = 2 * (Start - Control) + 2 * (Control - End);
        return tangent.Det(vector).Abs() / Math.Pow(tangent.Length, 3);
    }

    /// <inheritdoc />
    public QuadraticBezierCurve2D Transform(AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        return new QuadraticBezierCurve2D(
            transformation.Transform(Start),
            transformation.Transform(Control),
            transformation.Transform(End));
    }

    #endregion
}
