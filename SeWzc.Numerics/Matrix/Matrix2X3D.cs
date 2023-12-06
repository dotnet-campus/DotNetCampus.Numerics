namespace SeWzc.Numerics.Matrix;

public readonly struct Matrix2X3D : IMatrix<Matrix2X3D, Vector3D, Vector2D, double, Matrix3X2D>
{
    public Matrix2X3D(double m11, double m12, double m13, double m21, double m22, double m23)
    {
        Row1 = new Vector3D(m11, m12, m13);
        Row2 = new Vector3D(m21, m22, m23);
    }

    public Matrix2X3D(Vector3D row1, Vector3D row2)
    {
        Row1 = row1;
        Row2 = row2;
    }

    public double M11 => Row1.X;
    public double M12 => Row1.Y;
    public double M13 => Row1.Z;
    public double M21 => Row2.X;
    public double M22 => Row2.Y;
    public double M23 => Row2.Z;

    public Vector3D Row1 { get; init; }
    public Vector3D Row2 { get; init; }

    public Vector2D Column1 => new(M11, M21);
    public Vector2D Column2 => new(M12, M22);
    public Vector2D Column3 => new(M13, M23);

    /// <inheritdoc />
    public double this[int row, int column] => (row, column) switch
    {
        (0, 0) => M11,
        (0, 1) => M12,
        (0, 2) => M13,
        (1, 0) => M21,
        (1, 1) => M22,
        (1, 2) => M23,
        (_, 0 or 1 or 2) => throw new ArgumentOutOfRangeException(nameof(row), "2x3 矩阵的行索引必须是小于 2 的非负数。"),
        _ => throw new ArgumentOutOfRangeException(nameof(column), "2x3 矩阵的列索引必须是小于 3 的非负数。"),
    };

    /// <inheritdoc />
    public Matrix3X2D Transpose => new(Column1, Column2, Column3);

    /// <inheritdoc />
    public Vector3D GetRow(int row)
    {
        return row switch
        {
            0 => Row1,
            1 => Row2,
            _ => throw new ArgumentOutOfRangeException(nameof(row), "2x3 矩阵的行索引必须是小于 2 的非负数。"),
        };
    }

    /// <inheritdoc />
    public Vector2D GetColumn(int column)
    {
        return column switch
        {
            0 => Column1,
            1 => Column2,
            2 => Column3,
            _ => throw new ArgumentOutOfRangeException(nameof(column), "2x3 矩阵的列索引必须是小于 3 的非负数。"),
        };
    }

    /// <inheritdoc />
    public double FrobeniusNorm => Math.Sqrt(Row1.SquaredLength + Row2.SquaredLength);

    #region 运算符重载

    /// <inheritdoc />
    public static Matrix2X3D operator +(Matrix2X3D matrix1, Matrix2X3D matrix2)
    {
        return new Matrix2X3D(
            matrix1.Row1 + matrix2.Row1,
            matrix1.Row2 + matrix2.Row2);
    }

    /// <inheritdoc />
    public static Matrix2X3D operator -(Matrix2X3D matrix1, Matrix2X3D matrix2)
    {
        return new Matrix2X3D(
            matrix1.Row1 - matrix2.Row1,
            matrix1.Row2 - matrix2.Row2);
    }

    /// <inheritdoc />
    public static Matrix2X3D operator *(Matrix2X3D matrix, double scalar)
    {
        return new Matrix2X3D(
            matrix.Row1 * scalar,
            matrix.Row2 * scalar);
    }

    /// <inheritdoc />
    public static Vector2D operator *(Matrix2X3D matrix, Vector3D vector)
    {
        return new Vector2D(
            matrix.Row1 * vector,
            matrix.Row2 * vector);
    }

    /// <inheritdoc />
    public static Vector3D operator *(Vector2D vector, Matrix2X3D matrix)
    {
        return new Vector3D(
            matrix.Column1 * vector,
            matrix.Column2 * vector,
            matrix.Column3 * vector);
    }

    /// <inheritdoc />
    public static Matrix2X3D operator *(double scalar, Matrix2X3D matrix)
    {
        return matrix * scalar;
    }

    /// <inheritdoc />
    public static Matrix2X3D operator /(Matrix2X3D matrix, double scalar)
    {
        return new Matrix2X3D(
            matrix.Row1 / scalar,
            matrix.Row2 / scalar);
    }

    /// <inheritdoc />
    public static Matrix2X3D operator -(Matrix2X3D matrix)
    {
        return new Matrix2X3D(
            -matrix.Row1,
            -matrix.Row2);
    }

    #endregion
}