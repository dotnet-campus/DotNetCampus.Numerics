using System.Runtime.CompilerServices;

namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 2 维点。
/// </summary>
/// <param name="X">点的 X 坐标。</param>
/// <param name="Y">点的 Y 坐标。</param>
public readonly record struct Point2D(double X, double Y) : IPoint<Point2D, Vector2D, double>, IAffineTransformable2D<Point2D>
{
    #region 静态方法

    /// <inheritdoc />
    public static Point2D Middle(Point2D point1, Point2D point2)
    {
        return new Point2D((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
    }

    /// <inheritdoc />
    public static Point2D Middle(IReadOnlyList<Point2D> points)
    {
        ArgumentNullException.ThrowIfNull(points);
        if (points.Count == 0)
        {
            throw new ArgumentException("The point list cannot be empty.");
        }

        var x = 0.0;
        var y = 0.0;
        foreach (var point in points)
        {
            x += point.X;
            y += point.Y;
        }

        return new Point2D(x / points.Count, y / points.Count);
    }

    /// <inheritdoc />
    public static Point2D WeightedSum(double weight1, Point2D point1, double weight2, Point2D point2)
    {
        return new Point2D(weight1 * point1.X + weight2 * point2.X, weight1 * point1.Y + weight2 * point2.Y);
    }

    /// <inheritdoc />
    public static Point2D WeightedSum(double weight1, Point2D point1, double weight2, Point2D point2, double weight3, Point2D point3)
    {
        return new Point2D(
            weight1 * point1.X + weight2 * point2.X + weight3 * point3.X,
            weight1 * point1.Y + weight2 * point2.Y + weight3 * point3.Y);
    }

    /// <inheritdoc />
    public static Point2D WeightedSum(double weight1, Point2D point1, double weight2, Point2D point2, double weight3, Point2D point3, double weight4,
        Point2D point4)
    {
        return new Point2D(
            weight1 * point1.X + weight2 * point2.X + weight3 * point3.X + weight4 * point4.X,
            weight1 * point1.Y + weight2 * point2.Y + weight3 * point3.Y + weight4 * point4.Y);
    }

    #endregion

    #region 成员方法

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2D ToVector()
    {
        return new Vector2D(X, Y);
    }

    #endregion

    #region 运算符重载

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2D operator +(Point2D point, Vector2D vector)
    {
        return new Point2D(point.X + vector.X, point.Y + vector.Y);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2D operator +(Vector2D vector, Point2D point)
    {
        return new Point2D(point.X + vector.X, point.Y + vector.Y);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2D operator -(Point2D point1, Point2D point2)
    {
        return new Vector2D(point1.X - point2.X, point1.Y - point2.Y);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2D operator -(Point2D point, Vector2D vector)
    {
        return new Point2D(point.X - vector.X, point.Y - vector.Y);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2D operator *(Point2D point, double scalar)
    {
        return new Point2D(point.X * scalar, point.Y * scalar);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2D operator *(double scalar, Point2D point)
    {
        return new Point2D(point.X * scalar, point.Y * scalar);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point2D operator /(Point2D point, double scalar)
    {
        return new Point2D(point.X / scalar, point.Y / scalar);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector2D(Point2D point)
    {
        return new Vector2D(point.X, point.Y);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Point2D(Vector2D vector)
    {
        return new Point2D(vector.X, vector.Y);
    }

    #endregion

    #region Transforms

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2D Transform(AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);
        return transformation.Transform(this);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2D ScaleTransform(Scaling2D scaling)
    {
        return new Point2D(X * scaling.ScaleX, Y * scaling.ScaleY);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2D Transform(SimilarityTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);
        return transformation.Transform(this);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2D ScaleTransform(double scaling)
    {
        return new Point2D(X * scaling, Y * scaling);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2D RotateTransform(AngularMeasure rotation)
    {
        var (sin, cos) = rotation.SinCos();
        return new Point2D(X * cos - Y * sin, X * sin + Y * cos);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point2D TranslateTransform(Vector2D translation)
    {
        return this + translation;
    }

    #endregion
}
