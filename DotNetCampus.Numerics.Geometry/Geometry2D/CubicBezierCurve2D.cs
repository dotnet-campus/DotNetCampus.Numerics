using DotNetCampus.Numerics.Functions;

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
    public BoundingBox2D GetBoundingBox()
    {
        // x(t) = (1 - t)^3 * x0 + 3 * (1 - t)^2 * t * x1 + 3 * (1 - t) * t^2 * x2 + t^3 * x3 = (-x0 + 3 * x1 - 3 * x2 + x3) * t^3 + 3 * (x0 - 2 * x1 + x2) * t^2 + 3 * (x1 - x0) * t + x0
        // y(t) = (1 - t)^3 * y0 + 3 * (1 - t)^2 * t * y1 + 3 * (1 - t) * t^2 * y2 + t^3 * y3 = (-y0 + 3 * y1 - 3 * y2 + y3) * t^3 + 3 * (y0 - 2 * y1 + y2) * t^2 + 3 * (y1 - y0) * t + y0
        var xFunction = new CubicFunction<double>(-Start.X + 3 * Control1.X - 3 * Control2.X + End.X, 3 * (Start.X - 2 * Control1.X + Control2.X),
            3 * (Control1.X - Start.X), Start.X);
        var yFunction = new CubicFunction<double>(-Start.Y + 3 * Control1.Y - 3 * Control2.Y + End.Y, 3 * (Start.Y - 2 * Control1.Y + Control2.Y),
            3 * (Control1.Y - Start.Y), Start.Y);
        var valueRange = Interval<double>.Create(0, 1);
        var xRange = xFunction.GetValueRange(valueRange);
        var yRange = yFunction.GetValueRange(valueRange);
        return BoundingBox2D.Create(xRange.Start, yRange.Start, xRange.End, yRange.End);
    }

    #endregion

    #region Transforms

    /// <inheritdoc />
    public CubicBezierCurve2D Transform(AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        return new CubicBezierCurve2D(
            Start.Transform(transformation),
            Control1.Transform(transformation),
            Control2.Transform(transformation),
            End.Transform(transformation));
    }

    /// <inheritdoc />
    public CubicBezierCurve2D ScaleTransform(Scaling2D scaling)
    {
        return new CubicBezierCurve2D(
            Start.ScaleTransform(scaling),
            Control1.ScaleTransform(scaling),
            Control2.ScaleTransform(scaling),
            End.ScaleTransform(scaling));
    }

    /// <inheritdoc />
    public CubicBezierCurve2D Transform(SimilarityTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        return new CubicBezierCurve2D(
            transformation.Transform(Start),
            transformation.Transform(Control1),
            transformation.Transform(Control2),
            transformation.Transform(End));
    }

    /// <inheritdoc />
    public CubicBezierCurve2D ScaleTransform(double scaling)
    {
        return new CubicBezierCurve2D(
            Start.ScaleTransform(scaling),
            Control1.ScaleTransform(scaling),
            Control2.ScaleTransform(scaling),
            End.ScaleTransform(scaling));
    }

    /// <inheritdoc />
    public CubicBezierCurve2D RotateTransform(AngularMeasure rotation)
    {
        return new CubicBezierCurve2D(
            Start.RotateTransform(rotation),
            Control1.RotateTransform(rotation),
            Control2.RotateTransform(rotation),
            End.RotateTransform(rotation));
    }

    /// <inheritdoc />
    public CubicBezierCurve2D TranslateTransform(Vector2D translation)
    {
        return new CubicBezierCurve2D(
            Start.TranslateTransform(translation),
            Control1.TranslateTransform(translation),
            Control2.TranslateTransform(translation),
            End.TranslateTransform(translation));
    }

    #endregion
}
