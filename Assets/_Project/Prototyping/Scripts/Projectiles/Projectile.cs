using System;
using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileType _projectileType;
    [SerializeField] private ProjectileTrigger _projectileTrigger;
    [SerializeField] private Shooting _shooting;

    private ShootingAnchor _shootingAnchor;

    public Shooting Shooting => _shooting;

    public event Action Despawned;


    [Inject]
    private void Construct(ShootingAnchor shootingAnchor)
    {
        _shootingAnchor = shootingAnchor;
    }

    void Awake() // temp for tests
    {
        OnSpawned();
    }

    private void OnSpawned()
    {
        _projectileTrigger.Init(_projectileType);
        _shooting.Init(_shootingAnchor.transform);
        Debug.Log("[Projectile] Spawned");
    }

    private void OnDespawned()
    {
        Debug.Log("[Projectile] Despawned");
        Despawned?.Invoke();
    }

    public class ProjectilePool : MonoMemoryPool<Projectile>
    {
        protected override void OnSpawned(Projectile projectile)
        {
            base.OnSpawned(projectile);
            projectile.OnSpawned();
        }

        protected override void OnDespawned(Projectile projectile)
        {
            projectile.OnDespawned();
            base.OnDespawned(projectile);
        }
    }
}
