using System.Numerics;

namespace SeWzc.Numerics.Geometry;

public interface IPoint<TSelf, TVector, TNum>
    where TSelf : unmanaged, IPoint<TSelf, TVector, TNum>
    where TVector : unmanaged, IVector<TVector, TNum>
    where TNum : unmanaged, INumber<TNum>
{
    #region 运算符重载

    /// <summary>
    /// 一个点加上一个向量得到新的点。
    /// </summary>
    /// <param name="point">要加上向量的点。</param>
    /// <param name="vector">要加上的向量。</param>
    /// <returns>相加后得到的新点。</returns>
    public static abstract TSelf operator +(TSelf point, TVector vector);

    /// <summary>
    /// 一个点加上一个向量得到新的点。
    /// </summary>
    /// <param name="vector">要加上的向量。</param>
    /// <param name="point">要加上向量的点。</param>
    /// <returns>相加后得到的新点。</returns>
    public static abstract TSelf operator +(TVector vector, TSelf point);

    /// <summary>
    /// 两个点之间的差向量。
    /// </summary>
    /// <param name="point1">向量终点。</param>
    /// <param name="point2">向量起点。</param>
    /// <returns>两个点之间的差向量。</returns>
    public static abstract TVector operator -(TSelf point1, TSelf point2);

    /// <summary>
    /// 一个点减去一个向量得到新的点。
    /// </summary>
    /// <param name="point">要减去向量的点。</param>
    /// <param name="vector">要减去的向量。</param>
    /// <returns>相减后得到的新点。</returns>
    public static abstract TSelf operator -(TSelf point, TVector vector);

    /// <summary>
    /// 一个点乘以标量得到新的点。
    /// </summary>
    /// <param name="point">要乘以标量的点。</param>
    /// <param name="scalar">要乘以的标量。</param>
    /// <returns>相乘后得到的新点。</returns>
    public static abstract TSelf operator *(TSelf point, TNum scalar);

    /// <summary>
    /// 标量乘以一个点得到新的点。
    /// </summary>
    /// <param name="scalar">要乘以的标量。</param>
    /// <param name="point">要乘以标量的点。</param>
    /// <returns>相乘后得到的新点。</returns>
    public static abstract TSelf operator *(TNum scalar, TSelf point);

    /// <summary>
    /// 一个点除以标量得到新的点。
    /// </summary>
    /// <param name="point">要除以标量的点。</param>
    /// <param name="scalar">要除以的标量。</param>
    /// <returns>相除后得到的新点。</returns>
    public static abstract TSelf operator /(TSelf point, double scalar);

    /// <summary>
    /// 将点转换为向量。
    /// </summary>
    /// <param name="point">要转换的点。</param>
    /// <returns>转换后得到的向量。</returns>
    public static abstract explicit operator TVector(TSelf point);

    /// <summary>
    /// 将向量转换为点。
    /// </summary>
    /// <param name="vector">要转换的向量。</param>
    /// <returns>转换后得到的点。</returns>
    public static abstract explicit operator TSelf(TVector vector);

    #endregion
}