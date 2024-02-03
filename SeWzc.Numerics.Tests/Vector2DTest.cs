using JetBrains.Annotations;
using Xunit;

namespace SeWzc.Numerics.Tests;

[TestSubject(typeof(Vector2D))]
public class Vector2DTest
{
    #region 成员方法

    [Fact(DisplayName = "测试向量的加法。")]
    public void AddTest()
    {
        var v1 = new Vector2D(1, 2);
        var v2 = new Vector2D(3, 4);
        var v3 = v1 + v2;
        Assert.Equal(4, v3.X);
        Assert.Equal(6, v3.Y);
    }

    [Fact(DisplayName = "测试向量的减法。")]
    public void SubtractTest()
    {
        var v1 = new Vector2D(1, 2);
        var v2 = new Vector2D(3, 4);
        var v3 = v1 - v2;
        Assert.Equal(-2, v3.X);
        Assert.Equal(-2, v3.Y);
    }

    [Fact(DisplayName = "测试向量的数乘。")]
    public void MultiplyTest()
    {
        var v1 = new Vector2D(1, 2);
        var v2 = v1 * 3;
        Assert.Equal(3, v2.X);
        Assert.Equal(6, v2.Y);
    }

    [Fact(DisplayName = "测试向量的数除。")]
    public void DivideTest()
    {
        var v1 = new Vector2D(1, 2);
        var v2 = v1 / 2;
        Assert.Equal(0.5, v2.X);
        Assert.Equal(1, v2.Y);
    }

    [Fact(DisplayName = "测试向量的点积。")]
    public void DotTest()
    {
        var v1 = new Vector2D(1, 2);
        var v2 = new Vector2D(3, 4);
        var dot = v1.Dot(v2);
        Assert.Equal(11, dot);
    }

    #endregion
}