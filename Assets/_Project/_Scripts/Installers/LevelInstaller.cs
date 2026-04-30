using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private LevelStateMachineTicker levelStateMachineTicker;

    public override void InstallBindings()
    {
        Debug.Log("LEVEL: Install");

        //RegisterSimpleStateMachine();

		RegisterGameplayServices();

        //RegisterLevelStateMachine();

        Container.Bind<IInitializable>().To<LevelBootstrapper>().AsSingle().NonLazy();
    }


	private void OnDestroy()
    {
        //UnregisterLevelStateMachine();
    }


	//private void RegisterLevelStateMachine()
 //   {
 //       Container.Bind<ILevelStateSwitcher>().To<LevelStateMachine>().AsSingle();
 //       Container.Bind<LevelBootstrapState>().FromNew().AsSingle();
	//	Container.Bind<LevelGameplayState>().FromNew().AsSingle();
 //       Container.Bind<LevelStateMachineTicker>().FromInstance(levelStateMachineTicker).AsSingle(); // ??
 //   }

 //   private void UnregisterLevelStateMachine()
 //   {
 //       Container.Unbind<ILevelStateSwitcher>();
	//	Container.Unbind<LevelBootstrapState>();
	//	Container.Unbind<LevelGameplayState>();
 //       Container.Unbind<LevelStateMachineTicker>();
 //   }

    private void RegisterGameplayServices()
    {

    }

}
