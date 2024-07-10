using SeWzc.Numerics.Matrix;

namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 2 维仿射变换。
/// </summary>
/// <param name="M11">变换矩阵的第一行第一列的元素。</param>
/// <param name="M12">变换矩阵的第一行第二列的元素。</param>
/// <param name="M21">变换矩阵的第二行第一列的元素。</param>
/// <param name="M22">变换矩阵的第二行第二列的元素。</param>
/// <param name="OffsetX">变换矩阵的水平偏移量。</param>
/// <param name="OffsetY">变换矩阵的垂直偏移量。</param>
public record AffineTransformation2D(double M11, double M12, double M21, double M22, double OffsetX, double OffsetY)
{
    #region 静态变量

    /// <summary>
    /// 表示单位矩阵的仿射变换。
    /// </summary>
    public static AffineTransformation2D Identity { get; } = new(1, 0, 0, 1, 0, 0);

    #endregion

    #region 静态方法

    /// <summary>
    /// 创建一个缩放变换的仿射变换。
    /// </summary>
    /// <param name="scaling">缩放变换的参数。</param>
    /// <returns>缩放变换的仿射变换。</returns>
    public static AffineTransformation2D CreateScaling(Scaling2D scaling)
    {
        return new AffineTransformation2D(scaling.ScaleX, 0, 0, scaling.ScaleY, 0, 0);
    }

    /// <summary>
    /// 创建一个剪切变换的仿射变换。
    /// </summary>
    /// <param name="shear">剪切变换的系数。</param>
    /// <returns>剪切变换的仿射变换。</returns>
    public static AffineTransformation2D CreateShearing(double shear)
    {
        return new AffineTransformation2D(1, shear, 0, 1, 0, 0);
    }

    /// <summary>
    /// 创建一个旋转变换的仿射变换。
    /// </summary>
    /// <param name="angle">旋转变换的角度。</param>
    /// <returns>旋转变换的仿射变换。</returns>
    public static AffineTransformation2D CreateRotation(AngularMeasure angle)
    {
        var cos = angle.Cos();
        var sin = angle.Sin();
        return new AffineTransformation2D(cos, -sin, sin, cos, 0, 0);
    }

    /// <summary>
    /// 创建一个平移变换的仿射变换。
    /// </summary>
    /// <param name="offset">平移变换的偏移量。</param>
    /// <returns>平移变换的仿射变换。</returns>
    public static AffineTransformation2D CreateTranslation(Vector2D offset)
    {
        return new AffineTransformation2D(1, 0, 0, 1, offset.X, offset.Y);
    }

    /// <summary>
    /// 通过仿射变换分解创建一个仿射变换。
    /// </summary>
    /// <param name="decomposition">仿射变换分解。</param>
    /// <returns>创建的仿射变换。</returns>
    public static AffineTransformation2D Create(AffineTransformation2DDecomposition decomposition)
    {
        ArgumentNullException.ThrowIfNull(decomposition);
        return Create(decomposition.Translation, decomposition.Rotation, decomposition.Shearing, decomposition.Scaling);
    }

    /// <summary>
    /// 通过仿射变换分解创建一个仿射变换。
    /// </summary>
    /// <param name="translation">平移向量。</param>
    /// <param name="rotation">旋转角度。</param>
    /// <param name="shear">剪切系数。</param>
    /// <param name="scaling">缩放参数。</param>
    /// <returns></returns>
    public static AffineTransformation2D Create(Vector2D translation, AngularMeasure rotation, double shear, Scaling2D scaling)
    {
        var cos = rotation.Cos();
        var sin = rotation.Sin();
        return new AffineTransformation2D(
            scaling.ScaleX * cos,
            scaling.ScaleY * (shear * cos - sin),
            scaling.ScaleX * sin,
            scaling.ScaleY * (shear * sin + cos),
            translation.X,
            translation.Y);
    }

    #endregion

    #region 属性

    /// <summary>
    /// 获取仿射变换的缩放参数。
    /// </summary>
    public Scaling2D Scaling
    {
        get
        {
            var scaleX = Math.Sqrt(M11 * M11 + M21 * M21);
            var scaleY = (M11 * M22 - M12 * M21) / scaleX;
            return new Scaling2D(scaleX, scaleY);
        }
    }

    /// <summary>
    /// 获取仿射变换的剪切系数。
    /// </summary>
    public double Shearing => (M11 * M12 + M21 * M22) / (M11 * M22 - M12 * M21);

    /// <summary>
    /// 获取仿射变换的旋转角度。
    /// </summary>
    public AngularMeasure Rotation => AngularMeasure.FromRadian(Math.Atan2(M21, M11));

    /// <summary>
    /// 获取仿射变换的平移向量。
    /// </summary>
    public Vector2D Translation => new(OffsetX, OffsetY);

    #endregion

    #region 成员方法

    /// <summary>
    /// 对仿射变换进行缩放。
    /// </summary>
    /// <param name="scaling">缩放变换的参数。</param>
    /// <returns>缩放后的仿射变换。</returns>
    public AffineTransformation2D Scale(Scaling2D scaling)
    {
        return new AffineTransformation2D(
            M11 * scaling.ScaleX,
            M12 * scaling.ScaleY,
            M21 * scaling.ScaleX,
            M22 * scaling.ScaleY,
            OffsetX * scaling.ScaleX,
            OffsetY * scaling.ScaleY);
    }

