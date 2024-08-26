using JetBrains.Annotations;
using Xunit;

namespace DotNetCampus.Numerics.Geometry.Tests;

[TestSubject(typeof(AffineTransformation2D))]
public class AffineTransformation2DTest
{
    #region 成员方法

    [Fact(DisplayName = "测试缩放变换创建。")]
    public void CreateScalingTest()
    {
        var scaling = AffineTransformation2D.CreateScaling(new Scaling2D(2, 3));
        var point = new Point2D(3, 4);
        var result = scaling.Transform(point);

        Assert.Equal(new Point2D(6, 12), result, GeometryNumericsEqualHelper.IsAlmostEqual);

        Assert.Equal(new Scaling2D(2, 3), scaling.Scaling, GeometryNumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(0, scaling.Shearing, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(AngularMeasure.Zero, scaling.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Vector2D(0, 0), scaling.Translation, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试剪切变换创建。")]
    public void CreateShearingTest()
    {
        var shearing = AffineTransformation2D.CreateShearing(2);
        var point = new Point2D(3, 4);
        var result = shearing.Transform(point);

        Assert.Equal(new Point2D(11, 4), result, GeometryNumericsEqualHelper.IsAlmostEqual);

        Assert.Equal(new Scaling2D(1, 1), shearing.Scaling, GeometryNumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(2, shearing.Shearing, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(AngularMeasure.Zero, shearing.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Vector2D(0, 0), shearing.Translation, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试旋转变换创建。")]
    public void CreateRotationTest()
    {
        var rotation = AffineTransformation2D.CreateRotation(AngularMeasure.FromDegree(90));
        var point = new Point2D(3, 4);
        var result = rotation.Transform(point);

        Assert.Equal(new Point2D(-4, 3), result, GeometryNumericsEqualHelper.IsAlmostEqual);

        Assert.Equal(new Scaling2D(1, 1), rotation.Scaling, GeometryNumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(0, rotation.Shearing, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(AngularMeasure.FromDegree(90), rotation.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Vector2D(0, 0), rotation.Translation, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试平移变换创建。")]
    public void CreateTranslationTest()
    {
        var translation = AffineTransformation2D.CreateTranslation(new Vector2D(1, 2));
        var point = new Point2D(3, 4);
        var result = translation.Transform(point);

        Assert.Equal(new Point2D(4, 6), result, GeometryNumericsEqualHelper.IsAlmostEqual);

        Assert.Equal(new Scaling2D(1, 1), translation.Scaling, GeometryNumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(0, translation.Shearing, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(AngularMeasure.Zero, translation.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Vector2D(1, 2), translation.Translation, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试仿射变换分解创建。")]
    public void DecompositionTest()
    {
        var decomposition = new AffineTransformation2DDecomposition(new Vector2D(1, 2), AngularMeasure.FromDegree(90), 2, new Scaling2D(2, 3));
        var transformation = AffineTransformation2D.Create(decomposition);
        var point = new Point2D(3, 4);
        var result = transformation.Transform(point);

        var decompose = transformation.Decompose();

        Assert.Equal(new Point2D(-11, 32), result, GeometryNumericsEqualHelper.IsAlmostEqual);

        Assert.Equal(new Scaling2D(2, 3), decompose.Scaling, GeometryNumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(2, decompose.Shearing, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(AngularMeasure.FromDegree(90), decompose.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Vector2D(1, 2), decompose.Translation, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试缩放剪切旋转平移变换。")]
    public void ScaleShearRotateTranslateTest()
    {
        var transformation = AffineTransformation2D.Identity.Scale(new Scaling2D(2, 3)).Shear(2).Rotate(AngularMeasure.FromDegree(90))
            .Translate(new Vector2D(1, 2));

        var point = new Point2D(3, 4);
        var result = transformation.Transform(point);

        var decompose = transformation.Decompose();

        Assert.Equal(new Point2D(-11, 32), result, GeometryNumericsEqualHelper.IsAlmostEqual);

        Assert.Equal(new Scaling2D(2, 3), decompose.Scaling, GeometryNumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(2, decompose.Shearing, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(AngularMeasure.FromDegree(90), decompose.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Vector2D(1, 2), decompose.Translation, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试按指定点缩放变换。")]
    public void ScaleAtTest()
    {
        var transformation = AffineTransformation2D.Identity.ScaleAt(new Scaling2D(2, 3), new Point2D(1, 2));

        var point = new Point2D(3, 4);
        var result = transformation.Transform(point);

        Assert.Equal(new Point2D(5, 8), result, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试绕指定点旋转变换。")]
    public void RotateAtTest()
    {
        var transformation = AffineTransformation2D.Identity.RotateAt(AngularMeasure.FromDegree(90), new Point2D(1, 2));

        var point = new Point2D(3, 4);
        var result = transformation.Transform(point);

        Assert.Equal(new Point2D(-1, 4), result, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试对向量进行变换。")]
    public void TransformVectorTest()
    {
        var transformation = AffineTransformation2D.Identity.Scale(new Scaling2D(2, 3)).Shear(2).Rotate(AngularMeasure.FromDegree(90))
            .Translate(new Vector2D(1, 2));
        var vector = new Vector2D(3, 4);
        var result = transformation.Transform(vector);

        Assert.Equal(new Vector2D(-12, 30), result, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试逆变换。")]
    public void InverseTest()
    {
        var transformation = AffineTransformation2D.Identity.Scale(new Scaling2D(2, 3)).Shear(2).Rotate(AngularMeasure.FromDegree(90))
            .Translate(new Vector2D(1, 2));
        var inverse = transformation.Inverse();

        var point = new Point2D(3, 4);
        var inverseResult = inverse.Transform(transformation.Transform(point));

        Assert.Equal(point, inverseResult, GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试仿射变换的叠加。")]
    public void ApplyTest()
    {
        var transformation = AffineTransformation2D.Identity
            .Apply(AffineTransformation2D.CreateScaling(new Scaling2D(2, 3)))
            .Apply(AffineTransformation2D.CreateShearing(2))
            .Apply(AffineTransformation2D.CreateRotation(AngularMeasure.FromDegree(90)))
            .Apply(AffineTransformation2D.CreateTranslation(new Vector2D(1, 2)));

        Assert.Equal(new Scaling2D(2, 3), transformation.Scaling, GeometryNumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(2, transformation.Shearing, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(AngularMeasure.FromDegree(90), transformation.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Vector2D(1, 2), transformation.Translation, NumericsEqualHelper.IsAlmostEqual);
    }

    #endregion
}
