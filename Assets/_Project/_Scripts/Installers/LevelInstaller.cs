using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("LEVEL: Install");

        RegisterSimpleStateMachine();

		RegisterGameplayServices();

        Container.Bind<IInitializable>().To<LevelBootstrapper>().AsSingle().NonLazy();
    }

    private void RegisterGameplayServices()
    {

    }
	private void RegisterSimpleStateMachine()
	{
		Container.Bind<ILevelStateMachine>().To<LevelStateMachine>().AsSingle();
	}
}
