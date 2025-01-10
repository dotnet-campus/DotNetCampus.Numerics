using System.Runtime.Intrinsics.Arm;

namespace DotNetCampus.Numerics;

/// <summary>
/// 浮点数扩展方法。
/// </summary>
public static class FloatingPointNumberExtensions
{
    #region 静态方法

    /// <summary>
    /// 计算平方根。
    /// </summary>
    /// <param name="value">要计算平方根的浮点数。</param>
    /// <returns>浮点数的平方根。</returns>
    public static double Sqrt(this double value)
    {
        return Math.Sqrt(value);
    }

    /// <summary>
    /// 计算平方根。
    /// </summary>
    /// <param name="value">要计算平方根的浮点数。</param>
    /// <returns>浮点数的平方根。</returns>
    public static float Sqrt(this float value)
    {
        return MathF.Sqrt(value);
    }

    /// <summary>
    /// 计算绝对值。
    /// </summary>
    /// <param name="value">要计算绝对值的浮点数。</param>
    /// <returns>浮点数的绝对值。</returns>
    public static double Abs(this double value)
    {
        return Math.Abs(value);
    }

    /// <summary>
    /// 计算绝对值。
    /// </summary>
    /// <param name="value">要计算绝对值的浮点数。</param>
    /// <returns>浮点数的绝对值。</returns>
    public static float Abs(this float value)
    {
        return MathF.Abs(value);
    }

    #endregion

    #region 舍入

    /// <summary>
    /// 对浮点数进行舍入。
    /// </summary>
    /// <remarks>
    /// 这个舍入方法使用的是计算机常用的四舍六入五成双的舍入规则。
    /// </remarks>
    /// <param name="value">要进行舍入的浮点数。</param>
    /// <returns>最接近的整数。</returns>
    public static int Round(this double value)
    {
        return (int)Math.Round(value);
    }

    /// <summary>
    /// 对浮点数进行舍入。
    /// </summary>
    /// <remarks>
    /// 这个舍入方法使用的是计算机常用的四舍六入五成双的舍入规则。
    /// </remarks>
    /// <param name="value">要进行舍入的浮点数。</param>
    /// <returns>最接近的整数。</returns>
    public static int Round(this float value)
    {
        return (int)MathF.Round(value);
    }

