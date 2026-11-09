using System.Collections.Immutable;

namespace DotNetCampus.Numerics.Functions;

/// <summary>
/// 一次方程 <c>f(x) = ax + b</c>，其中 a 不应该为零。
/// </summary>
public readonly record struct LinearFunction<TNum>(TNum A, TNum B) : IFunction<TNum>
    where TNum : unmanaged
{
    #region 属性

    /// <inheritdoc cref="IFunction{TNum}.Derivative" />
    public ConstFunction<TNum> Derivative => new(A);

    /// <summary>
    /// 获取一次方程的根。
    /// </summary>
    /// <exception cref="InvalidOperationException">一次方程没有根或者有无数根。</exception>
    /// <returns>返回一次方程的根。</returns>
    public TNum Root
    {
        get
        {
            if (A.IsEqual(default))
            {
                throw new InvalidOperationException("一次方程没有根或者有无数根。");
            }

            return B.Negate().Divide(A);
        }
    }

    /// <inheritdoc />
    IFunction<TNum> IFunction<TNum>.Derivative => Derivative;

    #endregion

    #region 成员方法

    /// <inheritdoc />
    public ImmutableArray<TNum> GetRoots()
    {
        return A.IsEqual(default) ? ImmutableArray<TNum>.Empty : ImmutableArray.Create(B.Negate().Divide(A));
    }

    /// <inheritdoc />
    public TNum Evaluate(TNum x)
    {
        return A.Multiply(x).Add(B);
    }

    /// <inheritdoc />
    public TNum GetMax(Interval<TNum> interval)
    {
        return A.GreaterThan(default) ? Evaluate(interval.End) : Evaluate(interval.Start);
    }

    /// <inheritdoc />
    public TNum GetMin(Interval<TNum> interval)
    {
        return A.GreaterThan(default) ? Evaluate(interval.Start) : Evaluate(interval.End);
    }

    /// <inheritdoc />
    public Interval<TNum> GetValueRange(Interval<TNum> interval)
    {
        return new Interval<TNum>(GetMin(interval), GetMax(interval));
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"f(x) = {A}x + {B}";
    }

    #endregion
}
