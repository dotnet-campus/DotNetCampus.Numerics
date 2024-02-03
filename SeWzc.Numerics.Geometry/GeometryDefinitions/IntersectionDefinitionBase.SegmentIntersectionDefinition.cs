namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

partial class IntersectionDefinitionBase
{
    #region 内部类

    public sealed class SegmentIntersectionDefinition : IntersectionDefinitionBase
    {
        #region 私有字段

        private bool _isValid;

        #endregion

        #region 属性

        public SegmentDefinitionBase Segment1 { get; }
        public SegmentDefinitionBase Segment2 { get; }

        public override CurveDefinitionBase Geometry1 => Segment1;
        public override CurveDefinitionBase Geometry2 => Segment2;
        public override int Index => 0;

        #endregion

        #region 构造函数

        public SegmentIntersectionDefinition(Guid id, SegmentDefinitionBase segment1, SegmentDefinitionBase segment2) : base(id)
        {
            Segment1 = segment1;
            Segment2 = segment2;
            UpdateValue();
        }

        #endregion

        #region 成员方法

        protected override void UpdateValueCore()
        {
            var segment1 = Segment1.Segment;
            var segment2 = Segment2.Segment;
            var intersection = segment1.Intersection(segment2);
            _isValid = intersection.HasValue;
            Point = intersection.GetValueOrDefault();
        }

        protected override bool GetNewIsValidCore()
        {
            return _isValid && Segment1.IsValid && Segment2.IsValid;
        }

        #endregion
    }

    #endregion
}