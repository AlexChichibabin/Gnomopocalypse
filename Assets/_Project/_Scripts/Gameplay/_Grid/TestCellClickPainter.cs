using System.Collections.Generic;
using UnityEngine;

public class TestCellClickPainter : MonoBehaviour
{
    [SerializeField] private TestCell _cell;
    [SerializeField] private TestCellClic _cellClick;

    void Start()
    {
        _cellClick.Clicked += OnCellClicked;
    }

    void OnDestroy()
    {
        _cellClick.Clicked -= OnCellClicked;
    }

    private void OnCellClicked(ICell cell)
    {
        PaintCells(_cell.GetVerticalNeighbours(), Color.red);
        PaintCells(_cell.GetHorizontalNeighbours(), Color.blue);
        PaintCells(_cell.GetDiagonalNeighbours(), Color.yellow);
    }

    public void PaintCells(IEnumerable<ICell> cells, Color color)
    {
        foreach (ICell cell in cells)
        {
            if (cell is TestCell testCell && testCell.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.color = color;
            }
        }
    }
}
