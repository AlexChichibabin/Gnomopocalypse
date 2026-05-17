using UnityEngine;

public interface  IBattleGrid 
{
    Vector3 GetWorldPosition(ICell cell);
    ICell GetCell(Vector3 worldPosition);
    bool IsFilled(ICell cell);
}
