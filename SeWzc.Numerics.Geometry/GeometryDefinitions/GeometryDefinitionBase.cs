using System.Collections.Immutable;

namespace SeWzc.Numerics.Geometry.GeometryDefinitions;

/// <summary>
/// 几何定义基类。
/// </summary>
public abstract class GeometryDefinitionBase
{
    #region 私有字段

    private readonly ImmutableArray<GeometryDefinitionBase> _dependencies;

    #endregion

    #region 属性

    /// <summary>
    /// 几何定义的 ID。
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 几何定义的依赖。
    /// </summary>
    public ImmutableArray<GeometryDefinitionBase> Dependencies
    {
        get => _dependencies;
        protected init
        {
            _dependencies = value;
            var maxDeep = -1;
            foreach (var dependency in _dependencies)
            {
                if (dependency.DependencyDeep > maxDeep)
                    maxDeep = dependency.DependencyDeep;
            }

            DependencyDeep = maxDeep + 1;
        }
    }

    /// <summary>
    /// 依赖深度。
    /// </summary>
    /// <remarks>
    /// 等于依赖的最大依赖深度加一。如果没有依赖，则为零。
    /// </remarks>
    public int DependencyDeep { get; private init; }

    /// <summary>
    /// 几何定义是否有效。
    /// </summary>
    /// <remarks>
    /// 有效的定义为，该几何定义是存在的。例如一个交点，如果两条线段不相交，则该交点定义无效。
    /// </remarks>
    public bool IsValid { get; private set; }

    #endregion

    #region 构造函数

    public GeometryDefinitionBase(Guid id)
    {
        Id = id;
    }

    #endregion

    #region 事件

    public event GeometryDefinitionValueChangedEventHandler? ValueChanged;

    #endregion

    #region 成员方法

    /// <summary>
    /// 更新几何定义的值。
    /// </summary>
    public void UpdateValue()
    {
        var oldIsValid = IsValid;
        // 如果不是所有依赖都有效，则该几何定义无效，无需调用 UpdateValueCore 更新值。
        if (!_dependencies.All(d => d.IsValid))
        {
            // 依赖从有效变成无效，出发值更新事件。
            if (oldIsValid)
            {
                IsValid = false;
                ValueChanged?.Invoke(this);
            }

            return;
        }

        UpdateValueCore();
        IsValid = GetNewIsValidCore();
        ValueChanged?.Invoke(this);
    }

    /// <summary>
    /// 由子类实现的更新几何定义的值的方法。
    /// </summary>
    /// <returns></returns>
    protected abstract void UpdateValueCore();

    /// <summary>
    /// 判断几何定义的依赖是否有效。
    /// </summary>
    /// <returns></returns>
    protected bool DependenciesAreValid()
    {
        return _dependencies.All(d => d.IsValid);
    }

    /// <summary>
    /// 根据当前状况判断几何定义是否有效。一般用于更新 <see cref="IsValid" />。
    /// </summary>
    /// <returns>是否有效。</returns>
    protected abstract bool GetNewIsValidCore();

    #endregion
}

public delegate void GeometryDefinitionValueChangedEventHandler(GeometryDefinitionBase sender);