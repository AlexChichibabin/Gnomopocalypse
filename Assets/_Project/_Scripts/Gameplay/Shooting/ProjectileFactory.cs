using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ProjectileFactory : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ProjectileType _projectileType;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnDelay = 1f;

    private Projectile.ProjectilePool _projectilePool;
    private ShootingAnchor _shootingAnchor;

    [Inject]
    private void Construct(Projectile.ProjectilePool projectilePool, [InjectOptional] ShootingAnchor shootingAnchor)
    {
        _projectilePool = projectilePool;
        _shootingAnchor = shootingAnchor;
    }

    void Awake()
    {
        if (_spawnPoint == null) _spawnPoint = this.transform;
    }

    private Projectile SpawnProjectile()
    {
        Projectile projectile = _projectilePool.Spawn(_projectileType);
        projectile.transform.position = _spawnPoint.position;

        if (_shootingAnchor == null)
        {
            Debug.LogError("[ProjectileFactory] ShootingAnchor is missing");
            return projectile;
        }

        if (projectile.TryGetComponent(out Shooting shooting))
            shooting.Init(_shootingAnchor.transform);

        return projectile;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SpawnProjectile();
    }

}
