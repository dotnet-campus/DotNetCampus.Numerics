namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 2 维大小。
/// </summary>
/// <param name="Width">宽度。</param>
/// <param name="Height">高度。</param>
public readonly record struct Size2D(double Width, double Height)
{
    #region 运算符重载

    /// <summary>
    /// 将大小按指定倍数进行缩放。
    /// </summary>
    /// <param name="size"></param>
    /// <param name="scalar">缩放倍数。大于 1 为放大，小于 1 为缩小。</param>
    /// <returns></returns>
    public static Size2D operator *(Size2D size, double scalar)
    {
        return new Size2D(size.Width * scalar, size.Height * scalar);
    }

    /// <summary>
    /// 将大小按指定倍数进行反向缩放。
    /// </summary>
    /// <param name="size"></param>
    /// <param name="scalar">反向缩放倍数。大于 1 为缩小，小于 1 为放大。</param>
    /// <returns></returns>
    public static Size2D operator /(Size2D size, double scalar)
    {
        return new Size2D(size.Width / scalar, size.Height / scalar);
    }

    #endregion
}
