using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Debug.Log("PROJECT: Install");


		RegisterGameServices();

		RegisterSimpleStateMachine();


		Container.Bind<IInitializable>().To<GameBootstrapper>().AsSingle().NonLazy();
	}

	private void RegisterGameServices()
	{
		BindConfigProvider();
		BindInputService();
		BindPlayerProgress();
	}

	private void BindConfigProvider() =>
		Container.Bind<IConfigProvider>().To<ConfigProvider>().AsSingle();

	private void BindPlayerProgress() =>
		Container.Bind<IPlayerProgress>().To<PlayerProgress>().AsSingle();

	private void BindInputService()
	{
		Container.Bind<PlayerActions>().FromNew().AsSingle();
		Container.Bind<IInputService>().To<InputService>().AsSingle();
	}


	private void RegisterSimpleStateMachine()
	{
		Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
	}
}
