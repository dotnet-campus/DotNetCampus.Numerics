namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public abstract class PointDefinitionBase : GeometryDefinitionBase
{
    protected PointDefinitionBase(Guid id) : base(id)
    {
    }

    public Point2D Point { get; protected set; }
}