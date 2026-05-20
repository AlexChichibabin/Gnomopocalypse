using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileSelectionView : MonoBehaviour
{
    [SerializeField] private Transform _content;

    private List<ProjectileSlot>  _slots = new();

    private ProjectileSelection _projectileSelection;
    private ProjectileSlot.ProjectileSlotPool _projectileSlotPool;
    private ProjectileFactory _projectileFactory;


    [Inject]
    private void Construct(
        ProjectileSelection projectileSelection,
        ProjectileSlot.ProjectileSlotPool projectileSlotPool,
        ProjectileFactory projectileFactory)
    {
        _projectileSelection = projectileSelection;
        _projectileSlotPool = projectileSlotPool;
        _projectileSelection.StockChanged += OnStockChanged;
        _projectileSelection.StockBuilt += OnStockBuilt;
        _projectileFactory = projectileFactory;
    }

    private void OnStockBuilt(IReadOnlyList<ProjectileConfig> list)
    {
        foreach (var cfg in list)
        {
            ProjectileSlot slot = _projectileSlotPool.Spawn(cfg);
            slot.transform.SetParent(_content, false);
            _slots.Add(slot);
        }

        foreach (var slot in _slots)
        {
            slot.Selected += OnSlotSelected;
            slot.PointerEnter += OnSlotPointerEnter;
        }
    }

    private void OnSlotSelected(ProjectileSlot selectedSlot)
    {
        _projectileFactory.Spawn(selectedSlot.ProjectileConfig);
        selectedSlot.DespawnSlot();
    }

    private void OnSlotPointerEnter(ProjectileSlot selectedSlot)
    {
        foreach (var slot in _slots)
        {
            if (slot == selectedSlot)
                slot.Highlight();
            else
                slot.UnHighlight();
        }
    }


    private void OnStockChanged(IReadOnlyList<ProjectileConfig> list)
    {
        
    }


    private void OnDestroy()
    {
        if (_projectileSelection == null)
            return;

        _projectileSelection.StockChanged -= OnStockChanged;
        _projectileSelection.StockBuilt -= OnStockBuilt;

        foreach (var slot in _slots)
        {
            slot.Selected -= OnSlotSelected;
            slot.PointerEnter -= OnSlotPointerEnter;
        }
    }
}
