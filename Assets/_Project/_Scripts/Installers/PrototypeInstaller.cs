using UnityEngine;
using Zenject;

public class PrototypeInstaller : MonoInstaller
{
	[SerializeField] private Unit _unitPrefab;

	public override void InstallBindings()
	{

			Container.BindMemoryPool<Unit, Unit.UnitPool>()
			.FromComponentInNewPrefab(_unitPrefab)
			.UnderTransformGroup("Units");
	}
}
