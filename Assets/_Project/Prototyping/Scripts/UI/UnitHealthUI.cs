using UnityEngine;

public class UnitHealthUI : MonoBehaviour
{
    [SerializeField] private Bar _hpBar;
    [SerializeField]private UnitHealth _unitHealth;

    void Awake()
    {
        _unitHealth.HealthChanged += RefreshUI;
    }

    void OnDestroy()
    {
         _unitHealth.HealthChanged -= RefreshUI;
    }

    private void RefreshUI(float currentHp, float maxHp)
    {
        _hpBar.SetValue(currentHp,maxHp );
    }
}