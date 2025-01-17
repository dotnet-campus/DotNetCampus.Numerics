using System.Collections.Immutable;
using System.Numerics;
using static DotNetCampus.Numerics.TrigonometricFunctionHelper;

namespace DotNetCampus.Numerics.Functions;

/// <summary>
/// 正弦函数 <c>f(x) = ScaleY * sin(NScaleX * x + NOffsetX) + OffsetY</c>。
/// </summary>
public readonly record struct SinFunction<TNum>(TNum ScaleY, TNum OffsetY, TNum NScaleX, TNum NOffsetX) : IFunction<TNum>
    where TNum : unmanaged, IFloatingPoint<TNum>, ITrigonometricFunctions<TNum>
{
    #region 属性

    /// <inheritdoc cref="IFunction{TNum}.Derivative" />
    public CosFunction<TNum> Derivative => new(-ScaleY * NScaleX, OffsetY, NScaleX, NOffsetX);

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
        return ScaleY * TNum.Sin(NScaleX * x + NOffsetX) + OffsetY;
    }

    /// <inheritdoc />
    public TNum GetMax(Interval<TNum> interval)
    {
        if (ScaleY == TNum.Zero)
            return NOffsetX;

        if (NScaleX == TNum.Zero)
            return ScaleY * TNum.Sin(NOffsetX) + OffsetY;

        if (ScaleY > TNum.Zero)
            return ScaleY * GetMaxValueForSin(NScaleX * interval + NOffsetX) + OffsetY;

        return ScaleY * GetMinValueForSin(NScaleX * interval + NOffsetX) + OffsetY;
    }

    /// <inheritdoc />
    public TNum GetMin(Interval<TNum> interval)
    {
        if (ScaleY == TNum.Zero)
            return NOffsetX;

        if (NScaleX == TNum.Zero)
            return ScaleY * TNum.Sin(NOffsetX) + OffsetY;

        if (ScaleY > TNum.Zero)
            return ScaleY * GetMinValueForSin(NScaleX * interval + NOffsetX) + OffsetY;

        return ScaleY * GetMaxValueForSin(NScaleX * interval + NOffsetX) + OffsetY;
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
