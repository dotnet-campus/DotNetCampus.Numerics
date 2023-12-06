namespace SeWzc.Numerics;

public readonly record struct Point2D(double X, double Y) : IPoint<Point2D, Vector2D, double>
{
    #region 运算符重载

    public static Point2D operator +(Point2D point1, Point2D point2)
    {
        return new Point2D(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static Point2D operator +(Point2D point, Vector2D vector)
    {
        return new Point2D(point.X + vector.X, point.Y + vector.Y);
    }

    public static Point2D operator +(Vector2D vector, Point2D point)
    {
        return new Point2D(point.X + vector.X, point.Y + vector.Y);
    }

    public static Vector2D operator -(Point2D point1, Point2D point2)
    {
        return new Vector2D(point1.X - point2.X, point1.Y - point2.Y);
    }

    public static Point2D operator -(Point2D point, Vector2D vector)
    {
        return new Point2D(point.X - vector.X, point.Y - vector.Y);
    }

    public static Point2D operator *(Point2D point, double scalar)
    {
        return new Point2D(point.X * scalar, point.Y * scalar);
    }

    public static Point2D operator *(double scalar, Point2D point)
    {
        return new Point2D(point.X * scalar, point.Y * scalar);
    }

    public static Point2D operator /(Point2D point, double scalar)
    {
        return new Point2D(point.X / scalar, point.Y / scalar);
    }

    public static explicit operator Vector2D(Point2D point)
    {
        return new Vector2D(point.X, point.Y);
    }

    public static explicit operator Point2D(Vector2D vector)
    {
        return new Point2D(vector.X, vector.Y);
    }

#if WPF
    public static implicit operator Point(Point2D point)
    {
        return new Point(point.X, point.Y);
    }

    public static implicit operator Point2D(Point point)
    {
        return new Point2D(point.X, point.Y);
    }
#endif

    #endregion
}