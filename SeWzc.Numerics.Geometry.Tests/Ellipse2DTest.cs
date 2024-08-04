using System;
using JetBrains.Annotations;
using Xunit;

namespace SeWzc.Numerics.Geometry.Tests;

[TestSubject(typeof(Ellipse2D))]
public class Ellipse2DTest
{
    [Fact(DisplayName = "测试椭圆的创建。")]
    public void CreateTest()
    {
        var ellipse = new Ellipse2D(new Point2D(3, 4), 5, 6, AngularMeasure.FromDegree(30));
        Assert.Equal(3, ellipse.Center.X);
        Assert.Equal(4, ellipse.Center.Y);
        Assert.Equal(6, ellipse.A);
        Assert.Equal(5, ellipse.B);
        Assert.Equal(30, ellipse.Angle.Degree, (a, b) => a.IsAlmostEqual(b));
    }

    [Fact(DisplayName = "测试椭圆的轴端点。")]
    public void AxisEndpointTest()
    {
        var ellipse = new Ellipse2D(new Point2D(3, 4), 5, 6, AngularMeasure.FromDegree(30));
        Assert.Equal(3 + 6 * Math.Cos(Math.PI / 6), ellipse.AxisEndpointA.X, 10);
        Assert.Equal(4 + 6 * Math.Sin(Math.PI / 6), ellipse.AxisEndpointA.Y, 10);
        Assert.Equal(3 + 5 * Math.Cos(Math.PI * 4 / 6), ellipse.AxisEndpointB.X, 10);
        Assert.Equal(4 + 5 * Math.Sin(Math.PI * 4 / 6), ellipse.AxisEndpointB.Y, 10);
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
        Assert.Equal(AngularMeasure.FromDegree(30), transformedEllipse.Angle, (a, b) => a.IsAlmostEqual(b));
    }
}
