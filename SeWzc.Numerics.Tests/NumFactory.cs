using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SeWzc.Numerics.Tests;

public static class NumFactory
{
    #region 静态变量

    public const int Count = 1000;

    #endregion

    #region 静态方法

    public static IEnumerable<T> RandomCreateRange<T>(Func<Random, T> creator, Random random)
    {
        return Enumerable.Range(0, Count).Select(_ => creator(random));
    }

    public static ImmutableArray<T> RandomCreateRange<T>(IEnumerable<T> other, Func<Random, T> creator, Random random)
    {
        var array = other.Concat(RandomCreateRange(creator, random)).ToArray();
        random.Shuffle(array);
        return array.ToImmutableArray();
    }

    #endregion
}

public static class NumFactory<TNum>
    where TNum : unmanaged, INumber<TNum>
{
    #region 静态变量

    public static ImmutableArray<TNum> RandomNum { get; }

    #endregion

    #region 静态方法

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TNum NextNum(Random random)
    {
        if (typeof(TNum) == typeof(double))
            return Unsafe.BitCast<long, TNum>(random.NextInt64());
        if (typeof(TNum) == typeof(float))
            return Unsafe.BitCast<int, TNum>(random.Next());
        throw new NotSupportedException();
    }

    #endregion

    #region 构造函数

    static NumFactory()
    {
        if (typeof(TNum) != typeof(double) && typeof(TNum) != typeof(float))
            throw new NotSupportedException();

        RandomNum = NumFactory.RandomCreateRange([TNum.Zero, TNum.One], NextNum, new Random(0x225ca3d));
    }

    #endregion
}