using JetBrains.Annotations;
using Xunit;

namespace SeWzc.Numerics.Geometry.Tests;

[TestSubject(typeof(Point2D))]
public class Point2DTest
{
    #region 成员方法

    [Fact(DisplayName = "测试点的创建。")]
    public void CreateTest()
    {
        var point = new Point2D(3, 4);
        Assert.Equal(new Point2D(3, 4), point, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试点的加法。")]
    public void AddTest()
    {
        var point = new Point2D(3, 4);
        var vector = new Vector2D(5, 6);

        var result = point + vector;
        Assert.Equal(new Point2D(8, 10), result, GeometryNumericsEqualHelper.IsAlmostEqual);

        result = vector + point;
        Assert.Equal(new Point2D(8, 10), result, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试点的减法。")]
    public void SubtractTest()
    {
        var point1 = new Point2D(3, 4);
        var point2 = new Point2D(5, 6);
        var result = point1 - point2;
        Assert.Equal(new Vector2D(-2, -2), result, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试点的缩放。")]
    public void ScaleTest()
    {
        var point = new Point2D(3, 4);
        var result = point * 2;
        Assert.Equal(new Point2D(6, 8), result, GeometryNumericsEqualHelper.IsAlmostEqual);

        result = 2 * point;
        Assert.Equal(new Point2D(6, 8), result, GeometryNumericsEqualHelper.IsAlmostEqual);

        result = point / 2;
        Assert.Equal(new Point2D(1.5, 2), result, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试两个点中心点的计算。")]
    public void TwoPointsMiddle()
    {
        var point1 = new Point2D(3, 4);
        var point2 = new Point2D(5, 6);
        var result = Point2D.Middle(point1, point2);
        Assert.Equal(new Point2D(4, 5), result, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试多个点中心点的计算。")]
    public void MultiPointsMiddle()
    {
        var points = new Point2D[]
        {
            new(1, 2),
            new(3, 4),
            new(5, 6),
            new(7, 8),
            new(9, 10),
        };
        var result = Point2D.Middle(points);
        Assert.Equal(new Point2D(5, 6), result, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    #endregion
}
