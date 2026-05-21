using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestCellClic : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ICell _cell;

    public event Action<ICell> Clicked;

    public void OnPointerClick(PointerEventData eventData) =>
     Clicked?.Invoke(_cell);
}
