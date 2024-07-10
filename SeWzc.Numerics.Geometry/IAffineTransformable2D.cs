namespace SeWzc.Numerics.Geometry;

public interface IAffineTransformable2D<out T>
{
    T Transform(AffineTransformation2D transformation);
}