using JetBrains.Annotations;
using Xunit;

namespace DotNetCampus.Numerics.Geometry.Tests;

[TestSubject(typeof(Segment2D))]
public class Segment2DTest
{
    #region 成员方法

    [Fact(DisplayName = "测试线段交点。")]
    public void IntersectionTest()
    {
        // Arrange
        var point1 = new Point2D(0, 0);
        var point2 = new Point2D(4, 4);
        var segment1 = Segment2D.Create(point1, point2);

        var point3 = new Point2D(0, 4);
        var point4 = new Point2D(4, 0);
        var segment2 = Segment2D.Create(point3, point4);

        var expectedIntersection = new Point2D(2, 2);

        // Act
        var intersection = segment1.Intersection(segment2);

        // Assert
        Assert.NotNull(intersection);
        Assert.Equal(expectedIntersection, intersection.Value, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试平行线段无交点。")]
    public void ParallelSegmentsNoIntersectionTest()
    {
        // Arrange
        var point1 = new Point2D(0, 0);
        var point2 = new Point2D(4, 0);
        var segment1 = Segment2D.Create(point1, point2);

        var point3 = new Point2D(0, 1);
        var point4 = new Point2D(4, 1);
        var segment2 = Segment2D.Create(point3, point4);

        // Act
        var intersection = segment1.Intersection(segment2);

        // Assert
        Assert.Null(intersection);
    }

    [Fact(DisplayName = "测试线段端点相交。")]
    public void EndpointIntersectionTest()
    {
        // Arrange
        var point1 = new Point2D(4, 8);
        var point2 = new Point2D(4, 4);
        var segment1 = Segment2D.Create(point1, point2);

        var point3 = new Point2D(4, 4);
        var point4 = new Point2D(8, 8);
        var segment2 = Segment2D.Create(point3, point4);

        var expectedIntersection = new Point2D(4, 4);

        // Act
        var intersection = segment1.Intersection(segment2);

        // Assert
        Assert.NotNull(intersection);
        Assert.Equal(expectedIntersection, intersection.Value, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    #endregion
}