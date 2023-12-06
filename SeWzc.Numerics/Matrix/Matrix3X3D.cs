namespace SeWzc.Numerics.Matrix;

public readonly struct Matrix3X3D : ISquareMatrix<Matrix3X3D, Vector3D, double>
{
    public Matrix3X3D(double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33)
    {
        Row1 = new Vector3D(m11, m12, m13);
        Row2 = new Vector3D(m21, m22, m23);
        Row3 = new Vector3D(m31, m32, m33);
    }

    public Matrix3X3D(Vector3D row1, Vector3D row2, Vector3D row3)
    {
        Row1 = row1;
        Row2 = row2;
        Row3 = row3;
    }

    public double M11 => Row1.X;
    public double M12 => Row1.Y;
    public double M13 => Row1.Z;
    public double M21 => Row2.X;
    public double M22 => Row2.Y;
    public double M23 => Row2.Z;
    public double M31 => Row3.X;
    public double M32 => Row3.Y;
    public double M33 => Row3.Z;
    public Vector3D Row1 { get; init; }
    public Vector3D Row2 { get; init; }
    public Vector3D Row3 { get; init; }
    public Vector3D Column1 => new(M11, M21, M31);
    public Vector3D Column2 => new(M12, M22, M32);
    public Vector3D Column3 => new(M13, M23, M33);

    /// <inheritdoc />
    public double this[int row, int column] => (row, column) switch
    {
        (0, 0) => M11,
        (0, 1) => M12,
        (0, 2) => M13,
        (1, 0) => M21,
        (1, 1) => M22,
        (1, 2) => M23,
        (2, 0) => M31,
        (2, 1) => M32,
        (2, 2) => M33,
        (_, 0 or 1 or 2) => throw new ArgumentOutOfRangeException(nameof(row), "3x3 矩阵的行索引必须是小于 3 的非负数。"),
        _ => throw new ArgumentOutOfRangeException(nameof(column), "3x3 矩阵的列索引必须是小于 3 的非负数。"),
    };

    /// <inheritdoc />
    public Matrix3X3D Transpose => new(Column1, Column2, Column3);

    /// <inheritdoc />
    public Vector3D GetRow(int row)
    {
        return row switch
        {
            0 => Row1,
            1 => Row2,
            2 => Row3,
            _ => throw new ArgumentOutOfRangeException(nameof(row), "3x3 矩阵的行索引必须是小于 3 的非负数。"),
        };
    }

    /// <inheritdoc />
    public Vector3D GetColumn(int column)
    {
        return column switch
        {
            0 => Column1,
            1 => Column2,
            2 => Column3,
            _ => throw new ArgumentOutOfRangeException(nameof(column), "3x3 矩阵的列索引必须是小于 3 的非负数。"),
        };
    }

    /// <inheritdoc />
    public Matrix3X3D Inverse()
    {
        var det = M11 * (M22 * M33 - M23 * M32) - M12 * (M21 * M33 - M23 * M31) + M13 * (M21 * M32 - M22 * M31);

        if (det == 0)
            throw new MatrixNonInvertibleException(det);

        if (det.IsAlmostZero(Row1.SquaredLength + Row2.SquaredLength + Row3.SquaredLength))
            throw new MatrixNonInvertibleException(det);

        var invDet = 1 / det;
        var invM11 = (M22 * M33 - M23 * M32) * invDet;
        var invM12 = -(M12 * M33 - M13 * M32) * invDet;
        var invM13 = (M12 * M23 - M13 * M22) * invDet;
        var invM21 = -(M21 * M33 - M23 * M31) * invDet;
        var invM22 = (M11 * M33 - M13 * M31) * invDet;
        var invM23 = -(M11 * M23 - M13 * M21) * invDet;
        var invM31 = (M21 * M32 - M22 * M31) * invDet;
        var invM32 = -(M11 * M32 - M12 * M31) * invDet;
        var invM33 = (M11 * M22 - M12 * M21) * invDet;
        return new Matrix3X3D(invM11, invM12, invM13, invM21, invM22, invM23, invM31, invM32, invM33);
    }

    /// <inheritdoc />
    public double FrobeniusNorm => Math.Sqrt(Row1.SquaredLength + Row2.SquaredLength + Row3.SquaredLength);

    #region 运算符重载

    /// <inheritdoc />
    public static Matrix3X3D operator +(Matrix3X3D matrix1, Matrix3X3D matrix2)
    {
        return new Matrix3X3D(
            matrix1.Row1 + matrix2.Row1,
            matrix1.Row2 + matrix2.Row2,
            matrix1.Row3 + matrix2.Row3);
    }

    /// <inheritdoc />
    public static Matrix3X3D operator -(Matrix3X3D matrix1, Matrix3X3D matrix2)
    {
        return new Matrix3X3D(
            matrix1.Row1 - matrix2.Row1,
            matrix1.Row2 - matrix2.Row2,
            matrix1.Row3 - matrix2.Row3);
    }

    /// <inheritdoc />
    public static Matrix3X3D operator *(Matrix3X3D matrix, double scalar)
    {
        return new Matrix3X3D(
            matrix.Row1 * scalar,
            matrix.Row2 * scalar,
            matrix.Row3 * scalar);
    }

    /// <inheritdoc />
    public static Vector3D operator *(Matrix3X3D matrix, Vector3D vector)
    {
        return new Vector3D(
            matrix.Row1 * vector,
            matrix.Row2 * vector,
            matrix.Row3 * vector);
    }

    /// <inheritdoc />
    public static Vector3D operator *(Vector3D vector, Matrix3X3D matrix)
    {
        return new Vector3D(
            matrix.Column1 * vector,
            matrix.Column2 * vector,
            matrix.Column3 * vector);
    }

    /// <inheritdoc />
    public static Matrix3X3D operator *(double scalar, Matrix3X3D matrix)
    {
        return matrix * scalar;
    }

    /// <inheritdoc />
    public static Matrix3X3D operator /(Matrix3X3D matrix, double scalar)
    {
        return new Matrix3X3D(
            matrix.Row1 / scalar,
            matrix.Row2 / scalar,
            matrix.Row3 / scalar);
    }

    /// <inheritdoc />
    public static Matrix3X3D operator -(Matrix3X3D matrix)
    {
        return new Matrix3X3D(-matrix.Row1, -matrix.Row2, -matrix.Row3);
    }

    /// <inheritdoc />
    public static Matrix3X3D operator *(Matrix3X3D matrix, Matrix3X3D matrix2)
    {
        return new Matrix3X3D(
            matrix.Row1 * matrix2.Column1, matrix.Row1 * matrix2.Column2, matrix.Row1 * matrix2.Column3,
            matrix.Row2 * matrix2.Column1, matrix.Row2 * matrix2.Column2, matrix.Row2 * matrix2.Column3,
            matrix.Row3 * matrix2.Column1, matrix.Row3 * matrix2.Column2, matrix.Row3 * matrix2.Column3);
    }

    #endregion
}