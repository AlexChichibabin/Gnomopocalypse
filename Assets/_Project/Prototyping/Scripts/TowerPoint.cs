using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TowerPoint : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _sprite;

    private bool _dirty;

    private Tower.TowerPool _towerPool;

    public event Action<Tower> TowerSpawnrd;

    [Inject]
    public void Construct(Tower.TowerPool towerPool)
    {
        Debug.Log("[Tower] Construct");
        _towerPool = towerPool;
    }

    void Start()
    {
        _sprite.SetActive(true);
        _dirty = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_dirty)
            SpawnTower();
            
    }

    private void SpawnTower()
    {
        Tower tower = _towerPool.Spawn();
        tower.transform.position = transform.position;

        TowerSpawnrd?.Invoke(tower);

        _sprite.SetActive(false);
        _dirty = true;
    }
}
