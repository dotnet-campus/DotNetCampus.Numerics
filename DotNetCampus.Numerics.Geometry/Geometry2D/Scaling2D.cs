using System.Diagnostics.Contracts;

namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 2 维缩放。
/// </summary>
/// <param name="ScaleX">X 轴缩放比例。</param>
/// <param name="ScaleY">Y 轴缩放比例。</param>
public readonly record struct Scaling2D(double ScaleX, double ScaleY)
{
    #region 静态变量

    /// <summary>
    /// 单位缩放。
    /// </summary>
    public static Scaling2D Identity { get; } = new(1, 1);

    #endregion

    #region 静态方法

    /// <summary>
    /// 创建指定 X 轴和 Y 轴缩放比例的缩放。
    /// </summary>
    /// <param name="scaleX">X 轴缩放比例。</param>
    /// <param name="scaleY">Y 轴缩放比例。</param>
    /// <returns>缩放。</returns>
    public static Scaling2D Create(double scaleX, double scaleY)
    {
        return new Scaling2D(scaleX, scaleY);
    }

    /// <summary>
    /// 创建指定缩放比例的等比例缩放。
    /// </summary>
    /// <param name="scale">缩放比例。</param>
    /// <returns>缩放。</returns>
    public static Scaling2D Create(double scale)
    {
        return new Scaling2D(scale, scale);
    }

    #endregion

    #region 运算符重载

    /// <summary>
    /// 隐式将 double 转换为等比例缩放。
    /// </summary>
    /// <param name="scale">缩放比例。</param>
    /// <returns>缩放。</returns>
    public static implicit operator Scaling2D(double scale)
    {
        return Create(scale);
    }

    /// <summary>
    /// 将向量进行缩放。
    /// </summary>
    /// <param name="scaling"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector2D operator * (Scaling2D scaling, Vector2D vector)
    {
        return new Vector2D(vector.X * scaling.ScaleX, vector.Y * scaling.ScaleY);
    }

    /// <summary>
    /// 将向量进行缩放。
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="scaling"></param>
    /// <returns></returns>
    public static Vector2D operator *(Vector2D vector, Scaling2D scaling)
    {
        return scaling * vector;
    }

    #endregion
}
