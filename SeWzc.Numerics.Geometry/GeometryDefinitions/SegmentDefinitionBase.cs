namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public abstract partial class SegmentDefinitionBase : GeometryDefinitionBase
{
    protected SegmentDefinitionBase(Guid id) : base(id)
    {
    }

    public Segment2D Segment { get; protected set; }
}