using UnityEngine;
using Zenject;

public class PrototypeInstaller : MonoInstaller
{
	[SerializeField] private ShootingAnchor _shootingAnchor;

	public override void InstallBindings()
	{
		Container.Bind<ShootingAnchor>().FromInstance(_shootingAnchor).AsSingle();
	}
}
