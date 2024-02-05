using System;
using System.Collections.Immutable;
using System.Numerics;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;

namespace SeWzc.Numerics.Tests;

[TestSubject(typeof(IVector<,>))]
public class VectorTest
{
    private static readonly ImmutableArray<Type[]> VectorTypes =
    [
        [typeof(Vector2D), typeof(double)],
        [typeof(Vector3D), typeof(double)],
        [typeof(Vector4D), typeof(double)],
        [typeof(Vector2F), typeof(float)],
        [typeof(Vector3F), typeof(float)],
        [typeof(Vector4F), typeof(float)],
    ];

    private void Test(string methodName)
    {
        foreach (var genericTypeArguments in VectorTypes)
        {
            typeof(VectorTest)
                .GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(genericTypeArguments)
                .Invoke(null, Array.Empty<object?>());
        }
    }

    [Fact(DisplayName = "测试向量的加法。")]
    public void AddTest()
    {
        Test(nameof(AddTestGeneric));
    }

    private static void AddTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged, INumber<TNum>
    {
        VectorFactory<TVector, TNum>.Test((a, b) =>
        {
            var actual = a + b;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(a[i] + b[i], actual[i]);
        });
    }

    [Fact(DisplayName = "测试向量的减法。")]
    public void SubtractTest()
    {
        Test(nameof(SubtractTestGeneric));
    }

    private static void SubtractTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged, INumber<TNum>
    {
        VectorFactory<TVector, TNum>.Test((a, b) =>
        {
            var actual = a - b;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(a[i] - b[i], actual[i]);
        });
    }

    [Fact(DisplayName = "测试向量的点乘。")]
    public void DotTest()
    {
        Test(nameof(DotTestGeneric));
    }

    private static void DotTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged, INumber<TNum>
    {
        VectorFactory<TVector, TNum>.Test((a, b) =>
        {
            var actual = a * b;
            var sum = TNum.Zero;
            for (var i = 0; i < TVector.Dimension; i++)
                sum += a[i] * b[i];
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
        where TNum : unmanaged, INumber<TNum>
    {
        VectorFactory<TVector, TNum>.Test((v, n) =>
        {
            var actual = v * n;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(v[i] * n, actual[i]);
            actual = n * v;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(v[i] * n, actual[i]);
        });
    }

    [Fact(DisplayName = "测试向量的数除。")]
    public void DivideTest()
    {
        Test(nameof(DivideTestGeneric));
    }

    private static void DivideTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged, INumber<TNum>
    {
        VectorFactory<TVector, TNum>.Test((v, n) =>
        {
            var actual = v / n;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(v[i] / n, actual[i]);
        });
    }

    [Fact(DisplayName = "测试向量的长度。")]
    public void LengthTest()
    {
        Test(nameof(LengthTestGeneric));
    }

    private static void LengthTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged, INumber<TNum>
    {
        VectorFactory<TVector, TNum>.Test(v =>
        {
            var squaredLength = v.SquaredLength;
            var length = v.Length;

            var sum = TNum.Zero;
            for (var i = 0; i < TVector.Dimension; i++)
                sum += v[i] * v[i];

            if (typeof(TNum) == typeof(double))
            {
                Assert.Equal(Convert.ToDouble(sum), Convert.ToDouble(squaredLength));
                Assert.Equal(Convert.ToDouble(sum), Convert.ToDouble(length * length), (a, b) => a.IsAlmostEqual(b));
            }
            else
            {
                Assert.Equal(Convert.ToSingle(sum), Convert.ToSingle(squaredLength));
                Assert.Equal(Convert.ToSingle(sum), Convert.ToSingle(length * length), (a, b) => a.IsNearlyEqual(b));
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
        where TNum : unmanaged, INumber<TNum>
    {
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
    }

    [Fact(DisplayName = "测试向量的归一化。")]
    public void NormalizeTest()
    {
        Test(nameof(NormalizeTestGeneric));
    }

    private static void NormalizeTestGeneric<TVector, TNum>()
        where TVector : unmanaged, IVector<TVector, TNum>
        where TNum : unmanaged, INumber<TNum>
    {
        VectorFactory<TVector, TNum>.Test(v =>
        {
            var actual = v.Normalized;
            var length = v.Length;
            if (length is 0)
            {
                for (var i = 0; i < TVector.Dimension; i++)
                    Assert.True(actual[i] is 0);
            }
            else
            {
                for (var i = 0; i < TVector.Dimension; i++)
                    Assert.Equal(v[i] / length, actual[i]);
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
        where TNum : unmanaged, INumber<TNum>
    {
        VectorFactory<TVector, TNum>.Test(v =>
        {
            var actual = -v;
            for (var i = 0; i < TVector.Dimension; i++)
                Assert.Equal(-v[i], actual[i]);
        });
    }
}