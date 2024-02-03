namespace SeWzc.Numerics;

/// <summary>
/// 2 维向量。
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public readonly partial record struct Vector2D
{
    #region 属性

    /// <summary>
    /// 向量在极坐标上的角。
    /// </summary>
    public AngularMeasure Angle => AngularMeasure.FromRadian(Math.Atan2(Y, X));

    /// <summary>
    /// 法向量。
    /// </summary>
    public Vector2D NormalVector => new(-Y, X);

    #endregion

    #region 成员方法

    /// <summary>
    /// 行列式。
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public double Det(Vector2D other)
    {
        return X * other.Y - Y * other.X;
    }

    #endregion
}