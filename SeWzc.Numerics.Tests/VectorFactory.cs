using System;
using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SeWzc.Numerics.Tests;

internal static class VectorFactory<TVector, TNum>
    where TVector : unmanaged, IVector<TVector, TNum>
    where TNum : unmanaged, IFloatingPoint<TNum>
{
    #region 静态变量

    public static ImmutableArray<TVector> RandomVectors1 { get; } = NumFactory.RandomCreateRange([TVector.Zero], CreateRandomVector, new Random(0x79aefaa7));
    public static ImmutableArray<TVector> RandomVectors2 { get; } = NumFactory.RandomCreateRange([TVector.Zero], CreateRandomVector, new Random(0x43d1d9d1));

    #endregion

    #region 静态方法

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TVector> action)
    {
        for (var i = 0; i < NumFactory.Count; i++)
            action(RandomVectors1[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TVector, TNum> action)
    {
        for (var i = 0; i < NumFactory.Count; i++)
            action(RandomVectors1[i], NumFactory<TNum>.RandomNum[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TVector, TVector> action)
    {
        for (var i = 0; i < NumFactory.Count; i++)
            action(RandomVectors1[i], RandomVectors2[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TVector CreateRandomVector(Random random)
    {
        var dimension = TVector.Dimension;
        var objects = new object[dimension];
        for (var i = 0; i < dimension; i++)
            objects[i] = NumFactory<TNum>.NextNum(random);

        return (TVector)Activator.CreateInstance(typeof(TVector), objects)!;
    }

    #endregion
}
