namespace SeWzc.Numerics;

/// <summary>
/// 角（大小）。
/// </summary>
public readonly record struct AngularMeasure
{
    private AngularMeasure(double radian)
    {
        Radian = radian;
    }

    /// <summary>
    /// 弧度。
    /// </summary>
    public double Radian { get; }

    /// <summary>
    /// 角度。
    /// </summary>
    public double Degree => Radian * 180 / Math.PI;

    /// <summary>
    /// 角对应的单位方向向量。
    /// </summary>
    public Vector2D UnitVector => new(Math.Cos(Radian), Math.Sin(Radian));

    /// <summary>
    /// 从角度创建角。
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    public static AngularMeasure FromDegree(double degree)
    {
        return new AngularMeasure(degree * Math.PI / 180);
    }

    /// <summary>
    /// 从弧度创建角。
    /// </summary>
    /// <param name="radian"></param>
    /// <returns></returns>
    public static AngularMeasure FromRadian(double radian)
    {
        return new AngularMeasure(radian);
    }

    #region 运算符重载

    public static AngularMeasure operator +(AngularMeasure measure1, AngularMeasure measure2)
    {
        return new AngularMeasure(measure1.Radian + measure2.Radian);
    }

    public static AngularMeasure operator -(AngularMeasure measure1, AngularMeasure measure2)
    {
        return new AngularMeasure(measure1.Radian - measure2.Radian);
    }

    public static AngularMeasure operator *(AngularMeasure measure, double scalar)
    {
        return new AngularMeasure(measure.Radian * scalar);
    }

    public static AngularMeasure operator *(double scalar, AngularMeasure measure)
    {
        return new AngularMeasure(measure.Radian * scalar);
    }

    public static AngularMeasure operator /(AngularMeasure measure, double scalar)
    {
        return new AngularMeasure(measure.Radian / scalar);
    }

    #endregion
}