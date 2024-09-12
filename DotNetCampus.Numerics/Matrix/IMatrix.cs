using System.Numerics;

namespace DotNetCampus.Numerics.Matrix;

#pragma warning disable CA1000 // 不要在泛型类型中声明静态成员。这里是为了定义接口的抽象静态成员。

/// <summary>
/// 矩阵的接口。
/// </summary>
/// <typeparam name="TRow">每行的向量。</typeparam>
/// <typeparam name="TColumn">每一列的向量。</typeparam>
/// <typeparam name="TNum">矩阵元素的类型。</typeparam>
public interface IMatrix<TRow, TColumn, TNum>
    where TRow : unmanaged, IVector<TRow, TNum>
    where TColumn : unmanaged, IVector<TColumn, TNum>
    where TNum : unmanaged
#if NET8_0_OR_GREATER
    , IFloatingPoint<TNum>
#endif
{
    #region 静态变量

    /// <summary>
    /// 矩阵行数。
    /// </summary>
    static virtual int RowCount => TColumn.Dimension;

    /// <summary>
    /// 矩阵列数。
    /// </summary>
    static virtual int ColumnCount => TRow.Dimension;

    #endregion

    #region 属性

    /// <summary>
    /// 获取矩阵的第 <paramref name="row" /> 行、第 <paramref name="column" /> 列的元素。
    /// </summary>
    /// <param name="row">行的索引。基于 0 的索引。</param>
    /// <param name="column">列的索引。基于 0 的索引。</param>
    TNum this[int row, int column] { get; }

    /// <summary>
    /// 获取转置矩阵。
    /// </summary>
    /// <returns></returns>
    IMatrix<TColumn, TRow, TNum> Transpose { get; }

    /// <summary>
    /// Frobenius范数。等于矩阵元素的平方和的平方根。
    /// </summary>
    TNum FrobeniusNorm { get; }

    #endregion

    #region 成员方法

    /// <summary>
    /// 获取矩阵的第 <paramref name="row" /> 行。
    /// </summary>
    /// <param name="row">行的索引。基于 0 的索引。</param>
    /// <returns></returns>
    TRow GetRow(int row);

    /// <summary>
    /// 获取矩阵的第 <paramref name="column" /> 列。
    /// </summary>
    /// <param name="column">列的索引。基于 0 的索引。</param>
    /// <returns></returns>
    TColumn GetColumn(int column);

    #endregion
}

/// <summary>
/// 矩阵的接口。
/// </summary>
/// <typeparam name="TSelf">实现此接口的类型。</typeparam>
/// <typeparam name="TRow">每行的向量。</typeparam>
/// <typeparam name="TColumn">每一列的向量。</typeparam>
/// <typeparam name="TNum">矩阵元素的类型。</typeparam>
public interface IMatrix<TSelf, TRow, TColumn, TNum> : IMatrix<TRow, TColumn, TNum>
#if NET8_0_OR_GREATER
    , IEqualityOperators<TSelf, TSelf, bool>
#endif
    where TSelf : IMatrix<TSelf, TRow, TColumn, TNum>
    where TRow : unmanaged, IVector<TRow, TNum>
    where TColumn : unmanaged, IVector<TColumn, TNum>
    where TNum : unmanaged
#if NET8_0_OR_GREATER
    , IFloatingPoint<TNum>
#endif
{
    #region 静态变量

    /// <summary>
    /// 零矩阵。
    /// </summary>
    public static abstract TSelf Zero { get; }

    #endregion

#pragma warning disable CS1591

    public static abstract TSelf operator +(TSelf matrix1, TSelf matrix2);

    public static abstract TSelf operator -(TSelf matrix1, TSelf matrix2);

    public static abstract TSelf operator *(TSelf matrix, TNum scalar);

    public static abstract TColumn operator *(TSelf matrix, TRow vector);

    public static abstract TRow operator *(TColumn vector, TSelf matrix);

    public static abstract TSelf operator *(TNum scalar, TSelf matrix);

    public static abstract TSelf operator /(TSelf matrix, TNum scalar);

    public static abstract TSelf operator -(TSelf matrix);

#pragma warning restore CS1591
}

/// <summary>
/// 可转置的矩阵接口。
/// </summary>
/// <typeparam name="TSelf">实现此接口的类型。</typeparam>
/// <typeparam name="TRow">每行的向量。</typeparam>
/// <typeparam name="TColumn">每一列的向量。</typeparam>
/// <typeparam name="TNum">矩阵元素的类型。</typeparam>
/// <typeparam name="TTranspose">转置矩阵的类型。</typeparam>
public interface IMatrix<TSelf, TRow, TColumn, TNum, TTranspose> : IMatrix<TSelf, TRow, TColumn, TNum>
    where TSelf : IMatrix<TSelf, TRow, TColumn, TNum, TTranspose>
    where TRow : unmanaged, IVector<TRow, TNum>
    where TColumn : unmanaged, IVector<TColumn, TNum>
    where TNum : unmanaged
#if NET8_0_OR_GREATER
    , IFloatingPoint<TNum>
#endif
    where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum>
{
    #region 属性

    /// <summary>
    /// 获取转置矩阵。
    /// </summary>
    /// <returns></returns>
    public new TTranspose Transpose { get; }

    IMatrix<TColumn, TRow, TNum> IMatrix<TRow, TColumn, TNum>.Transpose => Transpose;

    #endregion
}

/// <summary>
/// 方阵的接口。
/// </summary>
/// <typeparam name="TSelf">实现此接口的类型。</typeparam>
/// <typeparam name="TVector">向量的类型。</typeparam>
/// <typeparam name="TNum">矩阵元素的类型。</typeparam>
public interface ISquareMatrix<TSelf, TVector, TNum> : IMatrix<TSelf, TVector, TVector, TNum, TSelf>
    where TSelf : ISquareMatrix<TSelf, TVector, TNum>
    where TVector : unmanaged, IVector<TVector, TNum>
    where TNum : unmanaged
#if NET8_0_OR_GREATER
    , IFloatingPoint<TNum>
#endif
{
    #region 静态变量

    /// <summary>
    /// 单位矩阵。
    /// </summary>
    public static abstract TSelf Identity { get; }

    #endregion

    #region 属性

    /// <summary>
    /// 行列式。
    /// </summary>
    public TNum Determinant { get; }

    /// <summary>
    /// 矩阵是否可逆。
    /// </summary>
    public bool Invertible { get; }

    /// <summary>
    /// 矩阵的逆。如果矩阵不可逆，则返回 null。
    /// </summary>
    public TSelf? Inverse { get; }

    #endregion

    #region 成员方法

    /// <summary>
    /// 获取矩阵的逆。
    /// </summary>
    /// <returns></returns>
    /// <exception cref="MatrixNonInvertibleException">该矩阵不可逆。</exception>
    public TSelf Invert();

    #endregion
}
