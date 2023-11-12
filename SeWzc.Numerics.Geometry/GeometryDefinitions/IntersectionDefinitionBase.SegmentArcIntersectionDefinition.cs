using System.Collections.Immutable;

namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

partial class IntersectionDefinitionBase
{
    public sealed class SegmentArcIntersectionDefinition : IntersectionDefinitionBase
    {
        private bool _isValid;

        public SegmentArcIntersectionDefinition(Guid id, SegmentDefinitionBase segment, ArcDefinitionBase arc, int index) : base(id)
        {
            if (index is not (0 or 1))
                throw new ArgumentException("交点索引必须是 0 或 1。", nameof(index));
            Segment = segment;
            Arc = arc;
            Index = index;
            Dependencies = ImmutableArray.Create<GeometryDefinitionBase>(segment, arc);
            UpdateValueCore();
        }

        public SegmentDefinitionBase Segment { get; }
        public ArcDefinitionBase Arc { get; }

        public override CurveDefinitionBase Geometry1 => Segment;
        public override CurveDefinitionBase Geometry2 => Arc;
        public override int Index { get; }

        protected override void UpdateValueCore()
        {
            var segment = Segment.Segment;
            var arc = Arc.Arc;
            var intersection = arc.Intersection(segment, Index);
            _isValid = intersection.HasValue;
            Point = intersection.GetValueOrDefault();
        }

        protected override bool GetNewIsValidCore()
        {
            return _isValid && Segment.IsValid && Arc.IsValid;
        }
    }
}