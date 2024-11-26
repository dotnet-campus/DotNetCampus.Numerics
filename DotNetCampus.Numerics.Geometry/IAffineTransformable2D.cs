namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 可进行 2 维仿射变换的对象。
/// </summary>
/// <typeparam name="T">可变换成的对象类型。</typeparam>
public interface IAffineTransformable2D<out T> : ISimilarityTransformable2D<T>
{
    /// <summary>
    /// 进行仿射变换。
    /// </summary>
    /// <param name="transformation">要进行的仿射变换。</param>
    /// <returns>变换后的对象。</returns>
    T Transform(AffineTransformation2D transformation);

    /// <summary>
    /// 进行相似变换。
    /// </summary>
    /// <remarks>
    /// 该方法会将相似变换转换为仿射变换后进行变换。如果对象支持相似变换，应当重写该方法以提供更高效的变换。
    /// </remarks>
    /// <param name="transformation">要进行的相似变换。</param>
    /// <returns>变换后的对象。</returns>
    T ISimilarityTransformable2D<T>.Transform(SimilarityTransformation2D transformation)
    {
        ArgumentNullException.ThrowIfNull(transformation);

        return Transform(transformation.ToAffineTransformation2D());
    }
}
