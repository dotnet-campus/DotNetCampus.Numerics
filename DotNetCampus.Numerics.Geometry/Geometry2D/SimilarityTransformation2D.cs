using System.Numerics;
using System.Runtime.CompilerServices;

namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 2 维相似变换。相比与仿射变换，相似变换无法进行剪切变换和非等比缩放。
/// </summary>
/// <remarks>
/// 进行变换的顺序为：缩放、旋转、平移。<paramref name="IsYScaleNegative" /> 会在缩放的阶段作用到 Y 轴的缩放上。
/// </remarks>
/// <param name="Scaling">相似变换的缩放倍数。值必须是正数，负缩放本质即将旋转角加 <see cref="AngularMeasure.Pi" />。</param>
/// <param name="IsYScaleNegative">Y 轴方向是否翻转。</param>
/// <param name="Rotation">旋转角度。</param>
/// <param name="Translation">平移量。</param>
public record SimilarityTransformation2D(double Scaling, bool IsYScaleNegative, AngularMeasure Rotation, Vector2D Translation)
{
    #region 静态变量

    /// <summary>
    /// 表示单位矩阵的相似变换。
    /// </summary>
    public static SimilarityTransformation2D Identity { get; } = new(1, false, AngularMeasure.Zero, Vector2D.Zero);

    #endregion

    #region 属性

    /// <summary>
    /// 缩放值。值必须是正数。
    /// </summary>
    public double Scaling
    {
        get;
        init => field = value.CheckPositive();
    } = Scaling.CheckPositive();

    /// <summary>
    /// Y 轴方向是否翻转。
    /// </summary>
    public bool IsYScaleNegative { get; init; } = IsYScaleNegative;

    /// <summary>
    /// 旋转角度。
    /// </summary>
    public AngularMeasure Rotation
    {
        get;
        init => field = value.Normalized;
    } = Rotation.Normalized;

    /// <summary>
    /// 平移量。
    /// </summary>
    public Vector2D Translation { get; init; } = Translation;

    #endregion

    #region 构造函数

    /// <summary>
    /// 创建一个 Y 轴方向不进行翻转的相似变换。
    /// </summary>
    /// <param name="scaling"></param>
    /// <param name="rotation"></param>
    /// <param name="translation"></param>
    public SimilarityTransformation2D(double scaling, AngularMeasure rotation, Vector2D translation)
        : this(scaling, false, rotation, translation)
    {
    }

    #endregion

    #region 成员方法

    /// <summary>
    /// 将相似变换转换为仿射变换。
    /// </summary>
    /// <returns>与当前相似变换等效的仿射变换。</returns>
    public AffineTransformation2D ToAffineTransformation2D()
    {
        var scaling = Scaling2D.Create(Scaling, IsYScaleNegative ? -Scaling : Scaling);
        return AffineTransformation2D.Create(Translation, Rotation, 0, scaling);
    }

    /// <summary>
    /// 将相似变换进行缩放。
    /// </summary>
    /// <param name="scale">缩放倍数。必须是正数，否则使用 <see cref="Scale(double,bool,bool)" /> 代替。</param>
    /// <returns>缩放后的相似变换。</returns>
    /// <exception cref="ArgumentOutOfRangeException">缩放倍数不是正数。</exception>
    public SimilarityTransformation2D Scale(double scale)
    {
        return this with
        {
            Scaling = Scaling * scale.CheckPositive(),
            Translation = Translation * scale,
        };
    }

    /// <summary>
    /// 将相似变换进行缩放。
    /// </summary>
    /// <param name="scale">
    /// 缩放倍数。必须是正数，非正数的情况需要通过 <paramref name="isXScaleNegative" /> 和 <paramref name="isYScaleNegative" />
    /// 来指定。
    /// </param>
    /// <param name="isXScaleNegative">X 轴方向是否翻转。</param>
    /// <param name="isYScaleNegative">Y 轴方向是否翻转。</param>
    /// <returns>缩放后的相似变换。</returns>
    /// <exception cref="ArgumentOutOfRangeException">缩放倍数不是正数。</exception>
    public SimilarityTransformation2D Scale(double scale, bool isXScaleNegative, bool isYScaleNegative)
    {
        return new SimilarityTransformation2D(
            Scaling * scale.CheckPositive(),
            IsYScaleNegative ^ isXScaleNegative ^ isYScaleNegative,
            (isXScaleNegative, isYScaleNegative) switch
            {
                (true, true) => AngularMeasure.Pi + Rotation,
                (true, false) => AngularMeasure.Pi - Rotation,
                (false, true) => AngularMeasure.Tau - Rotation,
                _ => Rotation,
            },
            scale * new Vector2D(isXScaleNegative ? -Translation.X : Translation.X, isYScaleNegative ? -Translation.Y : Translation.Y));
    }

    /// <summary>
    /// 将相似变换进行旋转。
    /// </summary>
    /// <param name="rotation">旋转角度。</param>
    /// <returns>旋转后的相似变换。</returns>
    public SimilarityTransformation2D Rotate(AngularMeasure rotation)
    {
        return this with
        {
            Rotation = (Rotation + rotation).Normalized,
            Translation = Translation.Rotate(rotation),
        };
    }

    /// <summary>
    /// 将相似变换进行平移。
    /// </summary>
    /// <param name="translation">平移量。</param>
    /// <returns>平移后的相似变换。</returns>
    public SimilarityTransformation2D Translate(Vector2D translation)
    {
        return this with { Translation = Translation + translation };
    }

    /// <summary>
    /// 将点进行相似变换。
    /// </summary>
    /// <param name="point">要变换的点。</param>
    /// <returns>变换后的点。</returns>
    public Point2D Transform(Point2D point)
    {
        return (Point2D)(Transform(point.ToVector()) + Translation);
    }

