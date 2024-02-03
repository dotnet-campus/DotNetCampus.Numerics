namespace SeWzc.Numerics;

public readonly partial record struct Vector3D
{
    public Vector3D(Vector2D vector2D, double Z = 0) : this(vector2D.X, vector2D.Y, Z)
    {
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
}