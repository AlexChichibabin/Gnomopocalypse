using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour
{
    public void OnSpawned()
    {
        Debug.Log("[Tower] Spawned");
    }

    public void OnDespawned()
    {
        Debug.Log("[Tower] Despawned");
    }

    public class TowerPool : MonoMemoryPool<Tower>
    {
        protected override void OnSpawned(Tower tower)
        {
            base.OnSpawned(tower);
            tower.OnSpawned();
        }

        protected override void OnDespawned(Tower tower)
        {
            tower.OnDespawned();
            base.OnDespawned(tower);
        }
    }
}
