namespace SeWzc.Numerics;

public readonly partial record struct Vector3D
{
    #region 构造函数

    public Vector3D(Vector2D vector2D, double Z = 0) : this(vector2D.X, vector2D.Y, Z)
    {
    }

    #endregion

    #region 成员方法

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

    #endregion
}