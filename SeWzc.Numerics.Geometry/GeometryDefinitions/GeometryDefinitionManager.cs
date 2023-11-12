namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

public class GeometryDefinitionManager
{
    private readonly Dictionary<Guid, GeometryDefinitionBase> _geometryDefinitionDictionary = new();
    private readonly HashSet<GeometryDefinitionBase> _geometryDefinitions = new();
    private readonly Dictionary<IntersectionKey, IntersectionDefinitionBase> _intersectionDictionary = new();
    private readonly HashSet<GeometryDefinitionBase> _validGeometryDefinitions = new();

    public void AddSegmentDefinition(SegmentDefinitionBase segmentDefinition)
    {
        var intersectionList = GetIntersectionDefinitions(segmentDefinition, _geometryDefinitions, _intersectionDictionary);
        AddGeometryDefinitionWithDependencies(segmentDefinition);
        foreach (var intersectionDefinition in intersectionList)
            AddGeometryDefinitionWithDependencies(intersectionDefinition);
    }

    public void AddArcDefinition(ArcDefinitionBase arcDefinition)
    {
        var intersectionList = GetIntersectionDefinitions(arcDefinition, _geometryDefinitions, _intersectionDictionary);
        AddGeometryDefinitionWithDependencies(arcDefinition);
        foreach (var intersectionDefinition in intersectionList)
            AddGeometryDefinitionWithDependencies(intersectionDefinition);
    }

    private static IReadOnlyCollection<IntersectionDefinitionBase> GetIntersectionDefinitions(
        GeometryDefinitionBase geometryDefinition,
        IEnumerable<GeometryDefinitionBase> existentGeometryDefinitions,
        IReadOnlyDictionary<IntersectionKey, IntersectionDefinitionBase> existentIntersectionDefinitions)
    {
        List<IntersectionDefinitionBase> result = new();

        foreach (var existentGeometryDefinition in existentGeometryDefinitions)
        {
            if (geometryDefinition is SegmentDefinitionBase segment1)
            {
                if (existentGeometryDefinition is SegmentDefinitionBase segment2)
                {
                    var key1 = new IntersectionKey(segment1, segment2, 0);
                    var key2 = new IntersectionKey(segment2, segment1, 0);
                    if (existentIntersectionDefinitions.ContainsKey(key1) || existentIntersectionDefinitions.ContainsKey(key2))
                        result.Add(new IntersectionDefinitionBase.SegmentIntersectionDefinition(Guid.NewGuid(), segment1, segment2));
                }
                else if (existentGeometryDefinition is ArcDefinitionBase arc2)
                {
                    var key1 = new IntersectionKey(segment1, arc2, 0);
                    var key2 = new IntersectionKey(arc2, segment1, 1);
                    if (existentIntersectionDefinitions.ContainsKey(key1) || existentIntersectionDefinitions.ContainsKey(key2))
                        result.Add(new IntersectionDefinitionBase.SegmentArcIntersectionDefinition(Guid.NewGuid(), segment1, arc2, 0));

                    var key3 = new IntersectionKey(segment1, arc2, 1);
                    var key4 = new IntersectionKey(arc2, segment1, 0);
                    if (existentIntersectionDefinitions.ContainsKey(key3) || existentIntersectionDefinitions.ContainsKey(key4))
                        result.Add(new IntersectionDefinitionBase.SegmentArcIntersectionDefinition(Guid.NewGuid(), segment1, arc2, 1));
                }
            }
            else if (geometryDefinition is ArcDefinitionBase arc1)
            {
                if (existentGeometryDefinition is SegmentDefinitionBase segment2)
                {
                    var key1 = new IntersectionKey(segment2, arc1, 0);
                    var key2 = new IntersectionKey(arc1, segment2, 1);
                    if (existentIntersectionDefinitions.ContainsKey(key1) || existentIntersectionDefinitions.ContainsKey(key2))
                        result.Add(new IntersectionDefinitionBase.SegmentArcIntersectionDefinition(Guid.NewGuid(), segment2, arc1, 0));

                    var key3 = new IntersectionKey(segment2, arc1, 1);
                    var key4 = new IntersectionKey(arc1, segment2, 0);
                    if (existentIntersectionDefinitions.ContainsKey(key3) || existentIntersectionDefinitions.ContainsKey(key4))
                        result.Add(new IntersectionDefinitionBase.SegmentArcIntersectionDefinition(Guid.NewGuid(), segment2, arc1, 1));
                }
                else if (existentGeometryDefinition is ArcDefinitionBase arc2)
                {
                    var key1 = new IntersectionKey(arc1, arc2, 0);
                    var key2 = new IntersectionKey(arc2, arc1, 1);
                    if (existentIntersectionDefinitions.ContainsKey(key1) || existentIntersectionDefinitions.ContainsKey(key2))
                        result.Add(new IntersectionDefinitionBase.ArcIntersectionDefinition(Guid.NewGuid(), arc1, arc2, 0));

                    var key3 = new IntersectionKey(arc1, arc2, 1);
                    var key4 = new IntersectionKey(arc2, arc1, 0);
                    if (existentIntersectionDefinitions.ContainsKey(key3) || existentIntersectionDefinitions.ContainsKey(key4))
                        result.Add(new IntersectionDefinitionBase.ArcIntersectionDefinition(Guid.NewGuid(), arc1, arc2, 1));
                }
            }
        }

        return result;
    }

    private void AddGeometryDefinitionWithDependencies(GeometryDefinitionBase geometryDefinition)
    {
        foreach (var dependency in geometryDefinition.Dependencies)
        {
            if (!_geometryDefinitions.Contains(dependency))
                AddGeometryDefinitionWithDependencies(dependency);
        }

        AddGeometryDefinition(geometryDefinition);
    }

    private void AddGeometryDefinition(GeometryDefinitionBase geometryDefinition)
    {
        if (_geometryDefinitions.Contains(geometryDefinition))
            throw new ArgumentException("已经添加过该几何定义。", nameof(geometryDefinition));

        _geometryDefinitions.Add(geometryDefinition);
        _geometryDefinitionDictionary.Add(geometryDefinition.Id, geometryDefinition);

        // 在值变化时，重新管理有效几何定义。
        geometryDefinition.ValueChanged += OnGeometryDefinitionValueChanged;
        if (geometryDefinition.IsValid)
            _validGeometryDefinitions.Add(geometryDefinition);

        // 如果是交点定义，添加到交点字典。
        if (geometryDefinition is IntersectionDefinitionBase intersectionDefinition)
        {
            // TODO 应该交换交点顺序看看是否已经存在，以保证交点的唯一性。
            var key = new IntersectionKey(intersectionDefinition.Geometry1, intersectionDefinition.Geometry2, intersectionDefinition.Index);
            _intersectionDictionary.Add(key, intersectionDefinition);
        }
    }

    private void OnGeometryDefinitionValueChanged(GeometryDefinitionBase geometryDefinition)
    {
        if (geometryDefinition.IsValid)
            _validGeometryDefinitions.Add(geometryDefinition);
        else
            _validGeometryDefinitions.Remove(geometryDefinition);
    }

    public IReadOnlyCollection<GeometryDefinitionBase> GetValidGeometryDefinitions()
    {
        return _validGeometryDefinitions.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<GeometryDefinitionBase> GetGeometryDefinitions()
    {
        return _geometryDefinitions.ToList().AsReadOnly();
    }

    private readonly record struct IntersectionKey(GeometryDefinitionBase Geometry1, GeometryDefinitionBase Geometry2, int Index);
}