    /// <summary>
    /// 对仿射变换进行剪切。
    /// </summary>
    /// <param name="shear">剪切变换的系数。</param>
    /// <returns>剪切后的仿射变换。</returns>
    public AffineTransformation2D Shear(double shear)
    {
        return this with { M11 = M11 + shear * M21, M12 = M12 + shear * M22, OffsetX = OffsetX + shear * OffsetY };
    }

    /// <summary>
    /// 对仿射变换进行旋转。
    /// </summary>
    /// <param name="angle">旋转变换的角度。</param>
    /// <returns>旋转后的仿射变换。</returns>
    public AffineTransformation2D Rotate(AngularMeasure angle)
    {
        var cos = angle.Cos();
        var sin = angle.Sin();
        return new AffineTransformation2D(
            M11 * cos - M21 * sin,
            M12 * cos - M22 * sin,
            M21 * cos + M11 * sin,
            M22 * cos + M12 * sin,
            OffsetX * cos - OffsetY * sin,
            OffsetX * sin + OffsetY * cos);
    }

    /// <summary>
    /// 对仿射变换进行平移。
    /// </summary>
    /// <param name="offset">平移变换的偏移量。</param>
    /// <returns>平移后的仿射变换。</returns>
    public AffineTransformation2D Translate(Vector2D offset)
    {
        return this with { OffsetX = OffsetX + offset.X, OffsetY = OffsetY + offset.Y };
    }

    /// <summary>
    /// 应用另一个仿射变换到当前仿射变换。
    /// </summary>
    /// <param name="transformation">要应用的仿射变换。</param>
    /// <returns>应用后的仿射变换。</returns>
    public AffineTransformation2D Apply(AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);
        return new AffineTransformation2D(
            M11 * transformation.M11 + M21 * transformation.M12,
            M12 * transformation.M11 + M22 * transformation.M12,
            M11 * transformation.M21 + M21 * transformation.M22,
            M12 * transformation.M21 + M22 * transformation.M22,
            transformation.M11 * OffsetX + transformation.M12 * OffsetY + transformation.OffsetX,
            transformation.M21 * OffsetX + transformation.M22 * OffsetY + transformation.OffsetY);
    }

    /// <summary>
    /// 计算当前仿射变换的逆变换。
    /// </summary>
    /// <returns>逆变换后的仿射变换。</returns>
    public AffineTransformation2D Inverse()
    {
        var det = M11 * M22 - M12 * M21;
        if (det == 0)
            throw new MatrixNonInvertibleException(det);

        var invDet = 1 / det;
        return new AffineTransformation2D(
            M22 * invDet,
            -M12 * invDet,
            -M21 * invDet,
            M11 * invDet,
            (M12 * OffsetY - M22 * OffsetX) * invDet,
            (M21 * OffsetX - M11 * OffsetY) * invDet);
    }

    /// <summary>
    /// 对指定的点进行仿射变换。
    /// </summary>
    /// <param name="point">要进行变换的点。</param>
    /// <returns>变换后的点。</returns>
    public Point2D Transform(Point2D point)
    {
        return new Point2D(M11 * point.X + M12 * point.Y + OffsetX, M21 * point.X + M22 * point.Y + OffsetY);
    }

    /// <summary>
    /// 对指定的值进行仿射变换。
    /// </summary>
    /// <typeparam name="TIn">输入值的类型。</typeparam>
    /// <typeparam name="TOut">输出值的类型。</typeparam>
    /// <param name="value">要进行变换的值。</param>
    /// <returns>变换后的值。</returns>
    public TOut Transform<TIn, TOut>(TIn value) where TIn : IAffineTransformable2D<TOut>
    {
        return value.Transform(this);
    }

    /// <summary>
    /// 对仿射变换进行分解。
    /// </summary>
    /// <returns>分解后的仿射变换。</returns>
    public AffineTransformation2DDecomposition Decompose()
    {
        return new AffineTransformation2DDecomposition(Translation, Rotation, Shearing, Scaling);
    }

    #endregion

    #region 运算符重载

    /// <summary>
    /// 将仿射变换隐式转换为 3x3 矩阵。
    /// </summary>
    /// <param name="affineTransformation2D">要转换的仿射变换。</param>
    /// <returns>转换后的 3x3 矩阵。</returns>
    public static implicit operator Matrix3X3D(AffineTransformation2D affineTransformation2D)
    {
        ArgumentNullException.ThrowIfNull(affineTransformation2D);

        return new Matrix3X3D(affineTransformation2D.M11, affineTransformation2D.M12, affineTransformation2D.OffsetX,
            affineTransformation2D.M21, affineTransformation2D.M22, affineTransformation2D.OffsetY,
            0, 0, 1);
    }

    #endregion
}

/// <summary>
/// 2 维仿射变换的分解。
/// </summary>
/// <param name="Translation">平移向量。</param>
/// <param name="Rotation">旋转角度。</param>
/// <param name="Shearing">剪切系数。</param>
/// <param name="Scaling">缩放参数。</param>
public record AffineTransformation2DDecomposition(Vector2D Translation, AngularMeasure Rotation, double Shearing, Scaling2D Scaling);