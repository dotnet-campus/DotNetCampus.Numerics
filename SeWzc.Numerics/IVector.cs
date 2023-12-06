using System.Numerics;

namespace SeWzc.Numerics;

public interface IVector<TSelf, TNum>
    where TSelf : unmanaged, IVector<TSelf, TNum>
    where TNum : unmanaged, INumber<TNum>
{
    /// <summary>
    /// 向量维度。
    /// </summary>
    public static abstract int Dimension { get; }

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

    /// <summary>
    /// 获取向量的第 <paramref name="index" /> 个分量。
    /// </summary>
    /// <param name="index"></param>
    public TNum this[int index] { get; }

    // /// <summary>
    // /// 将向量转换为 <see cref="ReadOnlySpan{TNum}" />。
    // /// </summary>
    // /// <returns></returns>
    // public ReadOnlySpan<TNum> AsReadonlySpan();

    /// <summary>
    /// 向量点乘。
    /// </summary>
    /// <param name="other">另一个向量。</param>
    /// <returns></returns>
    public TNum Dot(TSelf other);

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

public static class VectorExtensions
{
    /// <summary>
    /// 投影到另一个向量上的投影位置。
    /// </summary>
    /// <remarks>
    /// 投影位置满足：投影值乘以 <paramref name="other" /> 的单位向量等于该向量在 <paramref name="other" /> 上的投影向量。
    /// </remarks>
    /// <param name="vector">将指定的向量投影到另一个向量上。</param>
    /// <param name="other">要投影到的向量。</param>
    /// <returns>投影位置。</returns>
    public static double GetProjectionOn<TVector>(this TVector vector, TVector other)
        where TVector : unmanaged, IVector<TVector, double>
    {
        return vector * other.Normalized;
    }
}