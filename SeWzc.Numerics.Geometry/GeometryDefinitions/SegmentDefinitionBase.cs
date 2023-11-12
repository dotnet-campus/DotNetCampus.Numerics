namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public abstract partial class SegmentDefinitionBase : CurveDefinitionBase
{
    protected SegmentDefinitionBase(Guid id) : base(id)
    {
    }

    public Segment2D Segment { get; protected set; }
}