using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SeWzc.Numerics.Tests;

public static class VectorFactory<TVector, TNum>
    where TVector : unmanaged, IVector<TVector, TNum>
    where TNum : unmanaged, INumber<TNum>
{
    private const int Count = 1000;

    static VectorFactory()
    {
        Random random1 = new(0x79aefaa7);
        TVector[] randomVectors1 = [TVector.Zero, ..RangeSelect(_ => CreateRandomVector(random1))];
        random1.Shuffle(randomVectors1);
        RandomVectors1 = ImmutableCollectionsMarshal.AsImmutableArray(randomVectors1);

        Random random2 = new(0x43d1d9d1);
        TVector[] randomVectors2 = [TVector.Zero, ..RangeSelect(_ => CreateRandomVector(random2))];
        random2.Shuffle(randomVectors2);
        RandomVectors2 = ImmutableCollectionsMarshal.AsImmutableArray(randomVectors2);

        Random random3 = new(0x1e03afb6);
        TVector[] randomVectors3 = [TVector.Zero, ..RangeSelect(_ => CreateRandomVector(random3))];
        random3.Shuffle(randomVectors3);
        RandomVectors3 = ImmutableCollectionsMarshal.AsImmutableArray(randomVectors3);

        Random randomNum = new(0x225ca3d);
        RandomScalars = [TNum.Zero, ..Enumerable.Range(0, Count).Select(_ => NextNum(randomNum))];
    }

    private static ImmutableArray<TVector> RandomVectors1 { get; }
    private static ImmutableArray<TVector> RandomVectors2 { get; }
    private static ImmutableArray<TVector> RandomVectors3 { get; }
    private static ImmutableArray<TNum> RandomScalars { get; }

    private static IEnumerable<TVector> RangeSelect(Func<int, TVector> selector)
    {
        return Enumerable.Range(0, Count).Select(selector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TVector CreateRandomVector(Random random)
    {
        var dimension = TVector.Dimension;
        var objects = new object[dimension];
        for (var i = 0; i < dimension; i++)
            objects[i] = NextNum(random);

        return (TVector)Activator.CreateInstance(typeof(TVector), objects)!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static TNum NextNum(Random random)
    {
        if (typeof(TNum) == typeof(double))
            return Unsafe.BitCast<long, TNum>(random.NextInt64());
        if (typeof(TNum) == typeof(float))
            return Unsafe.BitCast<int, TNum>(random.Next());
        throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TVector> action)
    {
        for (var i = 0; i < Count; i++)
            action(RandomVectors1[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TVector, TNum> action)
    {
        for (var i = 0; i < Count; i++)
            action(RandomVectors1[i], RandomScalars[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TVector, TVector> action)
    {
        for (var i = 0; i < Count; i++)
            action(RandomVectors1[i], RandomVectors2[i]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Test(Action<TVector, TVector, TVector> action)
    {
        for (var i = 0; i < Count; i++)
            action(RandomVectors1[i], RandomVectors2[i], RandomVectors3[i]);
    }
}