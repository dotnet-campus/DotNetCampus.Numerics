namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 可进行 2 维相似变换的对象。
/// </summary>
/// <typeparam name="T">可变换成的对象类型。</typeparam>
public interface ISimilarityTransformable2D<out T>
{
    /// <summary>
    /// 进行相似变换。
    /// </summary>
    /// <param name="transformation">要进行的相似变换。</param>
    /// <returns>变换后的对象。</returns>
    T Transform(SimilarityTransformation2D transformation);
}
