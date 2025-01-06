namespace DotNetCampus.Numerics;

/// <summary>
/// 2 维向量。
/// </summary>
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

    /// <summary>
    /// 获取从当前向量到另一个向量的旋转。
    /// </summary>
    /// <param name="other">目标向量。</param>
    /// <returns>从当前向量旋转到目标向量的角。结果的范围为 -π~π。</returns>
    public AngularMeasure AngleTo(Vector2D other)
    {
        return AngularMeasure.FromRadian(Math.Atan2(Det(other), Dot(other)));
    }

    /// <summary>
    /// 将向量旋转指定的角度。
    /// </summary>
    /// <param name="angle">旋转角度。</param>
    /// <returns>旋转后的向量。</returns>
    public Vector2D Rotate(AngularMeasure angle)
    {
        var cos = Math.Cos(angle.Radian);
        var sin = Math.Sin(angle.Radian);
        return new Vector2D(X * cos - Y * sin, X * sin + Y * cos);
    }

    #endregion
}
