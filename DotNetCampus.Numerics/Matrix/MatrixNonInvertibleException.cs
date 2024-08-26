namespace DotNetCampus.Numerics.Matrix;

/// <summary>
/// 矩阵不可逆异常。
/// </summary>
public class MatrixNonInvertibleException : InvalidOperationException
{
    #region 属性

    /// <summary>
    /// 行列式。
    /// </summary>
    public double Det { get; }

    #endregion

    #region 构造函数

    /// <summary>
    /// 初始化 <see cref="MatrixNonInvertibleException"/> 类的新实例。
    /// </summary>
    public MatrixNonInvertibleException() : base("矩阵不可逆。")
    {
    }

    /// <summary>
    /// 使用指定的错误消息初始化 <see cref="MatrixNonInvertibleException"/> 类的新实例。
    /// </summary>
    /// <param name="message">描述错误的消息。</param>
    public MatrixNonInvertibleException(string message) : base(message)
    {
    }

    /// <summary>
    /// 使用指定的行列式值初始化 <see cref="MatrixNonInvertibleException"/> 类的新实例。
    /// </summary>
    /// <param name="det">行列式的值。</param>
    public MatrixNonInvertibleException(double det) : base($"矩阵不可逆。行列式为 {det}。")
    {
        Det = det;
    }

    /// <summary>
    /// 使用指定的错误消息和对作为此异常原因的内部异常的引用来初始化 <see cref="MatrixNonInvertibleException"/> 类的新实例。
    /// </summary>
    /// <param name="message">描述错误的消息。</param>
    /// <param name="innerException">导致当前异常的异常。</param>
    public MatrixNonInvertibleException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #endregion
}
