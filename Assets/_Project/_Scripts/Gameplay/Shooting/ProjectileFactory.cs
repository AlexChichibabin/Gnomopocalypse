using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ProjectileFactory : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform _spawnPoint;

    private Projectile.ProjectilePool _projectilePool;
    private ProjectileSelection _projectileSelection;
    private ShootingAnchor _shootingAnchor;

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

    private Projectile SpawnProjectile()
    {
        ProjectileConfig projectileConfig = _projectileSelection.TakeBottomProjectile();

        if (projectileConfig == null)
        {
            Debug.LogError("[ProjectileFactory] Projectile config is missing");
            return null;
        }

        Projectile projectile = _projectilePool.Spawn(projectileConfig);
        projectile.transform.position = _spawnPoint.position;

        if (_shootingAnchor == null)
        {
            Debug.LogError("[ProjectileFactory] ShootingAnchor is missing");
            return projectile;
        }

        if (projectile.TryGetComponent(out Shooting shooting))
            shooting.Init(_shootingAnchor.transform, projectileConfig);

        return projectile;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SpawnProjectile();
    }

}
