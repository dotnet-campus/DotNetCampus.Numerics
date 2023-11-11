namespace SeWzc.Numerics;

/// <summary>
/// 2 维向量。
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public readonly record struct Vector2D(double X, double Y)
{
    /// <summary>
    /// 模长的平方。
    /// </summary>
    public double SquaredLength => X * X + Y * Y;

    /// <summary>
    /// 模长。
    /// </summary>
    public double Length => Math.Sqrt(SquaredLength);

    /// <summary>
    /// 单位向量。
    /// </summary>
    public Vector2D Normalized => this / Length;

    /// <summary>
    /// 向量在极坐标上的角。
    /// </summary>
    public AngularMeasure Angle => AngularMeasure.FromRadian(Math.Atan2(Y, X));

    #region 向量特殊乘法

    /// <summary>
    /// 行列式。
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public double Det(Vector2D other)
    {
        return X * other.Y - Y * other.X;
    }

    /// <summary>
    /// 投影到另一个向量上的投影位置。
    /// </summary>
    /// <remarks>
    /// 投影位置满足：投影值乘以 <paramref name="other" /> 的单位向量等于该向量在 <paramref name="other" /> 上的投影向量。
    /// </remarks>
    /// <param name="other"></param>
    /// <returns>投影位置。</returns>
    public double Projection(Vector2D other)
    {
        return this * other.Normalized;
    }

    #endregion

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