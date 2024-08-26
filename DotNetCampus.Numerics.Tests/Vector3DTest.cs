using JetBrains.Annotations;
using Xunit;

namespace DotNetCampus.Numerics.Tests;

[TestSubject(typeof(Vector3D))]
public class Vector3DTest
{
    #region 成员方法

    [Theory(DisplayName = "测试向量的叉乘。")]
    [InlineData(1, 2, 3, 4, 5, 6, -3, 6, -3)]
    [InlineData(1, 2, -3, -4, 5, 6, 27, 6, 13)]
    [InlineData(1, 2, 3, -4, 5, 6, -3, -18, 13)]
    [InlineData(1, 2, -3, 4, 5, 6, 27, -18, -3)]
    [InlineData(1, 2, 3, 4, -5, -6, 3, 18, -13)]
    [InlineData(1, 2, -3, -4, -5, -6, -27, 18, 3)]
    public void CrossTest(double x1, double y1, double z1, double x2, double y2, double z2, double expectedX, double expectedY, double expectedZ)
    {
        var v1 = new Vector3D(x1, y1, z1);
        var v2 = new Vector3D(x2, y2, z2);
        var cross = v1.Cross(v2);
        Assert.Equal(expectedX, cross.X);
        Assert.Equal(expectedY, cross.Y);
        Assert.Equal(expectedZ, cross.Z);
    }

    #endregion
}