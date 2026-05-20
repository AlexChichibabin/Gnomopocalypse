using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Zenject;

public class ProjectileSelection : IInitializable
{
    private readonly IConfigProvider _configProvider;
    private List<ProjectileConfig> _projectileStock = new();

    public IReadOnlyList<ProjectileConfig> ProjectileStock => _projectileStock;

    public event Action<IReadOnlyList<ProjectileConfig>> StockChanged;
    public event Action<IReadOnlyList<ProjectileConfig>> StockBuilt;

    public ProjectileSelection(IConfigProvider configProvider) => 
        _configProvider = configProvider;

    public void Initialize()
    {
        _projectileStock.Clear();

        FillProjectiles();
    }

    private void FillProjectiles()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        LevelConfig lvlCfg = _configProvider.GetLevel(sceneName);

        for(int i = 0; i< lvlCfg.ProjectileOnLevel.Length; i++)
        {
            _projectileStock.Add(
                ProjectileConfigExtentions.GetProjectileConfigByType(lvlCfg.ProjectileOnLevel[i], _configProvider));
        }
        StockBuilt?.Invoke(_projectileStock);
    }

    public ProjectileConfig TakeBottomProjectile()
    {
        throw new NotImplementedException();
    }
}
