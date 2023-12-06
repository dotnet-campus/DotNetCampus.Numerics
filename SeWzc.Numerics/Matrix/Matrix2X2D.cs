namespace SeWzc.Numerics.Matrix;

public readonly struct Matrix2X2D : ISquareMatrix<Matrix2X2D, Vector2D, double>
{
    public Matrix2X2D(double m11, double m12, double m21, double m22)
    {
        Row1 = new Vector2D(m11, m12);
        Row2 = new Vector2D(m21, m22);
    }

    public Matrix2X2D(Vector2D row1, Vector2D row2)
    {
        Row1 = row1;
        Row2 = row2;
    }

    public double M11 => Row1.X;
    public double M12 => Row1.Y;
    public double M21 => Row2.X;
    public double M22 => Row2.Y;
    public Vector2D Row1 { get; init; }
    public Vector2D Row2 { get; init; }
    public Vector2D Column1 => new(M11, M21);
    public Vector2D Column2 => new(M12, M22);

    /// <inheritdoc />
    public double this[int row, int column] => (row, column) switch
    {
        (0, 0) => M11,
        (0, 1) => M12,
        (1, 0) => M21,
        (1, 1) => M22,
        (_, 0 or 1) => throw new ArgumentOutOfRangeException(nameof(row), "2x2 矩阵的行索引必须是小于 2 的非负数。"),
        _ => throw new ArgumentOutOfRangeException(nameof(column), "2x2 矩阵的列索引必须是小于 2 的非负数。"),
    };

    /// <inheritdoc />
    public Vector2D GetRow(int row)
    {
        return row switch
        {
            0 => Row1,
            1 => Row2,
            _ => throw new ArgumentOutOfRangeException(nameof(row), "2x2 矩阵的行索引必须是小于 2 的非负数。"),
        };
    }

    /// <inheritdoc />
    public Vector2D GetColumn(int column)
    {
        return column switch
        {
            0 => Column1,
            1 => Column2,
            _ => throw new ArgumentOutOfRangeException(nameof(column), "2x2 矩阵的列索引必须是小于 2 的非负数。"),
        };
    }

    /// <inheritdoc />
    public Matrix2X2D Transpose => new(Column1, Column2);

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

    /// <inheritdoc />
    public double FrobeniusNorm => Math.Sqrt(Row1.SquaredLength + Row2.SquaredLength);

    #region 运算符重载

    /// <inheritdoc />
    public static Matrix2X2D operator +(Matrix2X2D matrix1, Matrix2X2D matrix2)
    {
        return new Matrix2X2D(
            matrix1.Row1 + matrix2.Row1,
            matrix1.Row2 + matrix2.Row2
        );
    }

    /// <inheritdoc />
    public static Matrix2X2D operator -(Matrix2X2D matrix1, Matrix2X2D matrix2)
    {
        return new Matrix2X2D(
            matrix1.Row1 - matrix2.Row1,
            matrix1.Row2 - matrix2.Row2
        );
    }

    /// <inheritdoc />
    public static Matrix2X2D operator *(Matrix2X2D matrix, double scalar)
    {
        return new Matrix2X2D(
            matrix.Row1 * scalar,
            matrix.Row2 * scalar
        );
    }

    /// <inheritdoc />
    public static Vector2D operator *(Matrix2X2D matrix, Vector2D vector)
    {
        return new Vector2D(
            matrix.Row1 * vector,
            matrix.Row2 * vector
        );
    }

    /// <inheritdoc />
    public static Vector2D operator *(Vector2D vector, Matrix2X2D matrix)
    {
        return new Vector2D(
            matrix.Column1 * vector,
            matrix.Column2 * vector
        );
    }

    /// <inheritdoc />
    public static Matrix2X2D operator *(double scalar, Matrix2X2D matrix)
    {
        return new Matrix2X2D(
            matrix.Row1 * scalar,
            matrix.Row2 * scalar
        );
    }

    /// <inheritdoc />
    public static Matrix2X2D operator /(Matrix2X2D matrix, double scalar)
    {
        return new Matrix2X2D(
            matrix.Row1 / scalar,
            matrix.Row2 / scalar
        );
    }

    /// <inheritdoc />
    public static Matrix2X2D operator -(Matrix2X2D matrix)
    {
        return new Matrix2X2D(
            -matrix.Row1,
            -matrix.Row2
        );
    }

    /// <inheritdoc />
    public static Matrix2X2D operator *(Matrix2X2D matrix, Matrix2X2D matrix2)
    {
        return new Matrix2X2D(
            matrix.Row1 * matrix2.Column1,
            matrix.Row1 * matrix2.Column2,
            matrix.Row2 * matrix2.Column1,
            matrix.Row2 * matrix2.Column2
        );
    }

    #endregion
}