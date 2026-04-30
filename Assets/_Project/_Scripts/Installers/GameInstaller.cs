using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
        Debug.Log("PROJECT: Install");

        RegisterGameServices();

		RegisterGameStateMachine();

		RegisterSimpleStateMachine();


		Container.Bind<IInitializable>().To<GameBootstrapper>().AsSingle().NonLazy();
    }

	private void RegisterGameServices()
	{
        BindConfigProvider();
	}

    private void RegisterGameStateMachine()
	{
		Container.Bind<IGameStateSwitcher>().To<GameStateMachine>().AsSingle();
		Container.Bind<GameBootstrapState>().FromNew().AsSingle();
		Container.Bind<LoadNextLevelState>().FromNew().AsSingle();
	}

    private void BindConfigProvider() => 
		Container.Bind<IConfigProvider>().To<ConfigProvider>().AsSingle();

	private void RegisterSimpleStateMachine()
	{
		Container.Bind<ISimpleStateMachine>().To<SimpleStateMachine>().AsSingle();
	}
}
