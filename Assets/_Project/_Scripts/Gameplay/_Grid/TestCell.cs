using System.Collections.Generic;
using UnityEngine;

public class TestCell : MonoBehaviour, ICell
{
    private List<Unit> _units = new();
    private TestGreed _grid;
    private int _rowIndex;
    private int _columnIndex;

    public void Init(TestGreed grid, int rowIndex, int columnIndex)
    {
        _grid = grid;
        _rowIndex = rowIndex;
        _columnIndex = columnIndex;
    }
    
    public void SetWidth(float width)
    {
        if (!TryGetComponent(out SpriteRenderer spriteRenderer) || spriteRenderer.bounds.size.x == 0f)
        {
            return;
        }

        float scaleMultiplier = width / spriteRenderer.bounds.size.x;
        transform.localScale = new Vector3(transform.localScale.x * scaleMultiplier, transform.localScale.y, transform.localScale.z);
    }

    public ICell[] GetDiagonalNeighbours()
    {
        return _grid != null
            ? _grid.GetDiagonalNeighbours(_rowIndex, _columnIndex)
            : System.Array.Empty<ICell>();
    }

    public ICell[] GetHorizontalNeighbours()
    {
        return _grid != null
            ? _grid.GetHorizontalNeighbours(_rowIndex, _columnIndex)
            : System.Array.Empty<ICell>();
    }

    public ICell[] GetVerticalNeighbours()
    {
        return _grid != null
            ? _grid.GetVerticalNeighbours(_rowIndex, _columnIndex)
            : System.Array.Empty<ICell>();
    }

    public bool TryGetUnit(out Unit unit)
    {
        unit = _units.Count > 0 ? _units[0] : null;
        return unit != null;
    }
}
