namespace SeWzc.Numerics.Geometry;

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
}