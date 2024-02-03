namespace SeWzc.Numerics.Matrix;

public class MatrixNonInvertibleException : Exception
{
    #region 属性

    /// <summary>
    /// 行列式。
    /// </summary>
    public double Det { get; }

    #endregion

    #region 构造函数

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

    #endregion
}