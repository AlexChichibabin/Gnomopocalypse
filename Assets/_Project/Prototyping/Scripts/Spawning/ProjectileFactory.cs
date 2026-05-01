using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ProjectileFactory : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnDelay = 1f;

    private Projectile.ProjectilePool _projectilePool;
    //private Coroutine _spawnRoutine;


    [Inject]
    private void Construct(Projectile.ProjectilePool projectilePool)
    {
        _projectilePool = projectilePool;
    }

    void Awake()
    {
        if (_spawnPoint == null) _spawnPoint = this.transform;
    }

    //private void Start()
    //{
    //    _spawnRoutine = StartCoroutine(SpawnRoutine());
    //}

    //private IEnumerator SpawnRoutine()
    //{
    //    while (true)
    //    {
    //        Projectile projectile = SpawnProjectile();
    //
    //        yield return new WaitUntil(() => projectile.Shooting.IsMoving);
    //        yield return new WaitForSeconds(_spawnDelay);
    //    }
    //}

    private Projectile SpawnProjectile()
    {
        Projectile projectile = _projectilePool.Spawn();
        projectile.transform.position = _spawnPoint.position;

        return projectile;
    }

    //private void OnDisable()
    //{
    //    if (_spawnRoutine != null)
    //        StopCoroutine(_spawnRoutine);
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        SpawnProjectile();
    }

}
