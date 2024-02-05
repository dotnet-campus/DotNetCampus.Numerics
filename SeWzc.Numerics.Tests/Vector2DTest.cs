using System;
using JetBrains.Annotations;
using Xunit;

namespace SeWzc.Numerics.Tests;

[TestSubject(typeof(Vector2D))]
public class Vector2DTest
{
    #region 成员方法

    [Theory(DisplayName = "测试向量的行列式。")]
    [InlineData(1, 2, 3, 4, -2)]
    [InlineData(1, 2, -3, -4, 2)]
    [InlineData(1, 2, 3, -4, -10)]
    [InlineData(1, 2, -3, 4, 10)]
    public void DetTest(double x1, double y1, double x2, double y2, double expected)
    {
        var v1 = new Vector2D(x1, y1);
        var v2 = new Vector2D(x2, y2);
        var cross = v1.Det(v2);
        Assert.Equal(expected, cross);
    }

    [Theory(DisplayName = "测试向量的夹角。")]
    [InlineData(1, 0, 0, 1, Math.PI / 2)]
    [InlineData(1, 0, 1, 0, 0)]
    [InlineData(1, 0, 1, 1, Math.PI / 4)]
    [InlineData(1, 0, -1, 0, Math.PI)]
    public void AngleBetweenTest(double x1, double y1, double x2, double y2, double expected)
    {
        var v1 = new Vector2D(x1, y1);
        var v2 = new Vector2D(x2, y2);
        var angle = v2.Angle - v1.Angle;
        Assert.Equal(expected, angle.Radian);
    }

    [Theory(DisplayName = "测试向量的法向量。")]
    [InlineData(1, 2, -2, 1)]
    [InlineData(3, 4, -4, 3)]
    [InlineData(-3, -4, 4, -3)]
    [InlineData(-1, -2, 2, -1)]
    public void NormalVectorTest(double x, double y, double expectedX, double expectedY)
    {
        var v1 = new Vector2D(x, y);
        var v2 = v1.NormalVector;
        Assert.Equal(expectedX, v2.X);
        Assert.Equal(expectedY, v2.Y);
    }

    #endregion
}