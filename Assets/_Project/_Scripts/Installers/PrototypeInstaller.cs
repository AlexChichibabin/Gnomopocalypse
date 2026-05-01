using System;
using UnityEngine;
using Zenject;

public class PrototypeInstaller : MonoInstaller
{
	[SerializeField] private Unit _unitPrefab;
	[SerializeField] private Projectile _projectilePrefab;
	[SerializeField] private SpawnRateConfig _spawnRateConfig;

	public override void InstallBindings()
	{

			Container.Bind<SpawnRateConfig>().FromInstance(_spawnRateConfig).AsSingle();
			
			
			Container.BindMemoryPool<Unit, Unit.UnitPool>()
			.FromComponentInNewPrefab(_unitPrefab)
			.UnderTransformGroup("Units");

			Container.BindMemoryPool<Projectile, Projectile.ProjectilePool>()
			.FromComponentInNewPrefab(_projectilePrefab)
			.UnderTransformGroup("Projectiles");
	}
}
