public static class ProjectileConfigExtentions
{
    public static ProjectileConfig GetProjectileConfigByType(ProjectileType projectileType, IConfigProvider _configProvider)
    {
        foreach (var cfg in _configProvider.ProjectileConfigs)
        {
            if (cfg.ProjectileType == projectileType)
                return cfg;
        }
        return null;
    }
}
