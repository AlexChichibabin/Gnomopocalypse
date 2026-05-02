using UnityEngine;
using Zenject;

public class Unit : MonoBehaviour
{
  
    [SerializeField] private UnitMove _unitMove;
    [SerializeField] private UnitHealth _unitHealth;
    [SerializeField] private UnitView _unitView;

    private UnitsFactory factory;

	public UnitHealth Damageble => _unitHealth;

    public UnitType UnitType { get; private set; }
    public void SetPool(UnitsFactory factory) => this.factory = factory;
    private void OnSpawned(UnitConfig config)
    {
        UnitType = config.UnitType;

       _unitView.Init(config.UnitType);
        _unitMove.Init(config.StartMoveSpeed);
        _unitHealth.Init(config.StartHealth, config.MainDamagePercent, config.SecondaryDamagePercent);
        _unitHealth.ZeroHealth += OnZeroHealth;

    }

    private void OnZeroHealth()
    {
        Debug.Log("Gnome is dead");

		factory.DespawnUnit(this);
    }

    private void OnDespawned()
    {
        _unitHealth.ZeroHealth -= OnZeroHealth;
        Debug.Log("[Unit] Despawned");
    }

    public class UnitPool : MonoMemoryPool<UnitConfig, Unit>
    {
        protected override void Reinitialize(UnitConfig config, Unit unit)
        {
            unit.OnSpawned(config);
        }

        protected override void OnDespawned(Unit unit)
        {
            unit.OnDespawned();
            base.OnDespawned(unit);
        }
    }
}
