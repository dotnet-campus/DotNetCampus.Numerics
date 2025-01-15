namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 可进行 2 维仿射变换的对象。
/// </summary>
/// <typeparam name="T">可变换成的对象类型。</typeparam>
public interface IAffineTransformable2D<out T> : ISimilarityTransformable2D<T>
{
    #region 成员方法

    /// <summary>
    /// 进行仿射变换。
    /// </summary>
    /// <param name="transformation">要进行的仿射变换。</param>
    /// <returns>变换后的对象。</returns>
    T Transform(AffineTransformation2D transformation);

    /// <summary>
    /// 进行缩放。
    /// </summary>
    /// <param name="scaling">缩放比例。</param>
    /// <returns>变换后的对象。</returns>
    T ScaleTransform(Scaling2D scaling)
    {
        return Transform(new AffineTransformation2D(scaling.ScaleX, 0, 0, scaling.ScaleY, 0, 0));
    }

    /// <inheritdoc cref="ISimilarityTransformable2D{T}.ScaleTransform" />
    new T ScaleTransform(double scaling)
    {
        return ScaleTransform(Scaling2D.Create(scaling));
    }

    /// <inheritdoc cref="ISimilarityTransformable2D{T}.RotateTransform" />
    new T RotateTransform(AngularMeasure rotation)
    {
        var sin = rotation.Sin();
        var cos = rotation.Cos();
        return Transform(new AffineTransformation2D(cos, -sin, sin, cos, 0, 0));
    }

    /// <inheritdoc cref="ISimilarityTransformable2D{T}.TranslateTransform" />
    new T TranslateTransform(Vector2D translation)
    {
        return Transform(new AffineTransformation2D(1, 0, 0, 1, translation.X, translation.Y));
    }

    /// <summary>
    /// 进行相似变换。
    /// </summary>
    /// <remarks>
    /// 该方法会将相似变换转换为仿射变换后进行变换。
    /// </remarks>
    /// <param name="transformation">要进行的相似变换。</param>
    /// <returns>变换后的对象。</returns>
    T ISimilarityTransformable2D<T>.Transform(SimilarityTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        return Transform(transformation.ToAffineTransformation2D());
    }

    /// <inheritdoc />
    T ISimilarityTransformable2D<T>.ScaleTransform(double scaling)
    {
        return ScaleTransform(scaling);
    }

    /// <inheritdoc />
    T ISimilarityTransformable2D<T>.RotateTransform(AngularMeasure rotation)
    {
        return RotateTransform(rotation);
    }

    /// <inheritdoc />
    T ISimilarityTransformable2D<T>.TranslateTransform(Vector2D translation)
    {
        return TranslateTransform(translation);
    }

    #endregion
}
