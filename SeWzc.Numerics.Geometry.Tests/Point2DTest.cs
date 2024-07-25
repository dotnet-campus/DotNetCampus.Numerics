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
        Assert.Equal(3, point.X);
        Assert.Equal(4, point.Y);
    }

    [Fact(DisplayName = "测试点的加法。")]
    public void AddTest()
    {
        var point = new Point2D(3, 4);
        var vector = new Vector2D(5, 6);

        var result = point + vector;
        Assert.Equal(8, result.X);
        Assert.Equal(10, result.Y);

        result = vector + point;
        Assert.Equal(8, result.X);
        Assert.Equal(10, result.Y);
    }

    [Fact(DisplayName = "测试点的减法。")]
    public void SubtractTest()
    {
        var point1 = new Point2D(3, 4);
        var point2 = new Point2D(5, 6);
        var result = point1 - point2;
        Assert.Equal(-2, result.X);
        Assert.Equal(-2, result.Y);
    }

    [Fact(DisplayName = "测试点的缩放。")]
    public void ScaleTest()
    {
        var point = new Point2D(3, 4);
        var result = point * 2;
        Assert.Equal(6, result.X);
        Assert.Equal(8, result.Y);

        result = 2 * point;
        Assert.Equal(6, result.X);
        Assert.Equal(8, result.Y);

        result = point / 2;
        Assert.Equal(1.5, result.X);
        Assert.Equal(2, result.Y);

        result = point / 2;
        Assert.Equal(1.5, result.X);
        Assert.Equal(2, result.Y);
    }

    [Fact(DisplayName = "测试两个点中心点的计算。")]
    public void TwoPointsMiddle()
    {
        var point1 = new Point2D(3, 4);
        var point2 = new Point2D(5, 6);
        var result = Point2D.Middle(point1, point2);
        Assert.Equal(4, result.X);
        Assert.Equal(5, result.Y);
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
        Assert.Equal(5, result.X);
        Assert.Equal(6, result.Y);
    }

    #endregion
}
