using System.Collections.Immutable;
using System.Numerics;

namespace DotNetCampus.Numerics.Functions;

/// <summary>
/// 常数函数。
/// </summary>
/// <param name="Value">常数值。</param>
public readonly record struct ConstFunction<TNum>(TNum Value) : IFunction<TNum>
    where TNum : unmanaged, IFloatingPoint<TNum>
{
    #region 属性

    /// <inheritdoc cref="IFunction{TNum}.Derivative" />
    public ConstFunction<TNum> Derivative => new(TNum.Zero);

    /// <inheritdoc />
    IFunction<TNum> IFunction<TNum>.Derivative => Derivative;

    #endregion

    #region 成员方法

    /// <inheritdoc />
    public ImmutableArray<TNum> GetRoots()
    {
        return ImmutableArray<TNum>.Empty;
    }

    /// <inheritdoc />
    public TNum Evaluate(TNum x)
    {
        return Value;
    }

    /// <inheritdoc />
    public TNum GetMax(Interval<TNum> interval)
    {
        return Value;
    }

    /// <inheritdoc />
    public TNum GetMin(Interval<TNum> interval)
    {
        return Value;
    }

    /// <inheritdoc />
    public Interval<TNum> GetValueRange(Interval<TNum> interval)
    {
        return new Interval<TNum>(Value, Value);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"f(x) = {Value}";
    }

    #endregion
}
