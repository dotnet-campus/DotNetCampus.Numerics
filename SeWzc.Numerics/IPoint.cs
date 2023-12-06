using System.Numerics;

namespace SeWzc.Numerics;

public interface IPoint<TSelf, TVector, TNum>
    where TSelf : unmanaged, IPoint<TSelf, TVector, TNum>
    where TVector : unmanaged, IVector<TVector, TNum>
    where TNum : unmanaged, INumber<TNum>
{
    #region 运算符重载

    public static abstract TSelf operator +(TSelf point1, TSelf point2);

    public static abstract TSelf operator +(TSelf point, TVector vector);

    public static abstract TSelf operator +(TVector vector, TSelf point);

    public static abstract TVector operator -(TSelf point1, TSelf point2);

    public static abstract TSelf operator -(TSelf point, TVector vector);

    public static abstract TSelf operator *(TSelf point, double scalar);

    public static abstract TSelf operator *(double scalar, TSelf point);

    public static abstract TSelf operator /(TSelf point, double scalar);

    public static abstract explicit operator TVector(TSelf point);

    public static abstract explicit operator TSelf(TVector vector);

    #endregion
}