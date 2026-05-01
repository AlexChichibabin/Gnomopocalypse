using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileType _projectileType;
    [SerializeField] private ProjectileTrigger _projectileTrigger;

    void Awake() // temp for tests
    {
        OnSpawned();
    }

    private void OnSpawned()
    {
        _projectileTrigger.Init(_projectileType);
        Debug.Log("[Projectile] Spawned");
    }

    private void OnDespawned()
    {
        Debug.Log("[Projectile] Despawned");
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
