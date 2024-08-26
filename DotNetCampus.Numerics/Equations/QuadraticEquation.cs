namespace DotNetCampus.Numerics.Equations;

/// <summary>
/// 二元一次方程。A * x^2 + B * x + C = 0。
/// </summary>
public record QuadraticEquation(double A, double B, double C)
{
    #region 属性

    /// <summary>
    /// 判别式。
    /// </summary>
    public double Discriminant => B * B - 4 * A * C;

    /// <summary>
    /// 是否有根。
    /// </summary>
    public bool HasRoot => Discriminant >= 0;

    /// <summary>
    /// 第一个根。
    /// </summary>
    public double Root1
    {
        get
        {
            if (Discriminant < 0)
                throw new InvalidOperationException("方程无实数根。");

            return (-B + Discriminant.Sqrt()) / (2 * A);
        }
    }

    /// <summary>
    /// 第二个根。
    /// </summary>
    public double Root2
    {
        get
        {
            if (Discriminant < 0)
                throw new InvalidOperationException("方程无实数根。");

            return (-B - Discriminant.Sqrt()) / (2 * A);
        }
    }

    #endregion

    #region 成员方法

    /// <summary>
    /// 求值。
    /// </summary>
    /// <param name="x">参数值。</param>
    /// <returns>返回方程在 <paramref name="x" /> 处的值。</returns>
    public double Evaluate(double x)
    {
        return A * x * x + B * x + C;
    }

    #endregion
}
