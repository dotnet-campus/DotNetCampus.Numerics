using SeWzc.Numerics.Equations;
using SeWzc.Numerics.Matrix;

namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 椭圆。
/// </summary>
public readonly record struct Ellipse2D : IAffineTransformable2D<Ellipse2D>
{
    #region 私有字段

    private readonly AngularMeasure _angle;

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
        get => _angle;
        init => _angle = value.Normalized switch
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
        this.A = Math.Max(A, B);
        this.B = Math.Min(A, B);
        this.Angle = Angle;
    }

    #endregion

    #region 成员方法

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

        // 获取椭圆的方程 X^T * M * X = 1 中的 M 矩阵
        var inverse = matrix.Invert();
        var inverseTranspose = inverse.Transpose;
        var m = inverseTranspose * inverse;

        // 计算特征值，以及特征值对应的特征向量
        var eigenEquation = new QuadraticEquation(1, -(m.M11 + m.M22), m.Determinant);
        var eigenValue1 = eigenEquation.Root1;
        var eigenValue2 = eigenEquation.Root2;
        var eigenVector2 = new Vector2D(m.M12, eigenValue2 - m.M11);

        // 特征值分别是长轴和短轴的平方的倒数，所以获取特征值平方根的倒数即可得到长轴和短轴
        var b = Math.ReciprocalSqrtEstimate(eigenValue1);
        var a = Math.ReciprocalSqrtEstimate(eigenValue2);
        // 旋转是半长轴和 x 轴的夹角，所以获取特征向量的角度即可得到旋转角
        var rotate = eigenVector2.Angle;
        return new Ellipse2D(center, a, b, rotate);
    }

    #endregion
}
