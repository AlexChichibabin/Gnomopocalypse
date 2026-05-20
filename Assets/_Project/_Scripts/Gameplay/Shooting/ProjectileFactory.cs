using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnCooldown = 1f;

    private Projectile.ProjectilePool _projectilePool;
    private ProjectileSelection _projectileSelection;
    private ShootingAnchor _shootingAnchor;
    private Shooting _currentShooting;
    private Coroutine _spawnCooldownRoutine;

    [Inject]
    private void Construct(
        Projectile.ProjectilePool projectilePool,
        ProjectileSelection projectileSelection,
        [InjectOptional] ShootingAnchor shootingAnchor)
    {
        _projectilePool = projectilePool;
        _projectileSelection = projectileSelection;
        _shootingAnchor = shootingAnchor;
    }

    void Awake()
    {
        if (_spawnPoint == null) _spawnPoint = this.transform;
    }

    private void Start()
    {
        //SpawnProjectile();
    }

    private void OnDisable()
    {
        // if (_currentShooting != null)
        //     _currentShooting.Released -= OnProjectileReleased;

        _currentShooting = null;

        if (_spawnCooldownRoutine != null)
        {
            StopCoroutine(_spawnCooldownRoutine);
            _spawnCooldownRoutine = null;
        }
    }

    public void Spawn(ProjectileConfig projectileConfig)
    {
        Projectile projectile = _projectilePool.Spawn(projectileConfig);
        projectile.transform.position = _spawnPoint.position;

        if (_shootingAnchor == null)
        {
            Debug.LogError("[ProjectileFactory] ShootingAnchor is missing");
            return;
        }

        if (projectile.TryGetComponent(out Shooting shooting))
        {
            shooting.Init(_shootingAnchor.transform, projectileConfig);
            _currentShooting = shooting;
        }
        else
        {
             Debug.LogError("[ProjectileFactory] Shooting component is missing");
        }
    }



    // private Projectile SpawnProjectile()
    // {
    //     ProjectileConfig projectileConfig = _projectileSelection.TakeBottomProjectile();

    //     if (projectileConfig == null)
    //     {
    //         Debug.LogError("[ProjectileFactory] Projectile config is missing");
    //         return null;
    //     }

    //     Projectile projectile = _projectilePool.Spawn(projectileConfig);
    //     projectile.transform.position = _spawnPoint.position;

    //     if (_shootingAnchor == null)
    //     {
    //         Debug.LogError("[ProjectileFactory] ShootingAnchor is missing");
    //         return projectile;
    //     }

    //     if (projectile.TryGetComponent(out Shooting shooting))
    //     {
    //         shooting.Init(_shootingAnchor.transform, projectileConfig);
    //         _currentShooting = shooting;
    //         _currentShooting.Released += OnProjectileReleased;
    //     }

    //     return projectile;
    // }


}
