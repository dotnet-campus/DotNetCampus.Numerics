using System.Numerics;

namespace DotNetCampus.Numerics.Geometry;

/// <summary>
/// 贝塞尔曲线接口。
/// </summary>
public interface IBezierCurve<TPoint, TVector, TNum> : ICurve
    where TPoint :unmanaged, IPoint<TPoint, TVector, TNum>
    where TVector : unmanaged, IVector<TVector, TNum>
    where TNum : unmanaged
#if NET8_0_OR_GREATER
    , IFloatingPoint<TNum>
#endif
{
    /// <summary>
    /// 获取曲线上的点。
    /// </summary>
    /// <param name="t">比例参数，范围为 [0, 1]。</param>
    /// <returns>曲线上的点。</returns>
    TPoint GetPoint(TNum t);

    /// <summary>
    /// 获取曲线切向量。
    /// </summary>
    /// <param name="t">比例参数，范围为 [0, 1]。</param>
    /// <returns>曲线上的切线的方向向量，指向曲线的正方向。</returns>
    TVector GetTangent(TNum t);

    /// <summary>
    /// 获取曲面法向量。
    /// </summary>
    /// <param name="t">比例参数，范围为 [0, 1]。</param>
    TVector GetNormal(TNum t);

    /// <summary>
    /// 获取曲线的曲率。
    /// </summary>
    /// <param name="t">比例参数，范围为 [0, 1]。</param>
    /// <returns>曲线的曲率。如果曲线是直线，则曲率为 0。</returns>
    double GetCurvature(TNum t);
}
