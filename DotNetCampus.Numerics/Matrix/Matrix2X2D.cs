namespace DotNetCampus.Numerics.Matrix;

public partial record Matrix2X2D : ISquareMatrix<Matrix2X2D, Vector2D, double>
{
    #region 静态变量

    /// <inheritdoc />
    public static Matrix2X2D Identity { get; } = new(1, 0, 0, 1);

    #endregion

    #region 属性

    /// <inheritdoc />
    public double Determinant => M11 * M22 - M12 * M21;

    /// <inheritdoc />
    public bool Invertible => !Determinant.IsAlmostZero(FrobeniusNorm);

    /// <inheritdoc />
    public Matrix2X2D? Inverse
    {
        get
        {
            var det = M11 * M22 - M12 * M21;
            if (det.IsAlmostZero(FrobeniusNorm))
                return null;
            return new Matrix2X2D(M22 / det, -M12 / det, -M21 / det, M11 / det);
        }
    }

    #endregion

    #region 成员方法

    /// <inheritdoc />
    public Matrix2X2D Invert()
    {
        return Inverse ?? throw new MatrixNonInvertibleException(Determinant);
    }

    #endregion
}
