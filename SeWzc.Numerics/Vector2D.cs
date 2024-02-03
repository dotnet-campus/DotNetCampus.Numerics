using SeWzc.Numerics.Matrix;

namespace SeWzc.Numerics;

/// <summary>
/// 2 维向量。
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public readonly partial record struct Vector2D
{
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
}