namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

partial class IntersectionBase
{
    public sealed class SegmentIntersection : IntersectionBase
    {
        private bool _isValid;

        public SegmentIntersection(Guid id, SegmentDefinitionBase segment1, SegmentDefinitionBase segment2) : base(id)
        {
            Segment1 = segment1;
            Segment2 = segment2;
            UpdateValue();
        }

        public SegmentDefinitionBase Segment1 { get; }
        public SegmentDefinitionBase Segment2 { get; }

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
    }
}