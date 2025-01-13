namespace DotNetCampus.Numerics;

/// <summary>
/// 舍入模式。主要用于取代 <see cref="MidpointRounding" />。
/// </summary>
public enum RoundMode
{
    /// <summary>
    /// 舍入到最近的整数，如果有两个整数同样接近，则舍入到其中的偶数。也叫银行家舍入法。
    /// </summary>
    HalfToEven,

    /// <summary>
    /// 舍入到最近的整数，如果有两个整数同样接近，则舍入到离零更远的数。即四舍五入。
    /// </summary>
    HalfAwayFromZero,

    /// <summary>
    /// 舍入到最近的整数，如果有两个整数同样接近，则舍入到离零更近的数。
    /// </summary>
    HalfToZero,

    /// <summary>
    /// 舍入到最近的整数，如果有两个整数同样接近，则舍入到值更大的数。
    /// </summary>
    HalfUp,

    /// <summary>
    /// 舍入到最近的整数，如果有两个整数同样接近，则舍入到值更小的数。
    /// </summary>
    HalfDown,

    /// <summary>
    /// 舍入到最近的远离零的整数，如果自己已经是整数，则不变。
    /// </summary>
    DirectAwayFromZero,

    /// <summary>
    /// 舍入到最近的靠近零的整数，如果自己已经是整数，则不变。
    /// </summary>
    DirectToZero,

    /// <summary>
    /// 舍入到最近的更大的整数，如果自己已经是整数，则不变。
    /// </summary>
    DirectUp,

    /// <summary>
    /// 舍入到最近的更小的整数，如果自己已经是整数，则不变。
    /// </summary>
    DirectDown,
}
