using JetBrains.Annotations;
using Xunit;

namespace DotNetCampus.Numerics.Geometry.Tests;

[TestSubject(typeof(Arc2D))]
public class Arc2DTest
{
    #region 成员方法

    [Fact(DisplayName = "测试圆弧的创建。")]
    public void CreateTest()
    {
        var arc = new Arc2D(new Circle2D(new Point2D(3, 4), 5), AngularMeasure.FromDegree(30), AngularMeasure.FromDegree(60));
        Assert.Equal(new Point2D(3, 4), arc.Circle.Center, GeometryNumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(5, arc.Circle.Radius, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(AngularMeasure.FromDegree(30), arc.StartAngle, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(AngularMeasure.FromDegree(60), arc.AngleSize, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试圆弧的开始点。")]
    public void StartPointTest()
    {
        var arc = new Arc2D(new Circle2D(new Point2D(3, 4), 5), AngularMeasure.FromDegree(30), AngularMeasure.FromDegree(60));
        var expected = new Point2D(3, 4) + AngularMeasure.FromDegree(30).UnitVector * 5;
        var actual = arc.StartPoint;
        Assert.Equal(expected, actual, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试圆弧的结束点。")]
    public void EndPointTest()
    {
        var arc = new Arc2D(new Circle2D(new Point2D(3, 4), 5), AngularMeasure.FromDegree(30), AngularMeasure.FromDegree(60));
        var result = arc.EndPoint;
        Assert.Equal(new Point2D(3, 9), result, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试圆弧与直线的交点。")]
    public void IntersectionWithLineTest()
    {
        var arc = new Arc2D(new Circle2D(new Point2D(3, 4), 5), AngularMeasure.FromDegree(30), AngularMeasure.FromDegree(60));
        var line = new Line2D(new Point2D(3, 4), new Vector2D(1, 1));
        var result = arc.Intersection(line, 0);
        Assert.NotNull(result);
        Assert.Equal(new Point2D(3, 4) + AngularMeasure.FromDegree(45).UnitVector * 5, result.Value, GeometryNumericsEqualHelper.IsAlmostEqual);
        result = arc.Intersection(line, 1);
        Assert.Null(result);
    }

    [Fact(DisplayName = "测试圆弧与线段的交点。")]
    public void IntersectionWithSegmentTest()
    {
        var arc = new Arc2D(new Circle2D(new Point2D(0, 0), 5), AngularMeasure.FromDegree(30), AngularMeasure.FromDegree(60));
        var segment = new Segment2D(new Line2D(new Point2D(3, 4), new Vector2D(1, -1)), 1);
        var result = arc.Intersection(segment, 1);
        Assert.NotNull(result);
        Assert.Equal(new Point2D(3, 4), result.Value, GeometryNumericsEqualHelper.IsAlmostEqual);
        result = arc.Intersection(segment, 0);
        Assert.Null(result);
    }

    #endregion
}
