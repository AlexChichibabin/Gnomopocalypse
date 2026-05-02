using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileTrigger _projectileTrigger;
    [SerializeField] private ProjectileView _projectileView;

    public event Action Despawned;

    private const float DespawnDelay = 15f;

    private ProjectilePool _pool;
    private Coroutine _despawnRoutine;

    private void Awake()
    {
        if (_projectileView == null)
            _projectileView = GetComponentInChildren<ProjectileView>();
    }

    private void OnSpawned(ProjectileConfig projectileConfig)
    {
        StopDespawnTimer();
        _projectileTrigger.Init(projectileConfig);
        _projectileView.Init(projectileConfig);
        _despawnRoutine = StartCoroutine(DespawnAfterDelay());
        //Debug.Log("[Projectile] Spawned");
    }

    private void OnDespawned()
    {
        StopDespawnTimer();
        Debug.Log("[Projectile] Despawned");
        Despawned?.Invoke();
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(DespawnDelay);

        _pool.Despawn(this);
    }

    private void StopDespawnTimer()
    {
        if (_despawnRoutine == null)
            return;

        StopCoroutine(_despawnRoutine);
        _despawnRoutine = null;
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
