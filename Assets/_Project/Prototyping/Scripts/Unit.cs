using UnityEngine;
using Zenject;

public class Unit : MonoBehaviour
{
    private float _startHealth = 100;// temp
    private float _startMoveSpeed = 1; // temp

    [SerializeField] private UnitMove _unitMove;
    [SerializeField] private UnitHealth _unitHealth;

    public UnitHealth Damageble => _unitHealth;

    public UnitType UnitType { get; private set; }

    [Inject]
    private void Construct(/* config*/)
    {
        //todo
    }

    private void OnSpawned()
    {
        UnitType = UnitType.Smelly;
        //TODO add UnitType

        _unitMove.Init( /* config from UnitType */ _startMoveSpeed);
        _unitHealth.Init( /* config from UnitType */ _startHealth, 25, 10);      // temp
        _unitHealth.ZeroHealth += OnZeroHealth;

    }

    private void OnZeroHealth()
    {
        //todo death
    }

    private void OnDespawned()
    {
        _unitHealth.ZeroHealth -= OnZeroHealth;
        Debug.Log("[Unit] Spawned");
    }

    public class UnitPool : MonoMemoryPool<Unit>
    {
        protected override void OnSpawned(Unit unit)
        {
            base.OnSpawned(unit);
            unit.OnSpawned();
        }

        protected override void OnDespawned(Unit unit)
        {
            unit.OnDespawned();
            base.OnDespawned(unit);
        }
    }
}
