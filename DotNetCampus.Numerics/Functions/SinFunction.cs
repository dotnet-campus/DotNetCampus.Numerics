using System.Collections.Immutable;
using static DotNetCampus.Numerics.TrigonometricFunctionHelper;

namespace DotNetCampus.Numerics.Functions;

/// <summary>
/// 正弦函数 <c>f(x) = ScaleY * sin(NScaleX * x + NOffsetX) + OffsetY</c>。
/// </summary>
public readonly record struct SinFunction<TNum>(TNum ScaleY, TNum OffsetY, TNum NScaleX, TNum NOffsetX) : IFunction<TNum>
    where TNum : unmanaged
{
    #region 属性

    /// <inheritdoc cref="IFunction{TNum}.Derivative" />
    public CosFunction<TNum> Derivative => new(ScaleY.Negate().Multiply(NScaleX), OffsetY, NScaleX, NOffsetX);

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
        return ScaleY.Multiply(NScaleX.Multiply(x).Add(NOffsetX).Sin()).Add(OffsetY);
    }

    /// <inheritdoc />
    public TNum GetMax(Interval<TNum> interval)
    {
        if (ScaleY.IsEqual(default))
            return NOffsetX;

        if (NScaleX.IsEqual(default))
            return ScaleY.Multiply(NOffsetX.Sin()).Add(OffsetY);

        if (ScaleY.GreaterThan(default))
            return ScaleY.Multiply(GetMaxValueForSin(NScaleX * interval + NOffsetX)).Add(OffsetY);

        return ScaleY.Multiply(GetMinValueForSin(NScaleX * interval + NOffsetX)).Add(OffsetY);
    }

    /// <inheritdoc />
    public TNum GetMin(Interval<TNum> interval)
    {
        if (ScaleY.IsEqual(default))
            return NOffsetX;

        if (NScaleX.IsEqual(default))
            return ScaleY.Multiply(NOffsetX.Sin()).Add(OffsetY);

        if (ScaleY.GreaterThan(default))
            return ScaleY.Multiply(GetMinValueForSin(NScaleX * interval + NOffsetX)).Add(OffsetY);

        return ScaleY.Multiply(GetMaxValueForSin(NScaleX * interval + NOffsetX)).Add(OffsetY);
    }

    /// <inheritdoc />
    public Interval<TNum> GetValueRange(Interval<TNum> interval)
    {
        return new Interval<TNum>(GetMin(interval), GetMax(interval));
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"f(x) = {ScaleY} * sin({NScaleX} * x + {NOffsetX}) + {OffsetY}";
    }

    #endregion
}
