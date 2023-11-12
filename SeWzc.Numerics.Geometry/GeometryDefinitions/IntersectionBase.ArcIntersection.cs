using System.Collections.Immutable;

namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

partial class IntersectionBase
{
    public sealed class ArcIntersection : IntersectionBase
    {
        private bool _isValid;

        public ArcIntersection(Guid id, ArcDefinitionBase arc1, ArcDefinitionBase arc2, int index) : base(id)
        {
            if (index is not (0 or 1))
                throw new ArgumentException("交点索引必须是 0 或 1。", nameof(index));
            Arc1 = arc1;
            Arc2 = arc2;
            Index = index;
            Dependencies = ImmutableArray.Create<GeometryDefinitionBase>(arc1, arc2);
            UpdateValue();
        }

        public ArcDefinitionBase Arc1 { get; }
        public ArcDefinitionBase Arc2 { get; }
        public int Index { get; }

        protected override void UpdateValueCore()
        {
            var arc1 = Arc1.Arc;
            var arc2 = Arc2.Arc;
            var intersection = arc1.Intersection(arc2, Index);
            _isValid = intersection.HasValue;
            Point = intersection.GetValueOrDefault();
        }

        protected override bool GetNewIsValidCore()
        {
            return _isValid && Arc1.IsValid && Arc2.IsValid;
        }
    }
}