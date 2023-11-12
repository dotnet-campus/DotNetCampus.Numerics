namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public abstract partial class IntersectionDefinitionBase : PointDefinitionBase
{
    protected IntersectionDefinitionBase(Guid id) : base(id)
    {
    }

    public abstract CurveDefinitionBase Geometry1 { get; }

    public abstract CurveDefinitionBase Geometry2 { get; }

    public abstract int Index { get; }
}