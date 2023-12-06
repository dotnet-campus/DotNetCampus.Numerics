namespace SeWzc.Numerics;

public readonly record struct Vector3D(double X, double Y, double Z) : IVector<Vector3D, double>
{
    public Vector3D(Vector2D vector2D, double Z = 0) : this(vector2D.X, vector2D.Y, Z)
    {
    }

    /// <inheritdoc />
    public static int Dimension => 3;

    /// <inheritdoc />
    public double this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        _ => throw new ArgumentOutOfRangeException(nameof(index), "3 维向量的索引必须是小于 3 的非负数。"),
    };

    /// <inheritdoc />
    public double SquaredLength => X * X + Y * Y + Z * Z;

    /// <inheritdoc />
    public double Length => Math.Sqrt(SquaredLength);

    /// <inheritdoc />
    public Vector3D Normalized => this / Length;

    /// <inheritdoc />
    public double Dot(Vector3D other)
    {
        return X * X + Y * Y + Z * Z;
    }

    /// <summary>
    /// 向量叉乘。
    /// </summary>
    /// <param name="other">另一个向量。</param>
    /// <returns></returns>
    public Vector3D Cross(Vector3D other)
    {
        return new Vector3D(
            Y * other.Z - Z * other.Y,
            Z * other.X - X * other.Z,
            X * other.Y - Y * other.X
        );
    }

    #region 运算符重载

    /// <inheritdoc />
    public static Vector3D operator +(Vector3D vector1, Vector3D vector2)
    {
        return new Vector3D(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z);
    }

    /// <inheritdoc />
    public static Vector3D operator -(Vector3D vector1, Vector3D vector2)
    {
        return new Vector3D(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z);
    }

    /// <inheritdoc />
    public static double operator *(Vector3D vector1, Vector3D vector2)
    {
        return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
    }

    /// <inheritdoc />
    public static Vector3D operator *(Vector3D vector, double scalar)
    {
        return new Vector3D(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
    }

    /// <inheritdoc />
    public static Vector3D operator *(double scalar, Vector3D vector)
    {
        return new Vector3D(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
    }

    /// <inheritdoc />
    public static Vector3D operator /(Vector3D vector, double scalar)
    {
        return new Vector3D(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);
    }

    /// <inheritdoc />
    public static Vector3D operator -(Vector3D vector)
    {
        return new Vector3D(-vector.X, -vector.Y, -vector.Z);
    }

    #endregion
}