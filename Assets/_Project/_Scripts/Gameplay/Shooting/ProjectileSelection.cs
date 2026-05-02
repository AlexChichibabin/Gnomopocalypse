using System.Collections.Generic;
using System;
using Zenject;

public class ProjectileSelection : IInitializable
{
    private const int StockSize = 5;

    private readonly IConfigProvider _configProvider;
    private readonly List<ProjectileConfig> _projectileStock = new();

    public IReadOnlyList<ProjectileConfig> ProjectileStock => _projectileStock;
    public event Action<IReadOnlyList<ProjectileConfig>> StockChanged;

    public ProjectileSelection(IConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }

    public void Initialize()
    {
        _projectileStock.Clear();

        for (int i = 0; i < StockSize; i++)
            AddRandomProjectile();

        StockChanged?.Invoke(_projectileStock);
    }

    public ProjectileConfig TakeBottomProjectile()
    {
        if (_projectileStock.Count == 0)
            AddRandomProjectile();

        if (_projectileStock.Count == 0)
            return null;

        ProjectileConfig projectileConfig = _projectileStock[0];
        _projectileStock.RemoveAt(0);
        AddRandomProjectile();
        StockChanged?.Invoke(_projectileStock);

        return projectileConfig;
    }

    private void AddRandomProjectile()
    {
        ProjectileConfig projectileConfig = _configProvider.GetRandomProjectileConfig();

        if (projectileConfig != null)
            _projectileStock.Add(projectileConfig);
    }
}
