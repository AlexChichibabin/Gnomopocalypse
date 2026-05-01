using UnityEngine;
using Zenject;

public class Unit : MonoBehaviour
{
    private float _startMoveSpeed = 1; // temp

    [SerializeField] private UnitMove _unitMove;

    [Inject]
    private void Construct(/* config*/)
    {
        //todo
    }

    private void OnSpawned()
    {
        _unitMove.Init( /* config.startMoveSpeed */ _startMoveSpeed);

        Debug.Log("[Unit] Spawned");
    }

    private void OnDespawned()
    {
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
