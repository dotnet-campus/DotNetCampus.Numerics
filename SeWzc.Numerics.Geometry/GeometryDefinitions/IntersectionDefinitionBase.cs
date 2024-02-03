namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public abstract partial class IntersectionDefinitionBase : PointDefinitionBase
{
    #region 属性

    public abstract CurveDefinitionBase Geometry1 { get; }

    public abstract CurveDefinitionBase Geometry2 { get; }

    public abstract int Index { get; }

    #endregion

    #region 构造函数

    protected IntersectionDefinitionBase(Guid id) : base(id)
    {
    }

    #endregion
}