    /// <summary>
    /// 将向量进行相似变换。与点的变换不同的是，向量的变换不会计算 <see cref="Translation" />。
    /// </summary>
    /// <param name="vector">要变换的向量。</param>
    /// <returns>变换后的向量。</returns>
    public Vector2D Transform(Vector2D vector)
    {
        var scaling = new Scaling2D(Scaling, IsYScaleNegative ? -Scaling : Scaling);
        return (vector * scaling).Rotate(Rotation);
    }

    /// <summary>
    /// 对指定的值进行相似变换。
    /// </summary>
    /// <typeparam name="TOut">输入值的类型。</typeparam>
    /// <typeparam name="TIn">输出值的类型。</typeparam>
    /// <param name="value">要进行相似变换的值。</param>
    /// <returns>变换后的值。</returns>
    public TOut Transform<TIn, TOut>(TIn value) where TIn : ISimilarityTransformable2D<TOut>
    {
        return value.Transform(this);
    }

    /// <summary>
    /// 计算当前相似变换的逆变换。
    /// </summary>
    /// <returns>逆变换。</returns>
    public SimilarityTransformation2D Inverse()
    {
        var scaling = 1 / Scaling;
        return new SimilarityTransformation2D(
            scaling,
            IsYScaleNegative,
            IsYScaleNegative ? Rotation : -Rotation,
            -Translation.Rotate(-Rotation) * Scaling2D.Create(scaling, IsYScaleNegative ? -scaling : scaling)
        );
    }

    /// <summary>
    /// 应用另一个相似变换到当前相似变换上。
    /// </summary>
    /// <remarks>
    /// 使用应用后的结果对目标进行变换时，相当于使用当前变换对目标进行变换，然后再使用 <paramref name="transformation" /> 对变换后的目标进行变换。
    /// </remarks>
    /// <param name="transformation">要应用的相似变换。</param>
    /// <returns>应用后的相似变换。</returns>
    public SimilarityTransformation2D Apply(SimilarityTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        var yScaleFactor = transformation.IsYScaleNegative ? -1 : 1;
        return new SimilarityTransformation2D(
            Scaling * transformation.Scaling,
            IsYScaleNegative ^ transformation.IsYScaleNegative,
            (yScaleFactor * Rotation + transformation.Rotation).Normalized,
            transformation.Transform(Translation) + transformation.Translation);
    }

    /// <summary>
    /// 将相似变换在指定的点进行旋转。
    /// </summary>
    /// <param name="rotation">旋转角度。</param>
    /// <param name="center">旋转中心。</param>
    /// <returns>旋转后的相似变换。</returns>
    public SimilarityTransformation2D RotateAt(AngularMeasure rotation, Point2D center)
    {
        var centerVector = center.ToVector();
        return this with
        {
            Rotation = Rotation + rotation,
            Translation = (Translation - centerVector).Rotate(rotation) + centerVector,
        };
    }

    /// <summary>
    /// 将相似变换在指定的点进行缩放。
    /// </summary>
    /// <param name="scale">缩放倍数。必须是正数。</param>
    /// <param name="center">缩放中心。</param>
    /// <returns>缩放后的相似变换。</returns>
    public SimilarityTransformation2D ScaleAt(double scale, Point2D center)
    {
        var centerVector = center.ToVector();
        return this with
        {
            Scaling = Scaling * scale.CheckPositive(),
            Translation = centerVector + (Translation - centerVector) * scale,
        };
    }

    /// <summary>
    /// 将相似变换在指定的点进行缩放。
    /// </summary>
    /// <param name="scale">缩放倍数。必须是正数。</param>
    /// <param name="center">缩放中心。</param>
    /// <param name="isXScaleNegative">X 轴方向是否翻转。</param>
    /// <param name="isYScaleNegative">Y 轴方向是否翻转。</param>
    /// <returns>缩放后的相似变换。</returns>
    public SimilarityTransformation2D ScaleAt(double scale, Point2D center, bool isXScaleNegative, bool isYScaleNegative)
    {
        var centerVector = center.ToVector();
        var translation = Translation - centerVector;
        return new SimilarityTransformation2D(
            Scaling * scale.CheckPositive(),
            IsYScaleNegative ^ isXScaleNegative ^ isYScaleNegative,
            (isXScaleNegative, isYScaleNegative) switch
            {
                (true, true) => AngularMeasure.Pi + Rotation,
                (true, false) => AngularMeasure.Pi - Rotation,
                (false, true) => AngularMeasure.Tau - Rotation,
                _ => Rotation,
            },
            centerVector + scale * new Vector2D(isXScaleNegative ? -translation.X : translation.X, isYScaleNegative ? -translation.Y : translation.Y));
    }

    #endregion
}

file static class ValueCheckExtensions
{
    #region 静态方法

    /// <summary>
    /// 检查值是否是正数。如果是正数，则返回原值；否则抛出 <see cref="ArgumentOutOfRangeException" /> 异常。
    /// </summary>
    /// <param name="value">参数值。</param>
    /// <param name="paramName">参数名。</param>
    /// <typeparam name="T">参数值类型。</typeparam>
    /// <returns>参数值本身。</returns>
    /// <exception cref="ArgumentOutOfRangeException">参数值不是正数。</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T CheckPositive<T>(this T value, [CallerArgumentExpression("value")] string? paramName = null)
        where T : INumber<T>
    {
        if (value <= T.Zero)
        {
            throw new ArgumentOutOfRangeException(paramName, value, "参数值必须是正数。");
        }

        return value;
    }

    #endregion
}
