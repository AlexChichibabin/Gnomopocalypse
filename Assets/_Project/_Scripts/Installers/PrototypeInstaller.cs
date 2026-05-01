using UnityEngine;
using Zenject;

public class PrototypeInstaller : MonoInstaller
{
	[SerializeField] private Tower _towerPrefab;
	[SerializeField] private Unit _unitPrefab;

	public override void InstallBindings()
	{
		Container.BindMemoryPool<Tower, Tower.TowerPool>()
			.FromComponentInNewPrefab(_towerPrefab)
			.UnderTransformGroup("Towers");

			Container.BindMemoryPool<Unit, Unit.UnitPool>()
			.FromComponentInNewPrefab(_unitPrefab)
			.UnderTransformGroup("Units");
	}
}
