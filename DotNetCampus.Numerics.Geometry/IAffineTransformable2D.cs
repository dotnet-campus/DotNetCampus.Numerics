namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 可进行 2 维仿射变换的对象。
/// </summary>
/// <typeparam name="T">可变换成的对象类型。</typeparam>
public interface IAffineTransformable2D<out T>
{
    /// <summary>
    /// 进行仿射变换。
    /// </summary>
    /// <param name="transformation">要进行的仿射变换。</param>
    /// <returns>变换后的对象。</returns>
    T Transform(AffineTransformation2D transformation);
}
