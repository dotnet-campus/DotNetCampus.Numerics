﻿using System.Numerics;

namespace SeWzc.Numerics.Matrix;

/// <summary>
/// 矩阵的接口。
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="TRow">每行的向量。</typeparam>
/// <typeparam name="TColumn">每一列的向量。</typeparam>
/// <typeparam name="TNum"></typeparam>
/// <typeparam name="TTranspose"></typeparam>
public interface IMatrix<TSelf, TRow, TColumn, TNum, TTranspose>
    where TSelf : IMatrix<TSelf, TRow, TColumn, TNum, TTranspose>
    where TRow : unmanaged, IVector<TRow, TNum>
    where TColumn : unmanaged, IVector<TColumn, TNum>
    where TNum : unmanaged, INumber<TNum>
    where TTranspose : IMatrix<TTranspose, TColumn, TRow, TNum, TSelf>
{
    /// <summary>
    /// 矩阵行数。
    /// </summary>
    public static int RowCount => TColumn.Dimension;

    /// <summary>
    /// 矩阵列数。
    /// </summary>
    public static int ColumnCount => TRow.Dimension;

    /// <summary>
    /// 获取矩阵的第 <paramref name="row" /> 行、第 <paramref name="column" /> 列的元素。
    /// </summary>
    /// <param name="row">行的索引。基于 0 的索引。</param>
    /// <param name="column">列的索引。基于 0 的索引。</param>
    public TNum this[int row, int column] { get; }

    /// <summary>
    /// 获取转置矩阵。
    /// </summary>
    /// <returns></returns>
    public TTranspose Transpose { get; }

    /// <summary>
    /// 获取矩阵的第 <paramref name="row" /> 行。
    /// </summary>
    /// <param name="row">行的索引。基于 0 的索引。</param>
    /// <returns></returns>
    public TRow GetRow(int row);

    /// <summary>
    /// 获取矩阵的第 <paramref name="column" /> 列。
    /// </summary>
    /// <param name="column">列的索引。基于 0 的索引。</param>
    /// <returns></returns>
    public TColumn GetColumn(int column);

    /// <summary>
    /// Frobenius范数。等于矩阵元素的平方和的平方根。
    /// </summary>
    public double FrobeniusNorm { get; }

    #region 运算符重载

    public static abstract TSelf operator +(TSelf matrix1, TSelf matrix2);

    public static abstract TSelf operator -(TSelf matrix1, TSelf matrix2);

    public static abstract TSelf operator *(TSelf matrix, TNum scalar);

    public static abstract TColumn operator *(TSelf matrix, TRow vector);

    public static abstract TRow operator *(TColumn vector, TSelf matrix);

    public static abstract TSelf operator *(TNum scalar, TSelf matrix);

    public static abstract TSelf operator /(TSelf matrix, TNum scalar);

    public static abstract TSelf operator -(TSelf matrix);

    #endregion
}

public interface ISquareMatrix<TSelf, TVector, TNum> : IMatrix<TSelf, TVector, TVector, TNum, TSelf>
    where TSelf : ISquareMatrix<TSelf, TVector, TNum>
    where TVector : unmanaged, IVector<TVector, TNum>
    where TNum : unmanaged, INumber<TNum>
{
    /// <summary>
    /// 矩阵的逆。
    /// </summary>
    /// <returns></returns>
    /// <exception cref="MatrixNonInvertibleException"></exception>
    public TSelf Inverse();

    #region 运算符重载

    public static abstract TSelf operator *(TSelf matrix1, TSelf matrix2);

    #endregion
}

public class MatrixNonInvertibleException : Exception
{
    public MatrixNonInvertibleException() : base("矩阵不可逆。")
    {
    }

    public MatrixNonInvertibleException(string message) : base(message)
    {
    }

    public MatrixNonInvertibleException(double det) : base($"矩阵不可逆。行列式为 {det}。")
    {
        Det = det;
    }

    /// <summary>
    /// 行列式。
    /// </summary>
    public double Det { get; }
}