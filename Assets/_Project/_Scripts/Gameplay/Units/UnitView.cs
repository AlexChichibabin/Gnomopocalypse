using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void Init(UnitType unitType)
    {
        _spriteRenderer.color = unitType switch
        {
            UnitType.Smelly => Color.green,
            UnitType.Dirty => new Color(0.8f, 0.45f, 0.2f),
            UnitType.Leaking => Color.cyan,
            UnitType.Sticky => Color.magenta,
            _ => Color.white
        };
    }
}
