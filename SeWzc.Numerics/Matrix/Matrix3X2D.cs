namespace SeWzc.Numerics.Matrix;

public readonly struct Matrix3X2D : IMatrix<Matrix3X2D, Vector2D, Vector3D, double, Matrix2X3D>
{
    public Matrix3X2D(double m11, double m12, double m22, double m21, double m31, double m32)
    {
        Row1 = new Vector2D(m11, m12);
        Row2 = new Vector2D(m21, m22);
        Row3 = new Vector2D(m31, m32);
    }

    public Matrix3X2D(Vector2D row1, Vector2D row2, Vector2D row3)
    {
        Row1 = row1;
        Row2 = row2;
        Row3 = row3;
    }

    public double M11 => Row1.X;
    public double M12 => Row1.Y;
    public double M21 => Row2.X;
    public double M22 => Row2.Y;
    public double M31 => Row3.X;
    public double M32 => Row3.Y;

    public Vector2D Row1 { get; init; }
    public Vector2D Row2 { get; init; }
    public Vector2D Row3 { get; init; }

    public Vector3D Column1 => new(M11, M21, M31);
    public Vector3D Column2 => new(M12, M22, M32);

    /// <inheritdoc />
    public double this[int row, int column] => (row, column) switch
    {
        (0, 0) => M11,
        (0, 1) => M12,
        (1, 0) => M21,
        (1, 1) => M22,
        (2, 0) => M31,
        (2, 1) => M32,
        (_, 0 or 1) => throw new ArgumentOutOfRangeException(nameof(row), "3x2 矩阵的行索引必须是小于 3 的非负数。"),
        _ => throw new ArgumentOutOfRangeException(nameof(column), "3x2 矩阵的列索引必须是小于 2 的非负数。"),
    };

    /// <inheritdoc />
    public Matrix2X3D Transpose => new(Column1, Column2);

    /// <inheritdoc />
    public Vector2D GetRow(int row)
    {
        return row switch
        {
            0 => Row1,
            1 => Row2,
            2 => Row3,
            _ => throw new ArgumentOutOfRangeException(nameof(row), "3x2 矩阵的行索引必须是小于 3 的非负数。"),
        };
    }

    /// <inheritdoc />
    public Vector3D GetColumn(int column)
    {
        return column switch
        {
            0 => Column1,
            1 => Column2,
            _ => throw new ArgumentOutOfRangeException(nameof(column), "3x2 矩阵的列索引必须是小于 2 的非负数。"),
        };
    }

    /// <inheritdoc />
    public double FrobeniusNorm => Math.Sqrt(Row1.SquaredLength + Row2.SquaredLength + Row3.SquaredLength);

    #region 运算符重载

    /// <inheritdoc />
    public static Matrix3X2D operator +(Matrix3X2D matrix1, Matrix3X2D matrix2)
    {
        return new Matrix3X2D(
            matrix1.Row1 + matrix2.Row1,
            matrix1.Row2 + matrix2.Row2,
            matrix1.Row3 + matrix2.Row3);
    }

    /// <inheritdoc />
    public static Matrix3X2D operator -(Matrix3X2D matrix1, Matrix3X2D matrix2)
    {
        return new Matrix3X2D(
            matrix1.Row1 - matrix2.Row1,
            matrix1.Row2 - matrix2.Row2,
            matrix1.Row3 - matrix2.Row3);
    }

    /// <inheritdoc />
    public static Matrix3X2D operator *(Matrix3X2D matrix, double scalar)
    {
        return new Matrix3X2D(
            matrix.Row1 * scalar,
            matrix.Row2 * scalar,
            matrix.Row3 * scalar);
    }

    /// <inheritdoc />
    public static Vector3D operator *(Matrix3X2D matrix, Vector2D vector)
    {
        return new Vector3D(
            matrix.Row1 * vector,
            matrix.Row2 * vector,
            matrix.Row3 * vector);
    }

    /// <inheritdoc />
    public static Vector2D operator *(Vector3D vector, Matrix3X2D matrix)
    {
        return new Vector2D(
            matrix.Column1 * vector,
            matrix.Column2 * vector);
    }

    /// <inheritdoc />
    public static Matrix3X2D operator *(double scalar, Matrix3X2D matrix)
    {
        return matrix * scalar;
    }

    /// <inheritdoc />
    public static Matrix3X2D operator /(Matrix3X2D matrix, double scalar)
    {
        return new Matrix3X2D(
            matrix.Row1 / scalar,
            matrix.Row2 / scalar,
            matrix.Row3 / scalar);
    }

    /// <inheritdoc />
    public static Matrix3X2D operator -(Matrix3X2D matrix)
    {
        return new Matrix3X2D(
            -matrix.Row1,
            -matrix.Row2,
            -matrix.Row3);
    }

    #endregion
}