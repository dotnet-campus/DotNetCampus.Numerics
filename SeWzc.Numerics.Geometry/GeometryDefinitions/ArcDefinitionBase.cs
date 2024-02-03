namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public abstract partial class ArcDefinitionBase : CurveDefinitionBase
{
    #region 属性

    public Arc2D Arc { get; protected set; }

    #endregion

    #region 构造函数

    protected ArcDefinitionBase(Guid id) : base(id)
    {
    }

    #endregion
}