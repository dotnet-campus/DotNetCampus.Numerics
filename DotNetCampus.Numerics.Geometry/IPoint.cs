using System.Numerics;

namespace DotNetCampus.Numerics.Geometry;

#pragma warning disable CA1000 // 不要在泛型类型中声明静态成员。这里是为了定义接口的抽象静态成员。

/// <summary>
/// 点的接口。
/// </summary>
/// <typeparam name="TSelf">自身类型。</typeparam>
/// <typeparam name="TVector">点对应的向量类型。</typeparam>
/// <typeparam name="TNum">点的元素类型。</typeparam>
public interface IPoint<TSelf, TVector, TNum>
    where TSelf : unmanaged, IPoint<TSelf, TVector, TNum>
    where TVector : unmanaged, IVector<TVector, TNum>
    where TNum : unmanaged
#if NET8_0_OR_GREATER
    , IFloatingPoint<TNum>
#endif
{
    /// <summary>
    /// 获取两个点的中点。
    /// </summary>
    /// <param name="point1">第一个点。</param>
    /// <param name="point2">第二个点。</param>
    /// <returns>两个点的中点。</returns>
    static abstract TSelf Middle(TSelf point1, TSelf point2);

    /// <summary>
    /// 获取点列表的中心点。
    /// </summary>
    /// <param name="points">点列表。</param>
    /// <returns>点列表的中心点。</returns>
    /// <exception cref="ArgumentException">点列表不能为空。</exception>
    static abstract TSelf Middle(IReadOnlyList<TSelf> points);

    /// <summary>
    /// 获取两个点的加权和。
    /// </summary>
    /// <param name="weight1">第一个点的权重。</param>
    /// <param name="point1">第一个点。</param>
    /// <param name="weight2">第二个点的权重。</param>
    /// <param name="point2">第二个点。</param>
    /// <returns>两个点的加权中点。</returns>
    static abstract TSelf WeightedSum(TNum weight1, TSelf point1, TNum weight2, TSelf point2);

    /// <summary>
    /// 获取三个点的加权和。
    /// </summary>
    /// <param name="weight1">第一个点的权重。</param>
    /// <param name="point1">第一个点。</param>
    /// <param name="weight2">第二个点的权重。</param>
    /// <param name="point2">第二个点。</param>
    /// <param name="weight3">第三个点的权重。</param>
    /// <param name="point3">第三个点。</param>
    /// <returns>三个点的加权中点。</returns>
    static abstract TSelf WeightedSum(TNum weight1, TSelf point1, TNum weight2, TSelf point2, TNum weight3, TSelf point3);

    /// <summary>
    /// 获取四个点的加权和。
    /// </summary>
    /// <param name="weight1">第一个点的权重。</param>
    /// <param name="point1">第一个点。</param>
    /// <param name="weight2">第二个点的权重。</param>
    /// <param name="point2">第二个点。</param>
    /// <param name="weight3">第三个点的权重。</param>
    /// <param name="point3">第三个点。</param>
    /// <param name="weight4">第四个点的权重。</param>
    /// <param name="point4">第四个点。</param>
    /// <returns>四个点的加权中点。</returns>
    static abstract TSelf WeightedSum(TNum weight1, TSelf point1, TNum weight2, TSelf point2, TNum weight3, TSelf point3, TNum weight4, TSelf point4);

    /// <summary>
    /// 转换成向量。相当于从原点到该点的向量。
    /// </summary>
    /// <returns>从原点到该点的向量。</returns>
    TVector ToVector();

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
