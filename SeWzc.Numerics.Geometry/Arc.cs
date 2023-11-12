﻿namespace SeWzc.Numerics.Geometry;

/// <summary>
/// 圆弧。
/// </summary>
/// <param name="Circle">圆弧对应的圆。</param>
/// <param name="StartAngle">开始角。</param>
/// <param name="EndAngle">结束角。</param>
public readonly record struct Arc2D(Circle2D Circle, AngularMeasure StartAngle, AngularMeasure EndAngle)
{
    /// <summary>
    /// 开始点坐标。
    /// </summary>
    public Point2D StartPoint => Circle.GetPoint(StartAngle);

    /// <summary>
    /// 结束点坐标。
    /// </summary>
    public Point2D EndPoint => Circle.GetPoint(EndAngle);

    /// <summary>
    /// 圆心角。
    /// </summary>
    public AngularMeasure Angle => EndAngle - StartAngle;
}