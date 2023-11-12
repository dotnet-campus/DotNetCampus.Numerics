using System.Collections.Immutable;
using System.Diagnostics;

namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public class PlaneHelper
{
    public static IEnumerable<PlaneDefinitionBase> GetPlanes(GeometryDefinitionManager manager)
    {
        var geometryDefinitions = manager.GetValidGeometryDefinitions();

        var points = new List<IntersectionNode>();
        var curveIntersectionDict = new Dictionary<CurveDefinitionBase, List<IntersectionNode>>();
        foreach (var intersectionDefinitionBase in geometryDefinitions.OfType<IntersectionDefinitionBase>())
        {
            var intersectionNode = points.FirstOrDefault(p => (intersectionDefinitionBase.Point - p.Point).SquaredLength < 1e-20);
            if (intersectionNode is null)
            {
                intersectionNode = new IntersectionNode(intersectionDefinitionBase.Point);
                points.Add(intersectionNode);
            }

            intersectionNode.Intersections.Add(intersectionDefinitionBase);

            if (!curveIntersectionDict.TryGetValue(intersectionDefinitionBase.Geometry1, out var list1))
            {
                list1 = new List<IntersectionNode>();
                curveIntersectionDict.Add(intersectionDefinitionBase.Geometry1, list1);
            }

            if (!curveIntersectionDict.TryGetValue(intersectionDefinitionBase.Geometry2, out var list2))
            {
                list2 = new List<IntersectionNode>();
                curveIntersectionDict.Add(intersectionDefinitionBase.Geometry2, list2);
            }

            list1.Add(intersectionNode);
            list2.Add(intersectionNode);
        }

        var curveNodes = GetCurves(curveIntersectionDict);
    }

    /// <summary>
    /// 获取分段的曲线并加入到交点节点中。
    /// </summary>
    /// <param name="curveIntersectionDict"></param>
    /// <returns></returns>
    private static List<CurveNode> GetCurves(IReadOnlyDictionary<CurveDefinitionBase, List<IntersectionNode>> curveIntersectionDict)
    {
        var result = new List<CurveNode>();
        foreach (var (curve, intersectionNodes) in curveIntersectionDict)
        {
            if (intersectionNodes.Count < 2)
                continue;

            var nodes = (
                from intersectionNode in intersectionNodes
                let radio = curve switch
                {
                    SegmentDefinitionBase segment => segment.Segment.Line.Projection(intersectionNode.Point) / segment.Segment.Length,
                    ArcDefinitionBase arc => arc.Arc.GetAngle(intersectionNode.Point) / arc.Arc.Angle,
                    _ => throw new ArgumentOutOfRangeException(),
                }
                orderby radio
                select intersectionNode).ToList();

            for (var i = 0; i < nodes.Count - 1; i++)
            {
                var curveNode = new CurveNode(curve, nodes[i], nodes[i + 1]);
                result.Add(curveNode);
                nodes[i].Curves.Add(curveNode);
                nodes[i + 1].Curves.Add(curveNode);
            }
        }

        return result;
    }

    private static List<PlaneDefinitionBase> getPlaneDefinitionBases(IEnumerable<CurveNode> curveNodes)
    {
        var curveNodeSet = new HashSet<CurveNode>(curveNodes);
    }

    private static IEnumerable<PlaneDefinitionBase> GetPlaneByCurveChain(ImmutableList<CurveNode> chain, IntersectionNode lastPointNode, Direction direction,
        HashSet<CurveNode> leftMode, HashSet<CurveNode> rightMode)
    {
        var head = chain[0];

        Debug.Assert(lastPointNode.Curves.Count >= 2);

        if (lastPointNode.Curves.Count <= 2)
            yield break;

        while (lastPointNode.Curves.Count == 2)
        {
            var lastCurveNode = chain[^1];
            var nextCurveNode = lastPointNode.Curves[0] == lastCurveNode ? lastPointNode.Curves[1] : lastPointNode.Curves[0];
            var nextPointNode = nextCurveNode.Point1 == lastPointNode ? nextCurveNode.Point2 : nextCurveNode.Point1;

            if (nextCurveNode == head)
            {
                // 完全只有一个环，ture 或 false 都行
                yield return CreatePlaneDefinition(chain, false);
                yield break;
            }

            chain = chain.Add(nextCurveNode);
            lastPointNode = nextPointNode;

            if (lastPointNode.Curves.Count <= 2)
                yield break;
        }

        var curveNodes = Sort(chain[^1], lastPointNode);

    }

    private static IEnumerable<CurveNode> Sort(CurveNode input, IntersectionNode intersectionNode)
    {
        var pointCircle = new Circle2D(intersectionNode.Point, 1e-2);
        var list = intersectionNode.Curves.Select(c =>
        {
            switch (c.Curve)
            {
                case SegmentDefinitionBase segmentDefinitionBase:
                {
                    var interval = Interval.Create(segmentDefinitionBase.Segment.Line.Projection(c.Point1.Point),
                        segmentDefinitionBase.Segment.Line.Projection(c.Point2.Point));

                    var intersection = pointCircle.Intersection(segmentDefinitionBase.Segment, 0);
                    if (intersection is not null && interval.Contains(segmentDefinitionBase.Segment.Line.Projection(intersection.GetValueOrDefault())))
                        return (Angle: pointCircle.GetAngle(intersection.GetValueOrDefault()), CurveNode: c);

                    intersection = pointCircle.Intersection(segmentDefinitionBase.Segment, 1);
                    if (intersection is not null && interval.Contains(segmentDefinitionBase.Segment.Line.Projection(intersection.GetValueOrDefault())))
                        return (Angle: pointCircle.GetAngle(intersection.GetValueOrDefault()), CurveNode: c);

                    Debug.Fail("不应该没有交点");
                    return default;
                }
                case ArcDefinitionBase arcDefinitionBase:
                {
                    var interval = Interval.Create(arcDefinitionBase.Arc.Circle.GetAngle(c.Point1.Point).Degree,
                        arcDefinitionBase.Arc.Circle.GetAngle(c.Point2.Point).Degree);

                    var intersection = arcDefinitionBase.Arc.Intersection(pointCircle, 0);
                    if (intersection is not null && interval.Contains(arcDefinitionBase.Arc.Circle.GetAngle(intersection.GetValueOrDefault()).Degree))
                        return (Angle: pointCircle.GetAngle(intersection.GetValueOrDefault()), CurveNode: c);

                    intersection = arcDefinitionBase.Arc.Intersection(pointCircle, 1);
                    if (intersection is not null && interval.Contains(arcDefinitionBase.Arc.Circle.GetAngle(intersection.GetValueOrDefault()).Degree))
                        return (Angle: pointCircle.GetAngle(intersection.GetValueOrDefault()), CurveNode: c);

                    Debug.Fail("不应该没有交点");
                    return default;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }).ToList();

        var baseAngle = list.First(x => x.CurveNode == input).Angle;
        return list.Where(x => x.CurveNode is not null).OrderBy(x => x.Angle - baseAngle).Skip(1).Select(x => x.CurveNode).ToList();
    }

    private static PlaneDefinitionBase CreatePlaneDefinition(ImmutableList<CurveNode> chain, bool isLeft)
    {
        var curves = new List<CurveDefinitionBase>();
        var points = new List<IntersectionDefinitionBase>();

        var startPoint = chain[0].Point1 == chain[1].Point1 || chain[0].Point1 == chain[1].Point2 ? chain[0].Point2 : chain[0].Point1;
        var nowPoint = startPoint;
        for (var i = 0; i < chain.Count; i++)
        {
            var curveNode = chain[i];
            if (curveNode.Point1 == nowPoint && isLeft)
                curveNode.IsLeftOk = true;
            else
                curveNode.IsRightOk = true;

            curves.Add(curveNode.Curve);
            var point = nowPoint.Intersections.First(p =>
                (p.Geometry1 == curveNode.Curve && p.Geometry2 == chain[new Index(i - 1)].Curve)
                || (p.Geometry2 == curveNode.Curve && p.Geometry1 == chain[new Index(i - 1)].Curve));
            points.Add(point);

            nowPoint = curveNode.Point1 == nowPoint ? curveNode.Point2 : curveNode.Point1;
        }

        return new PlaneDefinitionBase(Guid.NewGuid(), curves.ToImmutableArray(), points.ToImmutableArray());
    }

    /// <summary>
    /// 获取曲线链的方向。
    /// </summary>
    [Flags]
    private enum Direction
    {
        Left,
        Right,
    }

    private class CurveNode
    {
        public CurveNode(CurveDefinitionBase curve, IntersectionNode point1, IntersectionNode point2)
        {
            Curve = curve;
            Point1 = point1;
            Point2 = point2;
        }

        public CurveDefinitionBase Curve { get; }
        public IntersectionNode Point1 { get; }
        public IntersectionNode Point2 { get; }
        public bool IsLeftOk { get; set; }
        public bool IsRightOk { get; set; }
    }

    private class IntersectionNode
    {
        public IntersectionNode(Point2D point)
        {
            Point = point;
        }

        public Point2D Point { get; }
        public List<IntersectionDefinitionBase> Intersections { get; } = new();
        public List<CurveNode> Curves { get; } = new();
    }
}

public class PlaneDefinitionBase : GeometryDefinitionBase
{
    public PlaneDefinitionBase(Guid id, ImmutableArray<CurveDefinitionBase> Curves, ImmutableArray<IntersectionDefinitionBase> Points) : base(id)
    {
        this.Curves = Curves;
        this.Points = Points;
    }

    public ImmutableArray<CurveDefinitionBase> Curves { get; }
    public ImmutableArray<IntersectionDefinitionBase> Points { get; }

    protected override void UpdateValueCore()
    {
    }

    protected override bool GetNewIsValidCore()
    {
        return Curves.All(c => c.IsValid) && Points.All(p => p.IsValid);
    }
}