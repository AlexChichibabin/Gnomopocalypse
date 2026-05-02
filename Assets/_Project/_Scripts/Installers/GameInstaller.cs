using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class GameInstaller : MonoInstaller
{
	[SerializeField] private AudioMixer audioMixer;
	

	public override void InstallBindings()
	{
		Debug.Log("PROJECT: Install");


		RegisterGameServices();

		RegisterSimpleStateMachine();


		Container.Bind<IInitializable>().To<GameBootstrapper>().AsSingle().NonLazy();
	}

	private void RegisterGameServices()
	{
		BindAudioMixer();
		BindConfigProvider();
		BindInputService();
		BindPlayerProgress();
		BindAudioService();
	}

	private void BindAudioMixer() =>
		Container.Bind<AudioMixer>().FromInstance(audioMixer).AsSingle();
	private void BindConfigProvider() =>
		Container.Bind<IConfigProvider>().To<ConfigProvider>().AsSingle();
	private void BindPlayerProgress() =>
		Container.Bind<IPlayerProgress>().To<PlayerProgress>().AsSingle();
	private void BindAudioService() =>
		Container.Bind<IAudioService>().To<AudioService>().AsSingle();
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
