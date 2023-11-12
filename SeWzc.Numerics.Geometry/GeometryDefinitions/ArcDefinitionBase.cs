namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public abstract partial class ArcDefinitionBase : GeometryDefinitionBase
{
    protected ArcDefinitionBase(Guid id) : base(id)
    {
    }

    public Arc2D Arc { get; protected set; }
}