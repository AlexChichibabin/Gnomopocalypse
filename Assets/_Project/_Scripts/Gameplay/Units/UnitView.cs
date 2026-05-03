using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private GameObject _smelly;
    [SerializeField] private GameObject _dirty;
    [SerializeField] private GameObject _leaking;
    [SerializeField] private GameObject _sticky;

    public void Init(UnitType unitType)
    {
        DisableAll();

        switch (unitType)
        {
            case UnitType.Smelly:
                SetActive(_smelly, true);
                break;
            case UnitType.Dirty:
                SetActive(_dirty, true);
                break;
            case UnitType.Leaking:
                SetActive(_leaking, true);
                break;
            case UnitType.Sticky:
                SetActive(_sticky, true);
                break;
        }
    }

    private void DisableAll()
    {
        SetActive(_smelly, false);
        SetActive(_dirty, false);
        SetActive(_leaking, false);
        SetActive(_sticky, false);
    }

    private void SetActive(GameObject unitView, bool isActive)
    {
        if (unitView != null)
            unitView.SetActive(isActive);
    }

    public void SetSortingOrder(int sortingOrder)
    {
        SetSortingOrder(_smelly, sortingOrder);
        SetSortingOrder(_dirty, sortingOrder);
        SetSortingOrder(_leaking, sortingOrder);
        SetSortingOrder(_sticky, sortingOrder);
    }

    private void SetSortingOrder(GameObject unitView, int sortingOrder)
    {
        if (unitView == null)
            return;

        SpriteRenderer spriteRenderer = unitView.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = sortingOrder;
    }
}
