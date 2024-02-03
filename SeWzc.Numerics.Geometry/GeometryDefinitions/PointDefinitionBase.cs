namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public abstract class PointDefinitionBase : GeometryDefinitionBase
{
    #region 属性

    public Point2D Point { get; protected set; }

    #endregion

    #region 构造函数

    protected PointDefinitionBase(Guid id) : base(id)
    {
    }

    #endregion
}