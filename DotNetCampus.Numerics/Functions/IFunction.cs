using System.Collections.Immutable;
using System.Numerics;

namespace DotNetCampus.Numerics.Functions;

/// <summary>
/// 数学函数接口。
/// </summary>
public interface IFunction<TNum>
    where TNum : unmanaged, IFloatingPoint<TNum>
{
    #region 属性

    /// <summary>
    /// 获取函数的导数。
    /// </summary>
    IFunction<TNum> Derivative { get; }

    #endregion

    #region 成员方法

    /// <summary>
    /// 获取函数的根。如果函数没有根或者有无数根，则返回空数组。
    /// </summary>
    /// <returns>返回函数的根。</returns>
    ImmutableArray<TNum> GetRoots();

    /// <summary>
    /// 求值。
    /// </summary>
    /// <param name="x">参数值。</param>
    /// <returns>返回函数在 <paramref name="x" /> 处的值。</returns>
    TNum Evaluate(TNum x);

    /// <summary>
    /// 获取区间内的最大值。
    /// </summary>
    /// <param name="interval">区间。</param>
    /// <returns>返回区间内的最大值。</returns>
    TNum GetMax(Interval<TNum> interval);

    /// <summary>
    /// 获取区间内的最小值。
    /// </summary>
    /// <param name="interval">区间。</param>
    /// <returns>返回区间内的最小值。</returns>
    TNum GetMin(Interval<TNum> interval);

    /// <summary>
    /// 获取函数在区间内的值域。
    /// </summary>
    /// <param name="interval">区间。</param>
    /// <returns>返回函数在区间内的值域。</returns>
    Interval<TNum> GetValueRange(Interval<TNum> interval);

    #endregion
}
