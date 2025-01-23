using System;
using JetBrains.Annotations;
using Xunit;

namespace DotNetCampus.Numerics.Geometry.Tests;

[TestSubject(typeof(Ellipse2D))]
public class Ellipse2DTest
{
    [Fact(DisplayName = "测试椭圆的创建。")]
    public void CreateTest()
    {
        var ellipse = new Ellipse2D(new Point2D(3, 4), 5, 6, AngularMeasure.FromDegree(30));
        Assert.Equal(3, ellipse.Center.X, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(4, ellipse.Center.Y, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(6, ellipse.A, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(5, ellipse.B, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(-60, ellipse.Angle.Degree, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试椭圆的轴端点。")]
    public void AxisEndpointTest()
    {
        var ellipse = new Ellipse2D(new Point2D(3, 4), 6, 5, AngularMeasure.FromDegree(30));
        Assert.Equal(3 + 6 * Math.Cos(Math.PI / 6), ellipse.AxisEndpointA.X, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(4 + 6 * Math.Sin(Math.PI / 6), ellipse.AxisEndpointA.Y, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(3 + 5 * Math.Cos(Math.PI * 4 / 6), ellipse.AxisEndpointB.X, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(4 + 5 * Math.Sin(Math.PI * 4 / 6), ellipse.AxisEndpointB.Y, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试椭圆的变换。")]
    public void TransformTest()
    {
        var ellipse = new Ellipse2D(new Point2D(), 5, 3, AngularMeasure.Zero);
        var transformation = AffineTransformation2D.Identity
            .Scale(new Scaling2D(2, 3))
            .Rotate(AngularMeasure.FromDegree(30));
        var transformedEllipse = ellipse.Transform(transformation);
        Assert.Equal(0, transformedEllipse.Center.X);
        Assert.Equal(0, transformedEllipse.Center.Y);
        Assert.Equal(10, transformedEllipse.A, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(9, transformedEllipse.B, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(AngularMeasure.FromDegree(30), transformedEllipse.Angle, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试椭圆旋转 90 度后的轴端点。")]
    public void AxisEndpointRotate90Test()
    {
        var ellipse = new Ellipse2D(new Point2D(), 4, 6, AngularMeasure.Zero)
            .Transform(AffineTransformation2D.CreateRotation(AngularMeasure.HalfPi));

        Assert.Equal(6, ellipse.AxisEndpointA.X, NumericsEqualHelper.IsAlmostEqual);
        Assert.True(ellipse.AxisEndpointA.Y.IsAlmostZero());
        Assert.True(ellipse.AxisEndpointB.X.IsAlmostZero());
        Assert.Equal(4, ellipse.AxisEndpointB.Y, NumericsEqualHelper.IsAlmostEqual);
    }
}
