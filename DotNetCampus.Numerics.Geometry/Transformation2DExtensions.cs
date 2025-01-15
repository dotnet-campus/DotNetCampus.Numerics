namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 提供 2D 变换相关的扩展方法。
/// </summary>
public static class Transformation2DExtensions
{
    #region 静态方法

    /// <summary>
    /// 将可相似变换的对象进行相似变换。
    /// </summary>
    /// <param name="this">要进行相似变换的对象。</param>
    /// <param name="transform">要进行的相似变换。</param>
    /// <typeparam name="T">要进行相似变换的对象类型。</typeparam>
    /// <returns>将对象进行相似变换后的对象。</returns>
    public static T Transform<T>(this T @this, SimilarityTransformation2D transform)
        where T : ISimilarityTransformable2D<T>
    {
        return @this.Transform(transform);
    }

    /// <summary>
    /// 将可仿射变换的对象进行仿射变换。
    /// </summary>
    /// <param name="this">要进行仿射变换的对象。</param>
    /// <param name="transform">要进行的仿射变换。</param>
    /// <typeparam name="T">要进行仿射变换的对象类型。</typeparam>
    /// <returns>将对象进行仿射变换后的对象。</returns>
    public static T Transform<T>(this T @this, AffineTransformation2D transform)
        where T : IAffineTransformable2D<T>
    {
        return @this.Transform(transform);
    }

    /// <summary>
    /// 将可相似变换的对象进行缩放变换。
    /// </summary>
    /// <param name="this">要进行缩放变换的对象。</param>
    /// <param name="scaling">缩放比例。</param>
    /// <typeparam name="T">要进行缩放变换的对象类型。</typeparam>
    /// <returns>将对象进行缩放变换后的对象。</returns>
    public static T ScaleTransform<T>(this T @this, double scaling)
        where T : ISimilarityTransformable2D<T>
    {
        return @this.ScaleTransform(scaling);
    }

    /// <summary>
    /// 将可仿射变换的对象进行缩放变换。
    /// </summary>
    /// <param name="this">要进行缩放变换的对象。</param>
    /// <param name="scaling">缩放比例。</param>
    /// <typeparam name="T">要进行缩放变换的对象类型。</typeparam>
    /// <returns>将对象进行缩放变换后的对象。</returns>
    public static T ScaleTransform<T>(this T @this, Scaling2D scaling)
        where T : IAffineTransformable2D<T>
    {
        return @this.ScaleTransform(scaling);
    }

    /// <summary>
    /// 将可相似变换的对象进行旋转变换。
    /// </summary>
    /// <param name="this">要进行旋转变换的对象。</param>
    /// <param name="rotation">旋转角度。</param>
    /// <typeparam name="T">要进行旋转变换的对象类型。</typeparam>
    /// <returns>将对象进行旋转变换后的对象。</returns>
    public static T RotateTransform<T>(this T @this, AngularMeasure rotation)
        where T : ISimilarityTransformable2D<T>
    {
        return @this.RotateTransform(rotation);
    }

    /// <summary>
    /// 将可仿射变换的对象进行平移变换。
    /// </summary>
    /// <param name="this">要进行平移变换的对象。</param>
    /// <param name="translation">平移向量。</param>
    /// <typeparam name="T">要进行平移变换的对象类型。</typeparam>
    /// <returns>将对象进行平移变换后的对象。</returns>
    public static T TranslateTransform<T>(this T @this, Vector2D translation)
        where T : ISimilarityTransformable2D<T>
    {
        return @this.TranslateTransform(translation);
    }

    /// <summary>
    /// 将可相似变换的对象进行相似变换。
    /// </summary>
    /// <param name="this">要进行相似变换的对象。</param>
    /// <param name="transform">要进行的相似变换。</param>
    /// <typeparam name="TIn">要进行相似变换的对象类型。</typeparam>
    /// <typeparam name="TOut">变换后的对象类型。</typeparam>
    /// <returns>将对象进行相似变换后的对象。</returns>
    public static TOut Transform<TIn, TOut>(this TIn @this, SimilarityTransformation2D transform)
        where TIn : ISimilarityTransformable2D<TOut>
    {
        return @this.Transform(transform);
    }

    /// <summary>
    /// 将可仿射变换的对象进行仿射变换。
    /// </summary>
    /// <param name="this">要进行仿射变换的对象。</param>
    /// <param name="transform">要进行的仿射变换。</param>
    /// <typeparam name="TIn">要进行仿射变换的对象类型。</typeparam>
    /// <typeparam name="TOut">变换后的对象类型。</typeparam>
    /// <returns>将对象进行仿射变换后的对象。</returns>
    public static TOut Transform<TIn, TOut>(this TIn @this, AffineTransformation2D transform)
        where TIn : IAffineTransformable2D<TOut>
    {
        return @this.Transform(transform);
    }

    /// <summary>
    /// 将可相似变换的对象进行缩放变换。
    /// </summary>
    /// <param name="this">要进行缩放变换的对象。</param>
    /// <param name="scaling">缩放比例。</param>
    /// <typeparam name="TIn">要进行缩放变换的对象类型。</typeparam>
    /// <typeparam name="TOut">变换后的对象类型。</typeparam>
    /// <returns>将对象进行缩放变换后的对象。</returns>
    public static TOut ScaleTransform<TIn, TOut>(this TIn @this, double scaling)
        where TIn : ISimilarityTransformable2D<TOut>
    {
        return @this.ScaleTransform(scaling);
    }

    /// <summary>
    /// 将可仿射变换的对象进行缩放变换。
    /// </summary>
    /// <param name="this">要进行缩放变换的对象。</param>
    /// <param name="scaling">缩放比例。</param>
    /// <typeparam name="TIn">要进行缩放变换的对象类型。</typeparam>
    /// <typeparam name="TOut">变换后的对象类型。</typeparam>
    /// <returns>将对象进行缩放变换后的对象。</returns>
    public static TOut ScaleTransform<TIn, TOut>(this TIn @this, Scaling2D scaling)
        where TIn : IAffineTransformable2D<TOut>
    {
        return @this.ScaleTransform(scaling);
    }

    /// <summary>
    /// 将可相似变换的对象进行旋转变换。
    /// </summary>
    /// <param name="this">要进行旋转变换的对象。</param>
    /// <param name="rotation">旋转角度。</param>
    /// <typeparam name="TIn">要进行旋转变换的对象类型。</typeparam>
    /// <typeparam name="TOut">变换后的对象类型。</typeparam>
    /// <returns>将对象进行旋转变换后的对象。</returns>
    public static TOut RotateTransform<TIn, TOut>(this TIn @this, AngularMeasure rotation)
        where TIn : ISimilarityTransformable2D<TOut>
    {
        return @this.RotateTransform(rotation);
    }

    /// <summary>
    /// 将可仿射变换的对象进行平移变换。
    /// </summary>
    /// <param name="this">要进行平移变换的对象。</param>
    /// <param name="translation">平移向量。</param>
    /// <typeparam name="TIn">要进行平移变换的对象类型。</typeparam>
    /// <typeparam name="TOut">变换后的对象类型。</typeparam>
    /// <returns>将对象进行平移变换后的对象。</returns>
    public static TOut TranslateTransform<TIn, TOut>(this TIn @this, Vector2D translation)
        where TIn : ISimilarityTransformable2D<TOut>
    {
        return @this.TranslateTransform(translation);
    }

    #endregion
}
