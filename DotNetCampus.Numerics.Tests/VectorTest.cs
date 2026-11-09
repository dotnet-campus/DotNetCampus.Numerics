using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;

namespace DotNetCampus.Numerics.Tests;

[TestSubject(typeof(IVector<,>))]
public class VectorTest
{
    #region 静态变量

    private static readonly ImmutableArray<Type[]> VectorTypes = ImmutableArray.Create<Type[]>(
        [typeof(Vector2D), typeof(double)],
        [typeof(Vector3D), typeof(double)],
        [typeof(Vector4D), typeof(double)],
        [typeof(Vector2F), typeof(float)],
        [typeof(Vector3F), typeof(float)],
        [typeof(Vector4F), typeof(float)]);

    #endregion

    #region 静态方法

    private static void Test(string methodName)
    {
        foreach (var genericTypeArguments in VectorTypes)
        {
            typeof(VectorTest)
                .GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(genericTypeArguments)
                .Invoke(null, Array.Empty<object?>());
        }
    }

    #endregion

    #region 测试方法

    [Fact(DisplayName = "测试向量的加法。")]
    public void AddTest()
    {
        Test(nameof(AddTestGeneric));
    }

    private static void AddTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged
    {
        VectorFactory<TVector, TNum>.Test((a, b) =>
        {
            var actual = a + b;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(a[i].Add(b[i]), actual[i]);
        });
    }

    [Fact(DisplayName = "测试向量的减法。")]
    public void SubtractTest()
    {
        Test(nameof(SubtractTestGeneric));
    }

    private static void SubtractTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged
    {
        VectorFactory<TVector, TNum>.Test((a, b) =>
        {
            var actual = a - b;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(a[i].Subtract(b[i]), actual[i]);
        });
    }

    [Fact(DisplayName = "测试向量的点乘。")]
    public void DotTest()
    {
        Test(nameof(DotTestGeneric));
    }

    private static void DotTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged
    {
        VectorFactory<TVector, TNum>.Test((a, b) =>
        {
            var actual = a * b;
            var sum = default(TNum);
            for (var i = 0; i < TVector.Dimension; i++)
                sum = sum.Add(a[i].Multiply(b[i]));
            Assert.Equal(sum, actual);
        });
    }

    [Fact(DisplayName = "测试向量的数乘。")]
    public void MultiplyTest()
    {
        Test(nameof(MultiplyTestGeneric));
    }

    private static void MultiplyTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged
    {
        VectorFactory<TVector, TNum>.Test((v, n) =>
        {
            var actual = v * n;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(v[i].Multiply(n), actual[i]);
            actual = n * v;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(v[i].Multiply(n), actual[i]);
        });
    }

    [Fact(DisplayName = "测试向量的数除。")]
    public void DivideTest()
    {
        Test(nameof(DivideTestGeneric));
    }

    private static void DivideTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged
    {
        VectorFactory<TVector, TNum>.Test((v, n) =>
        {
            var actual = v / n;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(v[i].Divide(n), actual[i]);
        });
    }

    [Fact(DisplayName = "测试向量的长度。")]
    public void LengthTest()
    {
        Test(nameof(LengthTestGeneric));
    }

    private static void LengthTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged
    {
        VectorFactory<TVector, TNum>.Test(v =>
        {
            var lengthSquared = v.LengthSquared;
            var length = v.Length;

            var sum = default(TNum);
            for (var i = 0; i < TVector.Dimension; i++)
                sum = sum.Add(v[i].Multiply(v[i]));

            if (typeof(TNum) == typeof(double))
            {
                Assert.Equal(Convert.ToDouble(sum, CultureInfo.InvariantCulture), Convert.ToDouble(lengthSquared, CultureInfo.InvariantCulture));
                Assert.Equal(Convert.ToDouble(sum, CultureInfo.InvariantCulture), Convert.ToDouble(length.Multiply(length), CultureInfo.InvariantCulture),
                    (a, b) => a.IsAlmostEqual(b));
            }
            else
            {
                Assert.Equal(Convert.ToSingle(sum, CultureInfo.InvariantCulture), Convert.ToSingle(lengthSquared, CultureInfo.InvariantCulture));
                Assert.Equal(Convert.ToSingle(sum, CultureInfo.InvariantCulture), Convert.ToSingle(length.Multiply(length), CultureInfo.InvariantCulture),
                    (a, b) => a.IsNearlyEqual(b));
            }
        });
    }

    [Fact(DisplayName = "测试向量的相等。")]
    public void EqualsTest()
    {
        Test(nameof(EqualsTestGeneric));
    }

    private static void EqualsTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged
    {
#pragma warning disable CS1718
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
        // ReSharper disable EqualExpressionComparison - 测试相等运算符重载。
        VectorFactory<TVector, TNum>.Test((a, b) =>
        {
            Assert.False(a == b);
            Assert.True(a != b);
        });

        VectorFactory<TVector, TNum>.Test(a =>
        {
            Assert.True(a == a);
            Assert.False(a != a);
        });
        // ReSharper disable EqualExpressionComparison
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Local
#pragma warning restore  CS1718
    }

    [Fact(DisplayName = "测试向量的归一化。")]
    public void NormalizeTest()
    {
        Test(nameof(NormalizeTestGeneric));
    }

    private static void NormalizeTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged
    {
        VectorFactory<TVector, TNum>.Test(v =>
        {
            var actual = v.Normalized;
            var length = v.Length;
            for (var i = 0; i < TVector.Dimension; i++)
            {
                var expected = v[i].Divide(length);
                if (expected.IsNaN())
                    Assert.True(actual[i].IsNaN());
                else
                    Assert.Equal(expected, actual[i]);
            }
        });
    }

    [Fact(DisplayName = "测试向量的负运算。")]
    public void UnaryNegationTest()
    {
        Test(nameof(UnaryNegationTestGeneric));
    }

    private static void UnaryNegationTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged
    {
        VectorFactory<TVector, TNum>.Test(v =>
        {
            var actual = -v;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(v[i].Negate(), actual[i]);
        });
    }

    [Fact(DisplayName = "测试向量索引超出范围。")]
    public void IndexOutRangeTest()
    {
        Test(nameof(IndexOutRangeTestGeneric));
    }

    private static void IndexOutRangeTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged
    {
        var v = new TVector();
        Assert.Throws<ArgumentOutOfRangeException>(() => v[-1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => v[TVector.Dimension]);
    }

    #endregion
}
