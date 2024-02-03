using System.Collections.Immutable;

namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public abstract partial class SegmentDefinitionBase
{
    #region 内部类

    public sealed class NormalSegmentDefinition : SegmentDefinitionBase
    {
        #region 属性

        public PointDefinitionBase Point1 { get; }
        public PointDefinitionBase Point2 { get; }

        #endregion

        #region 构造函数

        public NormalSegmentDefinition(Guid id, PointDefinitionBase point1, PointDefinitionBase point2) : base(id)
        {
            Point1 = point1;
            Point2 = point2;
            Dependencies = ImmutableArray.Create<GeometryDefinitionBase>(point1, point2);
            UpdateValue();
        }

        #endregion

        #region 成员方法

        protected override void UpdateValueCore()
        {
            var point1 = Point1.Point;
            var point2 = Point2.Point;
            Segment = Segment2D.Create(point1, point2);
        }

        protected override bool GetNewIsValidCore()
        {
            return Point1.IsValid && Point2.IsValid;
        }

        #endregion
    }

    #endregion
}