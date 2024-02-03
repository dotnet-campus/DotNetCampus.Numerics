using System.Numerics;

namespace SeWzc.Numerics;

public interface IVector<TSelf, TNum>
    where TSelf : unmanaged, IVector<TSelf, TNum>
    where TNum : unmanaged, INumber<TNum>
{
    #region 静态变量

    /// <summary>
    /// 向量维度。
    /// </summary>
    public static abstract int Dimension { get; }

    #endregion

    #region 属性

    /// <summary>
    /// 模长的平方。
    /// </summary>
    public TNum SquaredLength { get; }

    /// <summary>
    /// 模长。
    /// </summary>
    public TNum Length { get; }

    /// <summary>
    /// 单位向量。
    /// </summary>
    public TSelf Normalized { get; }

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

    public static abstract TSelf operator +(TSelf vector1, TSelf vector2);

    public static abstract TSelf operator -(TSelf vector1, TSelf vector2);

    public static abstract TNum operator *(TSelf vector1, TSelf vector2);

    public static abstract TSelf operator *(TSelf vector, TNum scalar);

    public static abstract TSelf operator *(TNum scalar, TSelf vector);

    public static abstract TSelf operator /(TSelf vector, TNum scalar);

    public static abstract TSelf operator -(TSelf vector);

    #endregion
}