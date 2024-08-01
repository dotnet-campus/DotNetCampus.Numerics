namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 2 维边界框。
/// </summary>
public readonly record struct BoundingBox2D
{
    #region 静态变量

    /// <summary>
    /// 空的 2 维边界框。
    /// </summary>
    public static BoundingBox2D Empty { get; } = new();

    #endregion

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
        {
            throw new ArgumentException("The minimum value of the bounding box must be less than or equal to the maximum value.");
        }

        return new BoundingBox2D(minX, minY, maxX, maxY);
    }

    /// <summary>
    /// 通过左上角位置和大小创建 2 维边界框。
    /// </summary>
    /// <param name="location">左上角位置。</param>
    /// <param name="size">大小。</param>
    /// <returns>创建的 2 维边界框。</returns>
    public static BoundingBox2D CreateByLocationSize(Point2D location, Size2D size)
    {
        return Create(location.X, location.Y, location.X + size.Width, location.Y + size.Height);
    }

    /// <summary>
    /// 通过一个点创建 2 维边界框。
    /// </summary>
    /// <param name="point">点。</param>
    /// <returns>宽高为 0 但不为空的 2 维边界框。</returns>
    public static BoundingBox2D Create(Point2D point)
    {
        return new BoundingBox2D(point.X, point.Y, point.X, point.Y);
    }

    /// <summary>
    /// 通过两个点创建 2 维边界框。
    /// </summary>
    /// <param name="point1">点1。</param>
    /// <param name="point2">点2。</param>
    /// <returns>包含两个点的最小 2 维边界框。</returns>
    public static BoundingBox2D Create(Point2D point1, Point2D point2)
    {
        return new BoundingBox2D(
            Math.Min(point1.X, point2.X),
            Math.Min(point1.Y, point2.Y),
            Math.Max(point1.X, point2.X),
            Math.Max(point1.Y, point2.Y));
    }

    /// <summary>
    /// 通过点列表创建 2 维边界框。
    /// </summary>
    /// <param name="points">点列表。</param>
    /// <returns>包含所有点的最小 2 维边界框。如果没有坐标，则返回一个空的 2 维边界框。</returns>
    public static BoundingBox2D Create(IReadOnlyCollection<Point2D> points)
    {
        ArgumentNullException.ThrowIfNull(points);
        if (points.Count == 0)
        {
            return Empty;
        }

        var minX = double.MaxValue;
        var minY = double.MaxValue;
        var maxX = double.MinValue;
        var maxY = double.MinValue;
        foreach (var point in points)
        {
            minX = Math.Min(minX, point.X);
            maxX = Math.Max(maxX, point.X);
            minY = Math.Min(minY, point.Y);
            maxY = Math.Max(maxY, point.Y);
        }

        return new BoundingBox2D(minX, minY, maxX, maxY);
    }

    /// <summary>
    /// 尝试创建一个 2 维边界框。并且如果最小值大于最大值，则返回空的边界框。
    /// </summary>
    /// <param name="minX">最小 X 值。</param>
    /// <param name="minY">最小 Y 值。</param>
    /// <param name="maxX">最大 X 值。</param>
    /// <param name="maxY">最大 Y 值。</param>
    /// <returns>创建的 2 维边界框。如果最小值大于最大值，则返回空的边界框。</returns>
    public static BoundingBox2D TryCreate(double minX, double minY, double maxX, double maxY)
    {
        if (minX > maxX || minY > maxY)
        {
            return Empty;
        }

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
    /// 边界框是否为空。宽高为 0 的边界框不一定为空。
    /// </summary>
    public bool IsEmpty => !_isNotEmpty;

    /// <summary>
    /// 宽度。
    /// </summary>
    public double Width => MaxX - MinX;

    /// <summary>
    /// 高度。
    /// </summary>
    public double Height => MaxY - MinY;

    /// <summary>
    /// 边界框坐标值最小的点（MinX, MinY）。
    /// </summary>
    public Point2D MinPoint => new(MinX, MinY);

    /// <summary>
    /// 边界框坐标值最大的点（MaxX, MaxY）。
    /// </summary>
    public Point2D MaxPoint => new(MaxX, MaxY);

    /// <summary>
    /// 中心点。
    /// </summary>
    public Point2D Center => new((MinX + MaxX) / 2, (MinY + MaxY) / 2);

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
        {
            return other;
        }

        if (other.IsEmpty)
        {
            return this;
        }

        return Create(
            Math.Min(MinX, other.MinX),
            Math.Min(MinY, other.MinY),
            Math.Max(MaxX, other.MaxX),
            Math.Max(MaxY, other.MaxY));
    }

    /// <summary>
    /// 合并当前边界框与指定点，返回合并后的边界框。
    /// </summary>
    /// <param name="point">要合并的点。</param>
    /// <returns>包含当前边界框和点的最小边界框。</returns>
    public BoundingBox2D Union(Point2D point)
    {
        if (IsEmpty)
        {
            return Create(point);
        }

        return Create(
            Math.Min(MinX, point.X),
            Math.Min(MinY, point.Y),
            Math.Max(MaxX, point.X),
            Math.Max(MaxY, point.Y));
    }

    /// <summary>
    /// 判断当前边界框是否包含指定点。
    /// </summary>
    /// <remarks>
    /// 如果点正好在边界上，也算包含。
    /// </remarks>
    /// <param name="point">指定点。</param>
    /// <returns>如果包含则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
    public bool Contains(Point2D point)
    {
        return point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY;
    }

    /// <summary>
    /// 判断当前边界框是否包含另一个边界框。
    /// </summary>
    /// <param name="other">判断是否被包含的边界框。</param>
    /// <returns>如果包含则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
    public bool Contains(BoundingBox2D other)
    {
        return MinX <= other.MinX && MinY <= other.MinY && MaxX >= other.MaxX && MaxY >= other.MaxY;
    }

    /// <summary>
    /// 向外扩张边界框。如果参数为负数，则会向内收缩。
    /// </summary>
    /// <param name="horizontalAmount">左右方向的扩张量。</param>
    /// <param name="verticalAmount">上下方向的扩张量。</param>
    /// <returns>扩张后的边界框。如果边界框垂直或竖值方向收缩后为空，则返回空的边界框。</returns>
    public BoundingBox2D Inflate(double horizontalAmount, double verticalAmount)
    {
        var minX = MinX - horizontalAmount;
        var minY = MinY - verticalAmount;
        var maxX = MaxX + horizontalAmount;
        var maxY = MaxY + verticalAmount;

        return TryCreate(minX, minY, maxX, maxY);
    }

    /// <summary>
    /// 向外扩张边界框。如果参数为负数，则会向内收缩。
    /// </summary>
    /// <param name="amount">四个方向上的扩张量。</param>
    /// <returns>扩张后的边界框。如果边界框垂直或竖值方向收缩后为空，则返回空的边界框。</returns>
    public BoundingBox2D Inflate(double amount)
    {
        return Inflate(amount, amount);
    }

    /// <summary>
    /// 获取当前边界框与另一个边界框的交集。
    /// </summary>
    /// <param name="other">另一个边界框。</param>
    /// <returns>两个边界框的交集。如果两个边界框没有交集，则返回空的边界框。</returns>
    public BoundingBox2D Intersect(BoundingBox2D other)
    {
        if (IsEmpty || other.IsEmpty)
        {
            return Empty;
        }

        var minX = Math.Max(MinX, other.MinX);
        var minY = Math.Max(MinY, other.MinY);
        var maxX = Math.Min(MaxX, other.MaxX);
        var maxY = Math.Min(MaxY, other.MaxY);

        return TryCreate(minX, minY, maxX, maxY);
    }

    #endregion
}
