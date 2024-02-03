namespace SeWzc.Numerics;

public static class VectorExtensions
{
    #region 静态方法

    /// <summary>
    /// 投影到另一个向量上的投影位置。
    /// </summary>
    /// <remarks>
    /// 投影位置满足：投影值乘以 <paramref name="other" /> 的单位向量等于该向量在 <paramref name="other" /> 上的投影向量。
    /// </remarks>
    /// <param name="vector">将指定的向量投影到另一个向量上。</param>
    /// <param name="other">要投影到的向量。</param>
    /// <returns>投影位置。</returns>
    public static double GetProjectionOn<TVector>(this TVector vector, TVector other)
        where TVector : unmanaged, IVector<TVector, double>
    {
        return vector * other.Normalized;
    }

    #endregion
}