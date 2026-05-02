using System;
using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileTrigger _projectileTrigger;
    [SerializeField] private ProjectileView _projectileView;

    public event Action Despawned;

    private ProjectilePool _pool;

    private void Awake()
    {
        if (_projectileView == null)
            _projectileView = GetComponentInChildren<ProjectileView>();
    }

    private void OnSpawned(ProjectileConfig projectileConfig)
    {
        _projectileTrigger.Init(projectileConfig);
        _projectileView.Init(projectileConfig);
        //Debug.Log("[Projectile] Spawned");
    }

    public void Despawn()
    {
        _pool.Despawn(this);
    }

    private void OnDespawned()
    {
        Debug.Log("[Projectile] Despawned");
        Despawned?.Invoke();
    }

    public class ProjectilePool : MonoMemoryPool<ProjectileConfig, Projectile>
    {
        protected override void Reinitialize(ProjectileConfig projectileConfig, Projectile projectile)
        {
            projectile._pool = this;
            projectile.OnSpawned(projectileConfig);
        }

        protected override void OnDespawned(Projectile projectile)
        {
            projectile.OnDespawned();
            base.OnDespawned(projectile);
        }
    }
}
