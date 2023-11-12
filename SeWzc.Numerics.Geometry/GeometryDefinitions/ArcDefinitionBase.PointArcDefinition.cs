using System.Collections.Immutable;

namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

partial class ArcDefinitionBase
{
    public sealed class PointArcDefinition : ArcDefinitionBase
    {
        public PointArcDefinition(Guid id, PointDefinitionBase Center, PointDefinitionBase StartPoint, PointDefinitionBase EndPoint) : base(id)
        {
            this.Center = Center;
            this.StartPoint = StartPoint;
            this.EndPoint = EndPoint;
            Dependencies = ImmutableArray.Create<GeometryDefinitionBase>(Center, StartPoint, EndPoint);
            UpdateValue();
        }

        public PointDefinitionBase Center { get; }
        public PointDefinitionBase StartPoint { get; }
        public PointDefinitionBase EndPoint { get; }

        protected override void UpdateValueCore()
        {
            var center = Center.Point;
            var startPoint = StartPoint.Point;
            var endPoint = EndPoint.Point;

            var circle = new Circle2D(center, (startPoint - center).Length);
            var startAngle = (startPoint - center).Angle;
            var endAngle = (endPoint - center).Angle;
            var angle = (endAngle - startAngle).Normalized;

            Arc = new Arc2D(circle, startAngle, angle);
        }

        protected override bool GetNewIsValidCore()
        {
            return Center.IsValid && StartPoint.IsValid && EndPoint.IsValid;
        }
    }
}