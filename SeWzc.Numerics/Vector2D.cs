namespace SeWzc.Numerics;

/// <summary>
/// 2 维向量。
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public readonly record struct Vector2D(double X, double Y) : IVector<Vector2D, double>
{
    /// <inheritdoc />
    public static int Dimension => 2;

    /// <inheritdoc />
    public double this[int index] => index switch
    {
        0 => X,
        1 => Y,
        _ => throw new ArgumentOutOfRangeException(nameof(index), "2 维向量的索引必须是小于 2 的非负数。"),
    };

    /// <inheritdoc />
    public double SquaredLength => X * X + Y * Y;

    /// <inheritdoc />
    public double Length => Math.Sqrt(SquaredLength);

    /// <inheritdoc />
    public Vector2D Normalized => this / Length;

    /// <summary>
    /// 向量在极坐标上的角。
    /// </summary>
    public AngularMeasure Angle => AngularMeasure.FromRadian(Math.Atan2(Y, X));

    /// <summary>
    /// 法向量。
    /// </summary>
    public Vector2D NormalVector => new(-Y, X);

    /// <summary>
    /// 行列式。
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public double Det(Vector2D other)
    {
        return X * other.Y - Y * other.X;
    }

    /// <inheritdoc />
    public double Dot(Vector2D other)
    {
        return this * other;
    }

    #region 运算符重载

    public static Vector2D operator +(Vector2D vector1, Vector2D vector2)
    {
        return new Vector2D(vector1.X + vector2.X, vector1.Y + vector2.Y);
    }

    public static Vector2D operator -(Vector2D vector1, Vector2D vector2)
    {
        return new Vector2D(vector1.X - vector2.X, vector1.Y - vector2.Y);
    }

    public static double operator *(Vector2D vector1, Vector2D vector2)
    {
        return vector1.X * vector2.X + vector1.Y * vector2.Y;
    }

    public static Vector2D operator *(Vector2D vector, double scalar)
    {
        return new Vector2D(vector.X * scalar, vector.Y * scalar);
    }

    public static Vector2D operator *(double scalar, Vector2D vector)
    {
        return new Vector2D(vector.X * scalar, vector.Y * scalar);
    }

    public static Vector2D operator /(Vector2D vector, double scalar)
    {
        return new Vector2D(vector.X / scalar, vector.Y / scalar);
    }

    public static Vector2D operator -(Vector2D vector)
    {
        return new Vector2D(-vector.X, -vector.Y);
    }

#if WPF
    public static implicit operator Vector(Vector2D vector)
    {
        return new Vector(vector.X, vector.Y);
    }

    public static implicit operator Vector2D(Vector vector)
    {
        return new Vector2D(vector.X, vector.Y);
    }
#endif

    #endregion
}