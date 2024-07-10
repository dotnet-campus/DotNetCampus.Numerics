namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 2 维边界框。
/// </summary>
public readonly record struct BoundingBox2D
{
    #region 静态方法

    /// <summary>
    /// 创建一个 2 维边界框。
    /// </summary>
    /// <param name="minX">最小 X 值。</param>
    /// <param name="minY">最小 Y 值。</param>
    /// <param name="maxX">最大 X 值。</param>
    /// <param name="maxY">最大 Y 值。</param>
    /// <returns>创建的 2 维边界框。</returns>
    public static BoundingBox2D Create(double minX, double minY, double maxX, double maxY)
    {
        if (minX > maxX || minY > maxY)
            throw new ArgumentException("The minimum value of the bounding box must be less than or equal to the maximum value.");

        return new BoundingBox2D(minX, minY, maxX, maxY);
    }

    #endregion

    #region 私有字段

    /// <summary>
    /// 边界框是否非空。没有调用构造函数时边界框默认为空。
    /// </summary>
    private readonly bool _isNotEmpty;

    #endregion

    #region 属性

    /// <summary>
    /// 最小 X 值。
    /// </summary>
    public double MinX { get; }

    /// <summary>
    /// 最小 Y 值。
    /// </summary>
    public double MinY { get; }

    /// <summary>
    /// 最大 X 值。
    /// </summary>
    public double MaxX { get; }

    /// <summary>
    /// 最大 Y 值。
    /// </summary>
    public double MaxY { get; }

    /// <summary>
    /// 边界框是否为空。
    /// </summary>
    public bool IsEmpty => !_isNotEmpty;

    #endregion

    #region 构造函数

    /// <summary>
    /// 创建一个空的 2 维边界框。
    /// </summary>
    public BoundingBox2D()
    {
        MinX = 0;
        MinY = 0;
        MaxX = 0;
        MaxY = 0;

        _isNotEmpty = false;
    }

    private BoundingBox2D(double minX, double minY, double maxX, double maxY)
    {
        MinX = minX;
        MinY = minY;
        MaxX = maxX;
        MaxY = maxY;

        _isNotEmpty = true;
    }

    #endregion

    #region 成员方法

    /// <summary>
    /// 合并当前边界框与另一个边界框，返回合并后的边界框。
    /// </summary>
    /// <param name="other">另一个边界框。</param>
    /// <returns>合并后的边界框。</returns>
    public BoundingBox2D Union(BoundingBox2D other)
    {
        if (IsEmpty)
            return other;
        if (other.IsEmpty)
            return this;

        return Create(
            Math.Min(MinX, other.MinX),
            Math.Min(MinY, other.MinY),
            Math.Max(MaxX, other.MaxX),
            Math.Max(MaxY, other.MaxY));
    }

    #endregion
}