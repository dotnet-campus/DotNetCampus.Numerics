namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 可进行 2 维相似变换的对象。
/// </summary>
/// <typeparam name="T">可变换成的对象类型。</typeparam>
public interface ISimilarityTransformable2D<out T>
{
    #region 成员方法

    /// <summary>
    /// 进行相似变换。
    /// </summary>
    /// <param name="transformation">要进行的相似变换。</param>
    /// <returns>变换后的对象。</returns>
    T Transform(SimilarityTransformation2D transformation);

    /// <summary>
    /// 进行缩放变换。缩放中心为原点。
    /// </summary>
    /// <param name="scaling">缩放比例。</param>
    /// <returns>变换后的对象。</returns>
    T ScaleTransform(double scaling);

    /// <summary>
    /// 进行旋转变换。旋转中心为原点。
    /// </summary>
    /// <param name="rotation">旋转角度。</param>
    /// <returns>变换后的对象。</returns>
    T RotateTransform(AngularMeasure rotation);

    /// <summary>
    /// 进行平移变换。
    /// </summary>
    /// <param name="translation">平移向量。</param>
    /// <returns>变换后的对象。</returns>
    T TranslateTransform(Vector2D translation);

    #endregion
}
