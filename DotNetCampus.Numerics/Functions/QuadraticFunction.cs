using System.Collections.Immutable;

namespace DotNetCampus.Numerics.Functions;

/// <summary>
/// 二次函数 <c>f(x) = ax^2 + bx + c</c>，其中 a 不应该为零。
/// </summary>
public readonly record struct QuadraticFunction<TNum>(TNum A, TNum B, TNum C) : IFunction<TNum>
    where TNum : unmanaged
{
    #region 属性

    /// <inheritdoc cref="IFunction{TNum}.Derivative" />
    public LinearFunction<TNum> Derivative => new(A.Multiply(2), B);

    /// <summary>
    /// 获取二次函数的判别式。
    /// </summary>
    public TNum Discriminant => B.Multiply(B).Subtract(4.Multiply(A.Multiply(C)));

    /// <inheritdoc />
    IFunction<TNum> IFunction<TNum>.Derivative => Derivative;

    #endregion

    #region 成员方法

    /// <inheritdoc />
    public ImmutableArray<TNum> GetRoots()
    {
        var delta = Discriminant;

        if (delta.IsAlmostZero(C))
        {
            var a2 = A.Multiply(2);
            return ImmutableArray.Create(B.Negate().Divide(a2));
        }

        else if (delta.LessThan(default))
        {
            return ImmutableArray<TNum>.Empty;
        }
        else
        {
            var a2 = A.Multiply(2);
            var sqrtDelta = delta.Sqrt();
            return ImmutableArray.Create(B.Negate().Add(sqrtDelta).Divide(a2), B.Negate().Subtract(sqrtDelta).Divide(a2));
        }
    }

    /// <inheritdoc />
    public TNum Evaluate(TNum x)
    {
        return A.Multiply(x).Add(B).Multiply(x).Add(C);
    }

    /// <inheritdoc />
    public TNum GetMax(Interval<TNum> interval)
    {
        // 如果 a 为零，则为一次函数。
        if (A.IsEqual(default))
        {
            return new LinearFunction<TNum>(B, C).GetMax(interval);
        }

        // 如果 a 大于 0，则函数在端点处取得最大值。
        if (A.GreaterThan(default))
        {
            return Evaluate(interval.Start).GreaterThan(Evaluate(interval.End)) ? Evaluate(interval.Start) : Evaluate(interval.End);
        }

        // 如果 a 小于 0，则函数在顶点处取得最大值，或者在接近顶点的地方取得最大值。
        var vertexX = B.Negate().Divide(A.Multiply(2));
        return Evaluate(interval.Start.GreaterThanOrEqual(vertexX)
            ? interval.Start
            : interval.End.LessThanOrEqual(vertexX)
                ? interval.End
                : vertexX);
    }

    /// <inheritdoc />
    public TNum GetMin(Interval<TNum> interval)
    {
        // 如果 a 为零，则为一次函数。
        if (A.IsEqual(default))
        {
            return new LinearFunction<TNum>(B, C).GetMin(interval);
        }

        // 如果 a 小于 0，则函数在端点处取得最小值。
        if (A.LessThan(default))
        {
            return Evaluate(interval.Start).LessThan(Evaluate(interval.End)) ? Evaluate(interval.Start) : Evaluate(interval.End);
        }

        // 如果 a 大于 0，则函数在顶点处取得最小值，或者在接近顶点的地方取得最小值。
        var vertexX = B.Negate().Divide(A.Multiply(2));
        return Evaluate(interval.Start.LessThanOrEqual(vertexX)
            ? interval.Start
            : interval.End.GreaterThanOrEqual(vertexX)
                ? interval.End
                : vertexX);
    }

    /// <inheritdoc />
    public Interval<TNum> GetValueRange(Interval<TNum> interval)
    {
        if (A.IsEqual(default))
        {
            return new LinearFunction<TNum>(B, C).GetValueRange(interval);
        }

        var vertexX = B.Negate().Divide(A.Multiply(2));
        if (A.GreaterThan(default))
        {
            var max = FloatingPointHelper.Max(Evaluate(interval.Start), Evaluate(interval.End));
            var min = Evaluate(interval.Start.GreaterThanOrEqual(vertexX)
                ? interval.Start
                : interval.End.LessThanOrEqual(vertexX)
                    ? interval.End
                    : vertexX);
            return new Interval<TNum>(min, max);
        }
        else
        {
            var min = FloatingPointHelper.Min(Evaluate(interval.Start), Evaluate(interval.End));
            var max = Evaluate(interval.Start.LessThanOrEqual(vertexX)
                ? interval.Start
                : interval.End.GreaterThanOrEqual(vertexX)
                    ? interval.End
                    : vertexX);
            return new Interval<TNum>(min, max);
        }
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"f(x) = {A}x^2 + {B}x + {C}";
    }

    #endregion
}
