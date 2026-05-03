using TMPro;
using UnityEngine;

public class UnitHealthUI : MonoBehaviour
{
    //[SerializeField] private Bar _hpBar;
    [SerializeField]private UnitHealth _unitHealth;
    [SerializeField] private TMP_Text _text;

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
        //_hpBar.SetValue(currentHp,maxHp );
        _text.text = Mathf.RoundToInt(currentHp).ToString();
    }
}
