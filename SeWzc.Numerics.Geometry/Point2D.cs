namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 2 维点。
/// </summary>
/// <param name="X">点的 X 坐标。</param>
/// <param name="Y">点的 Y 坐标。</param>
public readonly record struct Point2D(double X, double Y) : IPoint<Point2D, Vector2D, double>, IAffineTransformable2D<Point2D>
{
    #region 成员方法

    /// <inheritdoc />
    public Point2D Transform(AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);
        return transformation.Transform(this);
    }

    #endregion

    #region 运算符重载

    /// <inheritdoc />
    public static Point2D operator +(Point2D point, Vector2D vector)
    {
        return new Point2D(point.X + vector.X, point.Y + vector.Y);
    }

    /// <inheritdoc />
    public static Point2D operator +(Vector2D vector, Point2D point)
    {
        return new Point2D(point.X + vector.X, point.Y + vector.Y);
    }

    /// <inheritdoc />
    public static Vector2D operator -(Point2D point1, Point2D point2)
    {
        return new Vector2D(point1.X - point2.X, point1.Y - point2.Y);
    }

    /// <inheritdoc />
    public static Point2D operator -(Point2D point, Vector2D vector)
    {
        return new Point2D(point.X - vector.X, point.Y - vector.Y);
    }

    /// <inheritdoc />
    public static Point2D operator *(Point2D point, double scalar)
    {
        return new Point2D(point.X * scalar, point.Y * scalar);
    }

    /// <inheritdoc />
    public static Point2D operator *(double scalar, Point2D point)
    {
        return new Point2D(point.X * scalar, point.Y * scalar);
    }

    /// <inheritdoc />
    public static Point2D operator /(Point2D point, double scalar)
    {
        return new Point2D(point.X / scalar, point.Y / scalar);
    }

    /// <inheritdoc />
    public static explicit operator Vector2D(Point2D point)
    {
        return new Vector2D(point.X, point.Y);
    }

    /// <inheritdoc />
    public static explicit operator Point2D(Vector2D vector)
    {
        return new Point2D(vector.X, vector.Y);
    }

    #endregion
}