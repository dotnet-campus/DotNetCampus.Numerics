namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public abstract partial class SegmentDefinitionBase : CurveDefinitionBase
{
    #region 属性

    public Segment2D Segment { get; protected set; }

    #endregion

    #region 构造函数

    protected SegmentDefinitionBase(Guid id) : base(id)
    {
    }

    #endregion
}