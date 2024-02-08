namespace SeWzc.Numerics.Matrix;

public readonly partial record struct Matrix2X2D : ISquareMatrix<Matrix2X2D, Vector2D, double>
{
    /// <inheritdoc />
    public double Determinant => M11 * M22 - M12 * M21;

    /// <inheritdoc />
    public Matrix2X2D Inverse()
    {
        var det = M11 * M22 - M12 * M21;
        if (det == 0)
            throw new MatrixNonInvertibleException(det);
        if (det.IsAlmostZero(FrobeniusNorm))
            throw new MatrixNonInvertibleException(det);
        return new Matrix2X2D(M22 / det, -M12 / det, -M21 / det, M11 / det);
    }
}