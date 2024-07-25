using JetBrains.Annotations;
using Xunit;

namespace SeWzc.Numerics.Geometry.Tests;

[TestSubject(typeof(Scaling2D))]
public class Scaling2DTest
{
    #region 成员方法

    [Fact(DisplayName = "测试通过XY方向的缩放创建缩放。")]
    public void CreateFromXYTest()
    {
        var scaling = Scaling2D.Create(2, 3);
        Assert.Equal(2, scaling.ScaleX);
        Assert.Equal(3, scaling.ScaleY);
    }

    [Fact(DisplayName = "测试通过缩放比例创建缩放。")]
    public void CreateFromScaleTest()
    {
        var scaling = Scaling2D.Create(2);
        Assert.Equal(2, scaling.ScaleX);
        Assert.Equal(2, scaling.ScaleY);
    }

    #endregion
}
