using UnityEngine;
using Zenject;

public class PrototypeInstaller : MonoInstaller
{
	[SerializeField] private Unit _unitPrefab;
	[SerializeField] private Projectile _projectilePrefab;
	[SerializeField] private SpawnRateConfig _spawnRateConfig;
	[SerializeField] private ShootingAnchor _shootingAnchor;

	public override void InstallBindings()
	{

		Container.Bind<SpawnRateConfig>().FromInstance(_spawnRateConfig).AsSingle();

		Container.Bind<ShootingAnchor>().FromInstance(_shootingAnchor).AsSingle();


		Container.BindMemoryPool<Unit, Unit.UnitPool>()
		.FromComponentInNewPrefab(_unitPrefab)
		.UnderTransformGroup("Units");

		if (_projectilePrefab == null || _unitPrefab == null || _spawnRateConfig == null)
			Debug.LogError("[PrototypeInstaller] link lost");

		Container.BindMemoryPool<Projectile, Projectile.ProjectilePool>()
		.FromComponentInNewPrefab(_projectilePrefab)
		.UnderTransformGroup("Projectiles");
	}
}
