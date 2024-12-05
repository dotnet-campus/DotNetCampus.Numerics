using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Xunit;

namespace DotNetCampus.Numerics.Geometry.Tests;

[TestSubject(typeof(SimilarityTransformation2D))]
public class SimilarityTransformation2DTest
{
    #region 静态变量

    public static readonly TheoryData<SimilarityTransformation2D, Point2D, Point2D> TestData = new()
    {
        // 恒等变换
        { new SimilarityTransformation2D(1, false, AngularMeasure.Zero, Vector2D.Zero), new Point2D(1, 2), new Point2D(1, 2) },
        { new SimilarityTransformation2D(1, false, AngularMeasure.Zero, Vector2D.Zero), new Point2D(3, 4), new Point2D(3, 4) },
        // 缩放变换
        { new SimilarityTransformation2D(2, false, AngularMeasure.Zero, Vector2D.Zero), new Point2D(1, 2), new Point2D(2, 4) },
        { new SimilarityTransformation2D(2, false, AngularMeasure.Zero, Vector2D.Zero), new Point2D(3, 4), new Point2D(6, 8) },
        // 带 Y 轴方向翻转的缩放变换
        { new SimilarityTransformation2D(2, true, AngularMeasure.Zero, Vector2D.Zero), new Point2D(1, 2), new Point2D(2, -4) },
        { new SimilarityTransformation2D(2, true, AngularMeasure.Zero, Vector2D.Zero), new Point2D(3, 4), new Point2D(6, -8) },
        // 旋转变换
        { new SimilarityTransformation2D(1, false, AngularMeasure.FromDegree(90), Vector2D.Zero), new Point2D(1, 2), new Point2D(-2, 1) },
        { new SimilarityTransformation2D(1, false, AngularMeasure.FromDegree(90), Vector2D.Zero), new Point2D(3, 4), new Point2D(-4, 3) },
        // 平移变换
        { new SimilarityTransformation2D(1, false, AngularMeasure.Zero, new Vector2D(1, 2)), new Point2D(1, 2), new Point2D(2, 4) },
        { new SimilarityTransformation2D(1, false, AngularMeasure.Zero, new Vector2D(1, 2)), new Point2D(3, 4), new Point2D(4, 6) },
        // 组合变换
        { new SimilarityTransformation2D(2, false, AngularMeasure.FromDegree(90), new Vector2D(1, 2)), new Point2D(1, 2), new Point2D(-3, 4) },
        { new SimilarityTransformation2D(2, false, AngularMeasure.FromDegree(90), new Vector2D(1, 2)), new Point2D(3, 4), new Point2D(-7, 8) },
        { new SimilarityTransformation2D(2, true, AngularMeasure.FromDegree(90), new Vector2D(1, 2)), new Point2D(1, 2), new Point2D(5, 4) },
        { new SimilarityTransformation2D(2, true, AngularMeasure.FromDegree(90), new Vector2D(1, 2)), new Point2D(3, 4), new Point2D(9, 8) },
    };

    #endregion

    #region 成员方法

