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
    /// <param name="size">缩放前的大小。</param>
    /// <param name="scalar">缩放倍数。大于 1 为放大，小于 1 为缩小。</param>
    /// <returns>缩放后的大小。</returns>
    public static Size2D operator *(Size2D size, double scalar)
    {
        return new Size2D(size.Width * scalar, size.Height * scalar);
    }

    /// <summary>
    /// 将大小按指定倍数进行反向缩放。
    /// </summary>
    /// <param name="size">缩放前的大小。</param>
    /// <param name="scalar">反向缩放倍数。大于 1 为缩小，小于 1 为放大。</param>
    /// <returns>反向缩放后的大小。</returns>
    public static Size2D operator /(Size2D size, double scalar)
    {
        return new Size2D(size.Width / scalar, size.Height / scalar);
    }

    /// <summary>
    /// 计算两个大小的缩放比例。
    /// </summary>
    /// <param name="left">要缩放到的目标大小。</param>
    /// <param name="right">要进行缩放的原始大小。</param>
    /// <returns><paramref name="right" /> 缩放到 <paramref name="left" /> 所需的缩放比例。</returns>
    public static Scaling2D operator /(Size2D left, Size2D right)
    {
        return new Scaling2D(left.Width / right.Width, left.Height / right.Height);
    }

    /// <summary>
    /// 将大小按指定缩放比例进行缩放。
    /// </summary>
    /// <param name="size">缩放前的大小。</param>
    /// <param name="scaling">缩放比例。</param>
    /// <returns>缩放后的大小。</returns>
    public static Size2D operator *(Size2D size, Scaling2D scaling)
    {
        return new Size2D(size.Width * scaling.ScaleX, size.Height * scaling.ScaleY);
    }

    /// <summary>
    /// 将大小按指定的缩放比例进行反向缩放。
    /// </summary>
    /// <param name="size">缩放前的大小。</param>
    /// <param name="scaling">反向缩放比例。</param>
    /// <returns>反向缩放后的大小。</returns>
    public static Size2D operator /(Size2D size, Scaling2D scaling)
    {
        return new Size2D(size.Width / scaling.ScaleX, size.Height / scaling.ScaleY);
    }

    #endregion
}
