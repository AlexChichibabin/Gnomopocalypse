using System;
using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileTrigger _projectileTrigger;

    public event Action Despawned;

    private void OnSpawned(ProjectileType projectileType)
    {
        _projectileTrigger.Init(projectileType);
        //Debug.Log("[Projectile] Spawned");
    }

    private void OnDespawned()
    {
        Debug.Log("[Projectile] Despawned");
        Despawned?.Invoke();
    }

    public class ProjectilePool : MonoMemoryPool<ProjectileType, Projectile>
    {
        protected override void Reinitialize(ProjectileType projectileType, Projectile projectile)
        {
            projectile.OnSpawned(projectileType);
        }

        protected override void OnDespawned(Projectile projectile)
        {
            projectile.OnDespawned();
            base.OnDespawned(projectile);
        }
    }
}
