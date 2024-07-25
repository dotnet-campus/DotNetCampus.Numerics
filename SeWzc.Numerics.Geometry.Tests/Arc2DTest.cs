using JetBrains.Annotations;
using Xunit;

namespace SeWzc.Numerics.Geometry.Tests;

[TestSubject(typeof(Arc2D))]
public class Arc2DTest
{
    #region 成员方法

    [Fact(DisplayName = "测试圆弧的创建。")]
    public void CreateTest()
    {
        var arc = new Arc2D(new Circle2D(new Point2D(3, 4), 5), AngularMeasure.FromDegree(30), AngularMeasure.FromDegree(60));
        Assert.Equal(new Point2D(3, 4), arc.Circle.Center);
        Assert.Equal(5, arc.Circle.Radius);
        Assert.Equal(AngularMeasure.FromDegree(30), arc.StartAngle);
        Assert.Equal(AngularMeasure.FromDegree(60), arc.AngleSize);
    }

    [Fact(DisplayName = "测试圆弧的开始点。")]
    public void StartPointTest()
    {
        var arc = new Arc2D(new Circle2D(new Point2D(3, 4), 5), AngularMeasure.FromDegree(30), AngularMeasure.FromDegree(60));
        var expected = new Point2D(3, 4) + AngularMeasure.FromDegree(30).UnitVector * 5;
        var actual = arc.StartPoint;
        Assert.Equal(expected.X, actual.X, (a, b) => a.IsAlmostEqual(b));
        Assert.Equal(expected.Y, actual.Y, (a, b) => a.IsAlmostEqual(b));
    }

    [Fact(DisplayName = "测试圆弧的结束点。")]
    public void EndPointTest()
    {
        var arc = new Arc2D(new Circle2D(new Point2D(3, 4), 5), AngularMeasure.FromDegree(30), AngularMeasure.FromDegree(60));
        var result = arc.EndPoint;
        Assert.Equal(3, result.X, (a, b) => a.IsAlmostEqual(b));
        Assert.Equal(9, result.Y, (a, b) => a.IsAlmostEqual(b));
    }

    [Fact(DisplayName = "测试圆弧与直线的交点。")]
    public void IntersectionWithLineTest()
    {
        var arc = new Arc2D(new Circle2D(new Point2D(3, 4), 5), AngularMeasure.FromDegree(30), AngularMeasure.FromDegree(60));
        var line = new Line2D(new Point2D(3, 4), new Vector2D(1, 1));
        var result = arc.Intersection(line, 0);
        Assert.NotNull(result);
        Assert.Equal(3 + 2.5 * double.Sqrt(2), result.Value.X, (a, b) => a.IsAlmostEqual(b));
        Assert.Equal(4 + 2.5 * double.Sqrt(2), result.Value.Y, (a, b) => a.IsAlmostEqual(b));
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
        Assert.Equal(3, result.Value.X, (a, b) => a.IsAlmostEqual(b));
        Assert.Equal(4, result.Value.Y, (a, b) => a.IsAlmostEqual(b));
        result = arc.Intersection(segment, 0);
        Assert.Null(result);
    }

    #endregion
}