    /// <summary>
    /// 对浮点数进行四舍五入。
    /// </summary>
    /// <param name="value">要进行四舍五入的浮点数。</param>
    /// <returns>四舍五入到整数。</returns>
    public static int RoundingHalfAwayFromZero(this double value)
    {

        return (int)Math.Round(value, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 对浮点数进行四舍五入。
    /// </summary>
    /// <param name="value">要进行四舍五入的浮点数。</param>
    /// <returns>四舍五入到整数。</returns>
    public static int RoundingHalfAwayFromZero(this float value)
    {
        return (int)MathF.Round(value, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 对浮点数进行五舍六入。
    /// </summary>
    /// <param name="value">要进行五舍六入的浮点数。</param>
    /// <returns>五舍六入到整数。</returns>
    public static int RoundingHalfToZero(this double value)
    {
        return (int)Math.Truncate(value - Math.CopySign(0.5, value));
    }

    /// <summary>
    /// 对浮点数进行五舍六入。
    /// </summary>
    /// <param name="value">要进行五舍六入的浮点数。</param>
    /// <returns>五舍六入到整数。</returns>
    public static int RoundingHalfToZero(this float value)
    {
        return (int)MathF.Truncate(value - MathF.CopySign(0.5f, value));
    }

    /// <summary>
    /// 对浮点数进行向下取整。
    /// </summary>
    /// <param name="value">要进行向下取整的浮点数。</param>
    /// <returns>小于或等于指定浮点数的最大整数。</returns>
    public static int Floor(this double value)
    {
        return (int)Math.Floor(value);
    }

    /// <summary>
    /// 对浮点数进行向下取整。
    /// </summary>
    /// <param name="value">要进行向下取整的浮点数。</param>
    /// <returns>小于或等于指定浮点数的最大整数。</returns>
    public static int Floor(this float value)
    {
        return (int)MathF.Floor(value);
    }

    /// <summary>
    /// 对浮点数进行向上取整。
    /// </summary>
    /// <param name="value">要进行向上取整的浮点数。</param>
    /// <returns>大于或等于指定浮点数的最小整数。</returns>
    public static int Ceiling(this double value)
    {
        return (int)Math.Ceiling(value);
    }

    /// <summary>
    /// 对浮点数进行向上取整。
    /// </summary>
    /// <param name="value">要进行向上取整的浮点数。</param>
    /// <returns>大于或等于指定浮点数的最小整数。</returns>
    public static int Ceiling(this float value)
    {
        return (int)MathF.Ceiling(value);
    }

    #endregion

    #region 对数

    /// <summary>
    /// 计算自然对数。
    /// </summary>
    /// <param name="value">要计算自然对数的浮点数。</param>
    /// <returns>浮点数的自然对数。</returns>
    public static double Ln(this double value)
    {
        return Math.Log(value);
    }

    /// <summary>
    /// 计算自然对数。
    /// </summary>
    /// <param name="value">要计算自然对数的浮点数。</param>
    /// <returns>浮点数的自然对数。</returns>
    public static float Ln(this float value)
    {
        return MathF.Log(value);
    }

    /// <summary>
    /// 计算指定底数的对数。
    /// </summary>
    /// <param name="value">要计算对数的浮点数。</param>
    /// <param name="newBase">对数的底数。</param>
    /// <returns>浮点数的指定底数的对数。</returns>
    public static double Log(this double value, double newBase)
    {
        return Math.Log(value, newBase);
    }

    /// <summary>
    /// 计算指定底数的对数。
    /// </summary>
    /// <param name="value">要计算对数的浮点数。</param>
    /// <param name="newBase">对数的底数。</param>
    /// <returns>浮点数的指定底数的对数。</returns>
    public static float Log(this float value, float newBase)
    {
        return MathF.Log(value, newBase);
    }

    /// <summary>
    /// 计算以 10 为底的对数。
    /// </summary>
    /// <param name="value">要计算对数的浮点数。</param>
    /// <returns>浮点数的以 10 为底的对数。</returns>
    public static double Log10(this double value)
    {
        return Math.Log10(value);
    }

    /// <summary>
    /// 计算以 10 为底的对数。
    /// </summary>
    /// <param name="value">要计算对数的浮点数。</param>
    /// <returns>浮点数的以 10 为底的对数。</returns>
    public static float Log10(this float value)
    {
        return MathF.Log10(value);
    }

    #endregion

    #region 指数

    /// <summary>
    /// 计算平方。
    /// </summary>
    /// <param name="value">要计算平方的浮点数。</param>
    /// <returns>浮点数的平方。</returns>
    public static double Square(this double value)
    {
        return value * value;
    }

    /// <summary>
    /// 计算平方。
    /// </summary>
    /// <param name="value">要计算平方的浮点数。</param>
    /// <returns>浮点数的平方。</returns>
    public static float Square(this float value)
    {
        return value * value;
    }

    /// <summary>
    /// 计算立方。
    /// </summary>
    /// <param name="value">要计算立方的浮点数。</param>
    /// <returns>浮点数的立方。</returns>
    public static double Cube(this double value)
    {
        return value * value * value;
    }

    /// <summary>
    /// 计算立方。
    /// </summary>
    /// <param name="value">要计算立方的浮点数。</param>
    /// <returns>浮点数的立方。</returns>
    public static float Cube(this float value)
    {
        return value * value * value;
    }

    /// <summary>
    /// 计算指定次幂。
    /// </summary>
    /// <param name="value">要计算幂的浮点数。</param>
    /// <param name="exponent">幂的指数。</param>
    /// <returns>浮点数的指定次幂。</returns>
    public static double Pow(this double value, double exponent)
    {
        return Math.Pow(value, exponent);
    }

    /// <summary>
    /// 计算指定次幂。
    /// </summary>
    /// <param name="value">要计算幂的浮点数。</param>
    /// <param name="exponent">幂的指数。</param>
    /// <returns>浮点数的指定次幂。</returns>
    public static float Pow(this float value, float exponent)
    {
        return MathF.Pow(value, exponent);
    }

    /// <summary>
    /// 计算指定次幂。
    /// </summary>
    /// <param name="value">要计算幂的浮点数。</param>
    /// <param name="exponent">幂的指数。</param>
    /// <returns>浮点数的指定次幂。</returns>
    public static double Pow(this float value, double exponent)
    {
        return Math.Pow(value, exponent);
    }

    #endregion
}
