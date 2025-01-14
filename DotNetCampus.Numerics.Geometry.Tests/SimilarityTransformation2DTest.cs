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

    public static readonly TheoryData<AngularMeasure, Point2D> RotateAtTestData = new()
    {
        { AngularMeasure.FromDegree(0), new Point2D(0, 0) },
        { AngularMeasure.FromDegree(90), new Point2D(0, 0) },
        { AngularMeasure.FromDegree(0), new Point2D(2, 5) },
        { AngularMeasure.FromDegree(90), new Point2D(1, 2) },
        { AngularMeasure.FromDegree(90), new Point2D(3, 4) },
        { AngularMeasure.FromDegree(15), new Point2D(5, 6) },
        { AngularMeasure.FromDegree(123456), new Point2D(7, 8) },
    };

    public static readonly TheoryData<double, Point2D> ScaleAtTestData = new()
    {
        { 1, new Point2D(0, 0) },
        { 1651357, new Point2D(0, 0) },
        { 1, new Point2D(156, 811) },
        { 2, new Point2D(1, 2) },
        { 2.4, new Point2D(1, 2) },
        { 2, new Point2D(3, 4) },
        { 3, new Point2D(5, 6) },
        { 4, new Point2D(7, 8) },
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

    [Theory(DisplayName = "测试缩放变换。")]
    [MemberData(nameof(TestData))]
    public void ScaleTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D transformed)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var transformation = similarityTransformation2D.Scale(2);

        Assert.Equal(transformed * 2, transformation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试 Y 轴方向翻转的缩放变换。")]
    [MemberData(nameof(TestData))]
    public void ScaleWithYScaleNegativeTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D transformed)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var transformation = similarityTransformation2D.Scale(2, false, true);

        Assert.Equal(new Point2D(transformed.X * 2, -transformed.Y * 2), transformation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试 X 轴方向翻转的缩放变换。")]
    [MemberData(nameof(TestData))]
    public void ScaleWithXScaleNegativeTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D transformed)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var transformation = similarityTransformation2D.Scale(2, true, false);

        Assert.Equal(new Point2D(-transformed.X * 2, transformed.Y * 2), transformation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试 X 轴和 Y 轴方向同时翻转的缩放变换。")]
    [MemberData(nameof(TestData))]
    public void ScaleWithXYScaleNegativeTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D transformed)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var transformation = similarityTransformation2D.Scale(2, true, true);

        Assert.Equal(new Point2D(-transformed.X * 2, -transformed.Y * 2), transformation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试旋转变换。")]
    [MemberData(nameof(TestData))]
    public void RotateTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D transformed)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var transformation = similarityTransformation2D.Rotate(AngularMeasure.Degree90);

        Assert.Equal(new Point2D(-transformed.Y, transformed.X), transformation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试平移变换。")]
    [MemberData(nameof(TestData))]
    public void TranslateTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D transformed)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var transformation = similarityTransformation2D.Translate(new Vector2D(1, 2));

        Assert.Equal(transformed + new Vector2D(1, 2), transformation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
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

    [Theory(DisplayName = "测试应用另一个变换。")]
    [MemberData(nameof(TestData))]
    public void ApplyTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D expected)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var newTransformation = SimilarityTransformation2D.Identity
            .Apply(new SimilarityTransformation2D(similarityTransformation2D.Scaling, false, AngularMeasure.Zero, Vector2D.Zero))
            .Apply(new SimilarityTransformation2D(1, similarityTransformation2D.IsYScaleNegative, AngularMeasure.Zero, Vector2D.Zero))
            .Apply(new SimilarityTransformation2D(1, false, similarityTransformation2D.Rotation, Vector2D.Zero))
            .Apply(new SimilarityTransformation2D(1, false, AngularMeasure.Zero, similarityTransformation2D.Translation));

        Assert.Equal(expected, newTransformation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试应用变换后再应用其逆变换。")]
    [MemberData(nameof(TestData))]
    public void ApplyInverseTest(SimilarityTransformation2D similarityTransformation2D, Point2D point, Point2D expected)
    {
        Debug.Assert(similarityTransformation2D != null, nameof(similarityTransformation2D) + " != null");

        var newTransformation = similarityTransformation2D.Apply(similarityTransformation2D).Apply(similarityTransformation2D.Inverse());

        Assert.Equal(expected, newTransformation.Transform(point), GeometryNumericsEqualHelper.IsAlmostEqual);
    }

    [Theory(DisplayName = "测试绕指定点旋转方法与平移旋转平移方法的等价性。")]
    [MemberData(nameof(RotateAtTestData))]
    public void RotateAtTest(AngularMeasure rotation, Point2D point)
    {
        var actual = SimilarityTransformation2D.Identity.RotateAt(rotation, point);
        var expected = SimilarityTransformation2D.Identity.Translate(-point.ToVector())
            .Rotate(rotation)
            .Translate(point.ToVector());

        Assert.Equal(expected.Rotation, actual.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.Translation, actual.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.Scaling, actual.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.IsYScaleNegative, actual.IsYScaleNegative);
    }

    [Theory(DisplayName = "测试在指定点缩放方法与平移缩放平移方法的等价性。")]
    [MemberData(nameof(ScaleAtTestData))]
    public void ScaleAtTest(double scaling, Point2D point)
    {
        var actual = SimilarityTransformation2D.Identity.ScaleAt(scaling, point);
        var expected = SimilarityTransformation2D.Identity.Translate(-point.ToVector())
            .Scale(scaling)
            .Translate(point.ToVector());

        Assert.Equal(expected.Rotation, actual.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.Translation, actual.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.Scaling, actual.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.IsYScaleNegative, actual.IsYScaleNegative);

        actual = SimilarityTransformation2D.Identity.ScaleAt(scaling, point, false, true);
        expected = SimilarityTransformation2D.Identity.Translate(-point.ToVector())
            .Scale(scaling, false, true)
            .Translate(point.ToVector());

        Assert.Equal(expected.Rotation, actual.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.Translation, actual.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.Scaling, actual.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.IsYScaleNegative, actual.IsYScaleNegative);

        actual = SimilarityTransformation2D.Identity.ScaleAt(scaling, point, true, false);
        expected = SimilarityTransformation2D.Identity.Translate(-point.ToVector())
            .Scale(scaling, true, false)
            .Translate(point.ToVector());

        Assert.Equal(expected.Rotation, actual.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.Translation, actual.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.Scaling, actual.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.IsYScaleNegative, actual.IsYScaleNegative);

        actual = SimilarityTransformation2D.Identity.ScaleAt(scaling, point, true, true);
        expected = SimilarityTransformation2D.Identity.Translate(-point.ToVector())
            .Scale(scaling, true, true)
            .Translate(point.ToVector());

        Assert.Equal(expected.Rotation, actual.Rotation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.Translation, actual.Translation, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.Scaling, actual.Scaling, NumericsEqualHelper.IsAlmostEqual);
        Assert.Equal(expected.IsYScaleNegative, actual.IsYScaleNegative);
    }


    #endregion
}