    [Fact(DisplayName = "测试创建新的相似变换。")]
    public void NewTest()
    {
        var similarityTransformation2D = new SimilarityTransformation2D(1, AngularMeasure.Zero, Vector2D.Zero);

        Assert.Equal(1, similarityTransformation2D.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.False(similarityTransformation2D.IsYScaleNegative);
        Assert.Equal(AngularMeasure.Zero, similarityTransformation2D.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(Vector2D.Zero, similarityTransformation2D.Translation, NumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试创建缩放值不为正数的相似变换。")]
    public void NewScalingOutOfRangeTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SimilarityTransformation2D(0, false, AngularMeasure.Zero, Vector2D.Zero));
        Assert.Throws<ArgumentOutOfRangeException>(() => new SimilarityTransformation2D(-1, false, AngularMeasure.Zero, Vector2D.Zero));
    }

    [Fact(DisplayName = "测试恒等变换。")]
    public void IdentityTest()
    {
        var identity = SimilarityTransformation2D.Identity;
        var point = new Point2D(1, 2);

        Assert.Equal(1, identity.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.False(identity.IsYScaleNegative);
        Assert.Equal(AngularMeasure.Zero, identity.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(Vector2D.Zero, identity.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(point, identity.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试缩放变换。")]
    public void ScaleTest()
    {
        var scaling = SimilarityTransformation2D.Identity.Scale(2);
        var point = new Point2D(1, 2);

        Assert.Equal(2, scaling.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.False(scaling.IsYScaleNegative);
        Assert.Equal(AngularMeasure.Zero, scaling.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(Vector2D.Zero, scaling.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Point2D(2, 4), scaling.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试 Y 轴方向翻转的缩放变换。")]
    public void ScaleWithYScaleNegativeTest()
    {
        var scaling = SimilarityTransformation2D.Identity.Scale(2, false, true);
        var point = new Point2D(1, 2);

        Assert.Equal(2, scaling.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.True(scaling.IsYScaleNegative);
        Assert.Equal(AngularMeasure.Zero, scaling.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(Vector2D.Zero, scaling.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Point2D(2, -4), scaling.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试 X 轴方向翻转的缩放变换。")]
    public void ScaleWithXScaleNegativeTest()
    {
        var scaling = SimilarityTransformation2D.Identity.Scale(2, true, false);
        var point = new Point2D(1, 2);

        Assert.Equal(2, scaling.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.True(scaling.IsYScaleNegative);
        Assert.Equal(AngularMeasure.Pi, scaling.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(Vector2D.Zero, scaling.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Point2D(-2, 4), scaling.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试 X 轴和 Y 轴方向同时翻转的缩放变换。")]
    public void ScaleWithXYScaleNegativeTest()
    {
        var scaling = SimilarityTransformation2D.Identity.Scale(2, true, true);
        var point = new Point2D(1, 2);

        Assert.Equal(2, scaling.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.False(scaling.IsYScaleNegative);
        Assert.Equal(AngularMeasure.Pi, scaling.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(Vector2D.Zero, scaling.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Point2D(-2, -4), scaling.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }


    [Fact(DisplayName = "测试旋转变换。")]
    public void RotateTest()
    {
        var rotation = new SimilarityTransformation2D(1, false, AngularMeasure.FromDegree(90), Vector2D.Zero);
        var point = new Point2D(1, 2);

        Assert.Equal(1, rotation.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.False(rotation.IsYScaleNegative);
        Assert.Equal(AngularMeasure.FromDegree(90), rotation.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(Vector2D.Zero, rotation.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Point2D(-2, 1), rotation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试平移变换。")]
    public void TranslateTest()
    {
        var translation = new SimilarityTransformation2D(1, false, AngularMeasure.Zero, new Vector2D(1, 2));
        var point = new Point2D(1, 2);

        Assert.Equal(1, translation.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.False(translation.IsYScaleNegative);
        Assert.Equal(AngularMeasure.Zero, translation.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Vector2D(1, 2), translation.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(new Point2D(2, 4), translation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Fact(DisplayName = "测试通过 ISimilarityTransformable2D 接口进行变换。")]
    public void TransformByISimilarityTransformable2DTest()
    {
        var point = new Point2D(1, 2);
        var similarityTransformation2D = new SimilarityTransformation2D(2, false, AngularMeasure.FromDegree(90), new Vector2D(1, 2));

        Assert.Equal(new Point2D(-3, 4), similarityTransformation2D.Transform<Point2D, Point2D>(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试通过缩放、旋转和平移创建的变换。")]
    [MemberData(nameof(TestData))]
    public void ScaleRotateTranslateTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D expected)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var newTransformation = SimilarityTransformation2D.Identity
            .Scale(similarityTransformation2D.Scaling, false, similarityTransformation2D.IsYScaleNegative)
            .Rotate(similarityTransformation2D.Rotation)
            .Translate(similarityTransformation2D.Translation);

        Assert.Equal(expected, newTransformation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试变换。")]
    [MemberData(nameof(TestData))]
    public void TransformTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D expected)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        Assert.Equal(expected, similarityTransformation2D.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试转换为仿射变换。")]
    [MemberData(nameof(TestData))]
    public void ToAffineTransformation2DTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D expected)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var affineTransformation2D = similarityTransformation2D.ToAffineTransformation2D();
        Assert.Equal(expected, affineTransformation2D.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试逆变换。")]
    [MemberData(nameof(TestData))]
    public void InverseTest(SimilarityTransformation2D similarityTransformation2D, Point2D original, Point2D transformed)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var inverse = similarityTransformation2D.Inverse();
        Assert.Equal(original, inverse.Transform(transformed), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    #endregion
}
