using System.Collections.Immutable;
using System.Numerics;

namespace DotNetCampus.Numerics.Functions;

/// <summary>
/// 三次函数 <c>f(x) = ax^3 + bx^2 + cx + d</c>，其中 a 不应该为零。
/// </summary>
public readonly record struct CubicFunction<TNum>(TNum A, TNum B, TNum C, TNum D) : IFunction<TNum>
    where TNum : unmanaged, IFloatingPoint<TNum>
{
    #region 属性

    /// <inheritdoc cref="IFunction{TNum}.Derivative" />
    public QuadraticFunction<TNum> Derivative => new(A.Multiply(3), B.Multiply(2), C);

    /// <inheritdoc />
    IFunction<TNum> IFunction<TNum>.Derivative => Derivative;

    #endregion

    #region 成员方法

    /// <inheritdoc />
    public ImmutableArray<TNum> GetRoots()
    {
        throw new NotImplementedException("暂时没有实现");
    }

    /// <inheritdoc />
    public TNum Evaluate(TNum x)
    {
        return ((A * x + B) * x + C) * x + D;
    }

    /// <inheritdoc />
    public TNum GetMax(Interval<TNum> interval)
    {
        var quadraticFunction = Derivative;
        var extremumPoints = quadraticFunction.GetRoots();

        var max = TNum.Max(Evaluate(interval.Start), Evaluate(interval.End));
        if (extremumPoints.Length == 0)
            return max;

        foreach (var extremumPoint in extremumPoints)
        {
            if (interval.Contains(extremumPoint))
                max = TNum.Max(max, Evaluate(extremumPoint));
        }

        return max;
    }

    /// <inheritdoc />
    public TNum GetMin(Interval<TNum> interval)
    {
        var quadraticFunction = Derivative;
        var extremumPoints = quadraticFunction.GetRoots();

        var min = TNum.Min(Evaluate(interval.Start), Evaluate(interval.End));
        if (extremumPoints.Length == 0)
            return min;

        foreach (var extremumPoint in extremumPoints)
        {
            if (interval.Contains(extremumPoint))
                min = TNum.Min(min, Evaluate(extremumPoint));
        }

        return min;
    }

    /// <inheritdoc />
    public Interval<TNum> GetValueRange(Interval<TNum> interval)
    {
        var quadraticFunction = Derivative;
        var extremumPoints = quadraticFunction.GetRoots();

        var max = TNum.Max(Evaluate(interval.Start), Evaluate(interval.End));
        var min = TNum.Min(Evaluate(interval.Start), Evaluate(interval.End));
        if (extremumPoints.Length == 0)
            return new Interval<TNum>(min, max);

        foreach (var extremumPoint in extremumPoints)
        {
            if (interval.Contains(extremumPoint))
            {
                var value = Evaluate(extremumPoint);
                max = TNum.Max(max, value);
                min = TNum.Min(min, value);
            }
        }

        return new Interval<TNum>(min, max);
    }

    #endregion
}
