using System;
using System.Collections.Generic;
using UnityEngine;

public class TestGreed : MonoBehaviour, IBattleGrid
{
    private const int RowCount = 3;

    [System.Serializable]
    private struct Row
    {
        [SerializeField] private SpriteRenderer _image;

        public SpriteRenderer Image => _image;
        public Transform Parent => _image != null ? _image.transform : null;
    }

    [SerializeField] private int _numberOfCells;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Row[] _rows = new Row[RowCount];

    private List<ICell> _cells = new();
    private TestCell[,] _cellsGrid;

    public IReadOnlyList<ICell> Cells => _cells;

    private void OnValidate()
    {
        if (_rows == null || _rows.Length != RowCount)
        {
            Array.Resize(ref _rows, RowCount);
        }
    }

    private void Awake()
    {
        FillCellsFromChildren();
    }

    [ContextMenu(nameof(SpawnCells))]
    public void SpawnCells()
    {
        if (_numberOfCells <= 0 || _cellPrefab == null || _rows == null)
        {
            return;
        }

        ClearCells();
        _cells.Clear();
        _cellsGrid = new TestCell[_rows.Length, _numberOfCells];

        for (int rowIndex = 0; rowIndex < _rows.Length; rowIndex++)
        {
            Row row = _rows[rowIndex];

            if (row.Image == null || row.Parent == null)
            {
                continue;
            }

            SpawnRowCells(row, rowIndex);
        }
    }

    private void SpawnRowCells(Row row, int rowIndex)
    {
        float cellWidth = row.Image.bounds.size.x / _numberOfCells;
        float startX = row.Image.bounds.min.x + cellWidth / 2f;

        for (int columnIndex = 0; columnIndex < _numberOfCells; columnIndex++)
        {
            GameObject cellObject = Instantiate(_cellPrefab, row.Parent);

            if (!cellObject.TryGetComponent(out TestCell cell))
            {
                Debug.LogError($"{nameof(_cellPrefab)} must have {nameof(TestCell)} component.", this);
                Destroy(cellObject);
                continue;
            }

            cell.SetWidth(cellWidth);
            cellObject.transform.SetParent(row.Parent);
            cellObject.transform.position = new Vector3(startX + cellWidth * columnIndex, row.Parent.position.y, row.Parent.position.z);
            cell.Init(this, rowIndex, columnIndex);

            _cellsGrid[rowIndex, columnIndex] = cell;
            _cells.Add(cell);
        }
    }

    private void ClearCells()
    {
        if (_rows == null)
        {
            return;
        }

        foreach (Row row in _rows)
        {
            if (row.Parent == null)
            {
                continue;
            }

            ClearRowCells(row.Parent);
        }
    }

    private void ClearRowCells(Transform rowParent)
    {
        for (int i = rowParent.childCount - 1; i >= 0; i--)
        {
            Transform child = rowParent.GetChild(i);

            if (!child.TryGetComponent<TestCell>(out _))
            {
                continue;
            }

            if (Application.isPlaying)
            {
                Destroy(child.gameObject);
            }
            else
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    private void FillCellsFromChildren()
    {
        if (_rows == null)
        {
            return;
        }

        _cells.Clear();
        _cellsGrid = new TestCell[_rows.Length, _numberOfCells];

        for (int rowIndex = 0; rowIndex < _rows.Length; rowIndex++)
        {
            Row row = _rows[rowIndex];

            if (row.Parent == null)
            {
                continue;
            }

            int columnIndex = 0;

            for (int childIndex = 0; childIndex < row.Parent.childCount && columnIndex < _numberOfCells; childIndex++)
            {
                if (row.Parent.GetChild(childIndex).TryGetComponent(out TestCell cell))
                {
                    cell.Init(this, rowIndex, columnIndex);
                    _cellsGrid[rowIndex, columnIndex] = cell;
                    _cells.Add(cell);
                    columnIndex++;
                }
            }
        }
    }

    public bool TryGetCell(int rowIndex, int columnIndex, out TestCell cell)
    {
        if (_cellsGrid == null)
        {
            FillCellsFromChildren();
        }

        cell = null;

        if (_cellsGrid == null ||
            rowIndex < 0 ||
            columnIndex < 0 ||
            rowIndex >= _cellsGrid.GetLength(0) ||
            columnIndex >= _cellsGrid.GetLength(1))
        {
            return false;
        }

        cell = _cellsGrid[rowIndex, columnIndex];
        return cell != null;
    }

    public ICell[] GetHorizontalNeighbours(int rowIndex, int columnIndex)
    {
        return GetNeighbours(rowIndex, columnIndex, new[]
        {
            new Vector2Int(0, -1),
            new Vector2Int(0, 1)
        });
    }

    public ICell[] GetVerticalNeighbours(int rowIndex, int columnIndex)
    {
        return GetNeighbours(rowIndex, columnIndex, new[]
        {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0)
        });
    }

    public ICell[] GetDiagonalNeighbours(int rowIndex, int columnIndex)
    {
        return GetNeighbours(rowIndex, columnIndex, new[]
        {
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(1, 1)
        });
    }

    private ICell[] GetNeighbours(int rowIndex, int columnIndex, IReadOnlyList<Vector2Int> offsets)
    {
        List<ICell> neighbours = new();

        foreach (Vector2Int offset in offsets)
        {
            if (TryGetCell(rowIndex + offset.x, columnIndex + offset.y, out TestCell neighbour))
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours.ToArray();
    }
    
    public ICell GetCell(Vector3 worldPosition)
    {
        if (_cells.Count == 0)
        {
            FillCellsFromChildren();
        }

        foreach (ICell cell in _cells)
        {
            if (cell is not TestCell testCell || !testCell.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                continue;
            }

            Bounds bounds = spriteRenderer.bounds;

            if (worldPosition.x >= bounds.min.x &&
                worldPosition.x <= bounds.max.x &&
                worldPosition.y >= bounds.min.y &&
                worldPosition.y <= bounds.max.y)
            {
                return cell;
            }
        }

        return null;
    }

    public Vector3 GetWorldPosition(ICell cell)
    {
        if (cell is not TestCell testCell)
        {
            return Vector3.zero;
        }

        if (testCell.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            return spriteRenderer.bounds.center;
        }

        return testCell.transform.position;
    }

    public bool IsFilled(ICell cell)
    {
        return cell != null && cell.TryGetUnit(out _);
    }
}
