using System.Numerics;

namespace SeWzc.Numerics;

#pragma warning disable CA1000 // 不要在泛型类型中声明静态成员。这里是为了定义接口的抽象静态成员。

/// <summary>
/// 向量接口。
/// </summary>
/// <typeparam name="TSelf">实现此接口的类型。</typeparam>
/// <typeparam name="TNum">向量元素的类型。</typeparam>
public interface IVector<TSelf, TNum> : IEqualityOperators<TSelf, TSelf, bool>
    where TSelf : unmanaged, IVector<TSelf, TNum>
    where TNum : unmanaged, INumber<TNum>
{
    #region 静态变量

    /// <summary>
    /// 向量维度。
    /// </summary>
    public static abstract int Dimension { get; }

    /// <summary>
    /// 零向量。
    /// </summary>
    public static abstract TSelf Zero { get; }

    #endregion

    #region 属性

    /// <summary>
    /// 模长的平方。
    /// </summary>
    public TNum LengthSquared { get; }

    /// <summary>
    /// 模长。
    /// </summary>
    public TNum Length { get; }

    /// <summary>
    /// 单位向量。如果模长为 0，则返回每个分量都为 <see cref="double.NaN" /> 的向量。
    /// </summary>
    public TSelf Normalized { get; }

    /// <summary>
    /// 获取向量的第 <paramref name="index" /> 个分量。
    /// </summary>
    /// <param name="index"></param>
    public TNum this[int index] { get; }

    #endregion

    #region 成员方法

    /// <summary>
    /// 向量点乘。
    /// </summary>
    /// <param name="other">另一个向量。</param>
    /// <returns></returns>
    public TNum Dot(TSelf other);

    #endregion

    #region 运算符重载

#pragma warning disable CS1591

    public static abstract TSelf operator +(TSelf vector1, TSelf vector2);

    public static abstract TSelf operator -(TSelf vector1, TSelf vector2);

    public static abstract TNum operator *(TSelf vector1, TSelf vector2);

    public static abstract TSelf operator *(TSelf vector, TNum scalar);

    public static abstract TSelf operator *(TNum scalar, TSelf vector);

    public static abstract TSelf operator /(TSelf vector, TNum scalar);

    public static abstract TSelf operator -(TSelf vector);

#pragma warning restore CS1591

    #endregion
}
