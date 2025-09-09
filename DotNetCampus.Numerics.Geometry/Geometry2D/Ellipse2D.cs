using DotNetCampus.Numerics.Functions;
using DotNetCampus.Numerics.Matrix;

namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 椭圆。
/// </summary>
public readonly record struct Ellipse2D : IAffineTransformable2D<Ellipse2D>, IGeometry2D
{
    #region 静态方法

    /// <summary>
    /// 获取特征向量对应的角度。
    /// </summary>
    /// <param name="m"></param>
    /// <param name="eigenValue"></param>
    /// <returns></returns>
    private static AngularMeasure GetEigenVectorAngle(Matrix2X2D m, double eigenValue)
    {
        var eigenVector1 = new Vector2D(eigenValue - m.M22, m.M21);
        var eigenVector2 = new Vector2D(m.M12, eigenValue - m.M11);
        // 数学上两个向量都是对的，但是因为存在某个向量为 0 的情况，所以这里取长度较大的向量
        return eigenVector1.LengthSquared > eigenVector2.LengthSquared ? eigenVector1.Angle : eigenVector2.Angle;
    }

    #endregion

    #region 属性

    /// <summary>
    /// 椭圆上轴端点 A。
    /// </summary>
    public Point2D AxisEndpointA => Center + A * Angle.UnitVector;

    /// <summary>
    /// 椭圆上轴端点 B。
    /// </summary>
    public Point2D AxisEndpointB => Center + B * (Angle + AngularMeasure.HalfPi).UnitVector;

    /// <summary>
    /// 椭圆的中心。
    /// </summary>
    public Point2D Center { get; }

    /// <summary>
    /// 椭圆的半长轴。
    /// </summary>
    public double A { get; }

    /// <summary>
    /// 椭圆的半短轴。
    /// </summary>
    public double B { get; }

    /// <summary>
    /// 椭圆的旋转角。范围为 (-π / 2, π / 2]。
    /// </summary>
    public AngularMeasure Angle
    {
        get;
        init => field = value.Normalized switch
        {
            { Radian: > Math.PI * 3 / 2 } angle => angle - AngularMeasure.Tau,
            { Radian: > Math.PI / 2 } angle => angle - AngularMeasure.Pi,
            var angle => angle,
        };
    }

    #endregion

    #region 构造函数

    /// <summary>
    /// 通过圆创建椭圆。
    /// </summary>
    /// <param name="circle"></param>
    public Ellipse2D(Circle2D circle) : this(circle.Center, circle.Radius, circle.Radius, AngularMeasure.Zero)
    {
    }

    /// <summary>
    /// 椭圆。
    /// </summary>
    /// <remarks>
    /// 如果 <paramref name="A" /> 小于 <paramref name="B" />，则会自动交换两者的值。
    /// </remarks>
    /// <param name="Center">椭圆的中心。</param>
    /// <param name="A">椭圆的半长轴。</param>
    /// <param name="B">椭圆的半短轴。</param>
    /// <param name="Angle">椭圆的旋转角。</param>
    public Ellipse2D(Point2D Center, double A, double B, AngularMeasure Angle)
    {
        this.Center = Center;
        if (A < B)
        {
            (A, B) = (B, A);
            Angle += AngularMeasure.HalfPi;
        }

        this.A = A;
        this.B = B;
        this.Angle = Angle;
    }

    #endregion

    #region 成员方法

    /// <inheritdoc />
    public BoundingBox2D GetBoundingBox()
    {
        var (m11, m12, m21, m22, offsetX, offsetY) = AffineTransformation2D.CreateScaling(new Scaling2D(A, B)).Rotate(Angle).Translate(Center.ToVector());
        var maxX = Math.Sqrt(m11 * m11 + m12 * m12);
        var maxY = Math.Sqrt(m21 * m21 + m22 * m22);
        return BoundingBox2D.Create(-maxX + offsetX, -maxY + offsetY, maxX + offsetX, maxY + offsetY);
    }

    #endregion

    #region Transforms

    /// <inheritdoc />
    public Ellipse2D Transform(AffineTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        // 计算单位圆变换到该椭圆的仿射变换矩阵
        var oldTransform = AffineTransformation2D.Create(Center.ToVector(), Angle, 0, new Scaling2D(A, B));
        // 计算单位圆变换到新生成的椭圆的仿射变换矩阵
        var newTransform = oldTransform.Apply(transformation);

        // 新椭圆的中心点
        var center = (Point2D)newTransform.Translation;

        // 后面计算新生成的椭圆的半长轴、半短轴和旋转角

        // 从仿射变换矩阵中获取线性变换矩阵，不考虑中心点平移
        var matrix = new Matrix2X2D(newTransform.M11, newTransform.M12, newTransform.M21, newTransform.M22);
        var m = matrix * matrix.Transpose;
        // 计算特征值，以及特征值对应的特征向量
        var eigenEquation = new QuadraticFunction<double>(1, -(m.M11 + m.M22), m.Determinant);
        var eigenValues = eigenEquation.GetRoots();
        var eigenValue1 = eigenValues[0];
        var eigenValue2 = eigenValues[^1];
        var eigenVectorAngle2 = GetEigenVectorAngle(m, eigenValue2);

        // 特征值分别是长轴和短轴的平方，所以获取特征值平方根即可得到长轴和短轴
        var a = Math.Sqrt(eigenValue2);
        var b = Math.Sqrt(eigenValue1);
        // 旋转是半长轴和 x 轴的夹角，所以获取特征向量的角度即可得到旋转角
        return new Ellipse2D(center, a, b, eigenVectorAngle2);
    }

    /// <inheritdoc />
    public Ellipse2D ScaleTransform(Scaling2D scaling)
    {
        return Transform(AffineTransformation2D.CreateScaling(scaling));
    }

    /// <inheritdoc />
    public Ellipse2D Transform(SimilarityTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        var newCenter = Center.Transform(transformation);
        var newA = A * transformation.Scaling;
        var newB = B * transformation.Scaling;
        var newAngle = Angle + transformation.Rotation;

        return new Ellipse2D(newCenter, newA, newB, newAngle);
    }

    /// <inheritdoc />
    public Ellipse2D ScaleTransform(double scaling)
    {
        return new Ellipse2D(Center.ScaleTransform(scaling), A * scaling, B * scaling, Angle);
    }

    /// <inheritdoc />
    public Ellipse2D RotateTransform(AngularMeasure rotation)
    {
        return new Ellipse2D(Center.RotateTransform(rotation), A, B, Angle + rotation);
    }

    /// <inheritdoc />
    public Ellipse2D TranslateTransform(Vector2D translation)
    {
        return new Ellipse2D(Center.TranslateTransform(translation), A, B, Angle);
    }

    #endregion
}
