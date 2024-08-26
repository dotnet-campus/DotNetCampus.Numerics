namespace DotNetCampus.Numerics;

/// <summary>
/// 角（大小）。
/// </summary>
public readonly record struct AngularMeasure : IComparable<AngularMeasure>
{
    #region 静态变量

    /// <summary>
    /// 零角。
    /// </summary>
    public static readonly AngularMeasure Zero = new(0);

    /// <summary>
    /// π/2 角。
    /// </summary>
    public static readonly AngularMeasure HalfPi = new(Math.PI / 2);

    /// <summary>
    /// π 角。
    /// </summary>
    public static readonly AngularMeasure Pi = new(Math.PI);

    /// <summary>
    /// 3π/2 角。
    /// </summary>
    public static readonly AngularMeasure OneAndHalfPi = new(Math.PI * 3 / 2);

    /// <summary>
    /// 2π 角。
    /// </summary>
    public static readonly AngularMeasure Tau = new(Math.Tau);

    /// <summary>
    /// 90° 角。
    /// </summary>
    public static readonly AngularMeasure Degree90 = FromDegree(90);

    /// <summary>
    /// 180° 角。
    /// </summary>
    public static readonly AngularMeasure Degree180 = FromDegree(180);

    /// <summary>
    /// 270° 角。
    /// </summary>
    public static readonly AngularMeasure Degree270 = FromDegree(270);

    /// <summary>
    /// 360° 角。
    /// </summary>
    public static readonly AngularMeasure Degree360 = FromDegree(360);

    #endregion

    #region 静态方法

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

    #endregion

    #region 属性

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
    /// 将角转换为 0 到 2π 之间的角。不包括 2π。
    /// </summary>
    public AngularMeasure Normalized => FromRadian(Radian >= 0 ? Radian % Math.Tau : Radian % Math.Tau + Math.Tau);

    #endregion

    #region 构造函数

    private AngularMeasure(double radian)
    {
        Radian = radian;
    }

    #endregion

    #region 成员方法

    /// <summary>
    /// 返回角的余弦值。
    /// </summary>
    /// <returns></returns>
    public double Cos()
    {
        return Math.Cos(Radian);
    }

    /// <summary>
    /// 返回角的正弦值。
    /// </summary>
    /// <returns></returns>
    public double Sin()
    {
        return Math.Sin(Radian);
    }

    /// <summary>
    /// 返回角的正切值。
    /// </summary>
    /// <returns></returns>
    public double Tan()
    {
        return Math.Tan(Radian);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Radian} rad";
    }

    /// <inheritdoc />
    public int CompareTo(AngularMeasure other)
    {
        return Radian.CompareTo(other.Radian);
    }

    #endregion

    #region 运算符重载

    #pragma warning disable CS1591

    public static AngularMeasure operator +(AngularMeasure measure1, AngularMeasure measure2)
    {
        return new AngularMeasure(measure1.Radian + measure2.Radian);
    }

    public static AngularMeasure operator -(AngularMeasure measure1, AngularMeasure measure2)
    {
        return new AngularMeasure(measure1.Radian - measure2.Radian);
    }

    public static AngularMeasure operator -(AngularMeasure measure)
    {
        return new AngularMeasure(-measure.Radian);
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

    public static double operator /(AngularMeasure measure1, AngularMeasure measure2)
    {
        return measure1.Radian / measure2.Radian;
    }

    public static bool operator <(AngularMeasure measure1, AngularMeasure measure2)
    {
        return measure1.Radian < measure2.Radian;
    }

    public static bool operator >(AngularMeasure measure1, AngularMeasure measure2)
    {
        return measure1.Radian > measure2.Radian;
    }

    public static bool operator <=(AngularMeasure measure1, AngularMeasure measure2)
    {
        return measure1.Radian <= measure2.Radian;
    }

    public static bool operator >=(AngularMeasure measure1, AngularMeasure measure2)
    {
        return measure1.Radian >= measure2.Radian;
    }

    #pragma warning restore CS1591

    #endregion
}
