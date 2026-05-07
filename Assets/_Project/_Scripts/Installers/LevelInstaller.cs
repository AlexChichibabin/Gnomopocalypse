using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private UnitsSpawnSettings _unitsSpawnSettings;
    [SerializeField] private ShootingAnchor _shootingAnchor;

    public override void InstallBindings()
    {
        //Debug.Log("LEVEL: Install");

        RegisterSimpleStateMachine();

		RegisterGameplayServices();

        Container.Bind<IInitializable>().To<LevelBootstrapper>().AsSingle().NonLazy();
    }

    private void RegisterGameplayServices()
    {
        if (_unitPrefab == null || _projectilePrefab == null)
            Debug.LogError("[LevelInstaller] Pool prefab link lost");

        if (_unitsSpawnSettings != null)
        {
            Container.Bind<UnitsSpawnSettings>()
                .FromInstance(_unitsSpawnSettings)
                .AsSingle();
        }
        else
        {
            Container.Bind<UnitsSpawnSettings>()
                .FromComponentInHierarchy()
                .AsSingle();
        }

        if (_shootingAnchor != null)
        {
            Container.Bind<ShootingAnchor>()
                .FromInstance(_shootingAnchor)
                .AsSingle();
        }

        Container.BindInterfacesAndSelfTo<UnitsFactory>()
            .AsSingle();

        Container.BindInterfacesAndSelfTo<ProjectileSelection>()
            .AsSingle();

        Container.Bind<ICoroutineRunner>()
            .To<CoroutineRunner>()
            .FromNewComponentOnNewGameObject()
            .WithGameObjectName("CoroutineRunner")
            .AsSingle();

        Container.BindMemoryPool<Unit, Unit.UnitPool>()
            .FromComponentInNewPrefab(_unitPrefab)
            .UnderTransformGroup("Units");

        Container.BindMemoryPool<Projectile, Projectile.ProjectilePool>()
            .FromComponentInNewPrefab(_projectilePrefab)
            .UnderTransformGroup("Projectiles");

        Container.Bind<IPlayerHealth>().To<PlayerHealth>().AsSingle();

        Container.BindInterfacesAndSelfTo<GameCondition>().AsSingle();
        
        Container.Bind<IUnitTracker>().To<UnitTracker>().AsSingle();

        Container.Bind<IPauseState>().To<PauseState>().AsSingle();
    }
	private void RegisterSimpleStateMachine()
	{
		Container.Bind<ILevelStateMachine>().To<LevelStateMachine>().AsSingle();
	}
}
