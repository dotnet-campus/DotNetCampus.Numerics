using DotNetCampus.Numerics.Functions;

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
    public BoundingBox2D GetBoundingBox()
    {
        // x(t) = (1 - t)² * x0 + 2 * (1 - t) * t * x1 + t² * x2 = (x0 - 2 * x1 + x2) * t² + 2 * (x1 - x0) * t + x0
        // y(t) = (1 - t)² * y0 + 2 * (1 - t) * t * y1 + t² * y2 = (y0 - 2 * y1 + y2) * t² + 2 * (y1 - y0) * t + y0
        var xFunction = new QuadraticFunction<double>(Start.X - 2 * Control.X + End.X, 2 * (Control.X - Start.X), End.X - 2 * Control.X + Start.X);
        var yFunction = new QuadraticFunction<double>(Start.Y - 2 * Control.Y + End.Y, 2 * (Control.Y - Start.Y), End.Y - 2 * Control.Y + Start.Y);
        var valueRange = Interval<double>.Create(0, 1);
        var xRange = xFunction.GetValueRange(valueRange);
        var yRange = yFunction.GetValueRange(valueRange);
        return BoundingBox2D.Create(xRange.Start, yRange.Start, xRange.End, yRange.End);
    }

    #endregion

    #region Transforms

    /// <inheritdoc />
    public QuadraticBezierCurve2D Transform(AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        return new QuadraticBezierCurve2D(
            Start.Transform(transformation),
            Control.Transform(transformation),
            End.Transform(transformation));
    }

    /// <inheritdoc />
    public QuadraticBezierCurve2D ScaleTransform(Scaling2D scaling)
    {
        return new QuadraticBezierCurve2D(
            Start.ScaleTransform(scaling),
            Control.ScaleTransform(scaling),
            End.ScaleTransform(scaling));
    }

    /// <inheritdoc />
    public QuadraticBezierCurve2D Transform(SimilarityTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        return new QuadraticBezierCurve2D(
            transformation.Transform(Start),
            transformation.Transform(Control),
            transformation.Transform(End));
    }

    /// <inheritdoc />
    public QuadraticBezierCurve2D ScaleTransform(double scaling)
    {
        return new QuadraticBezierCurve2D(
            Start.ScaleTransform(scaling),
            Control.ScaleTransform(scaling),
            End.ScaleTransform(scaling));
    }

    /// <inheritdoc />
    public QuadraticBezierCurve2D RotateTransform(AngularMeasure rotation)
    {
        return new QuadraticBezierCurve2D(
            Start.RotateTransform(rotation),
            Control.RotateTransform(rotation),
            End.RotateTransform(rotation));
    }

    /// <inheritdoc />
    public QuadraticBezierCurve2D TranslateTransform(Vector2D translation)
    {
        return new QuadraticBezierCurve2D(
            Start.TranslateTransform(translation),
            Control.TranslateTransform(translation),
            End.TranslateTransform(translation));
    }

    #endregion
}
