using UnityEngine;
using Zenject;

public class PrototypeInstaller : MonoInstaller
{
	[SerializeField] private Tower towerPrefab;

	public override void InstallBindings()
	{
		Container.BindMemoryPool<Tower, Tower.TowerPool>()
			.FromComponentInNewPrefab(towerPrefab)
			.UnderTransformGroup("Towers");
	}
}
