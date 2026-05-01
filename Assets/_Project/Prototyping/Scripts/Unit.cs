using UnityEngine;
using Zenject;

public class Unit : MonoBehaviour
{
    private void OnSpawned()
    {
         Debug.Log("[Unit] Spawned");
    }

    private void OnDespawned()
    {
       Debug.Log("[Unit] Despawned");
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
