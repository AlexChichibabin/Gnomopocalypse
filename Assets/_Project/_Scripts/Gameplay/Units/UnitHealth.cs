using System;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    private float _startHealth;
    private float _mainDamage;
    private float _secondaryDamage; // !_secondaryDamage < _mainDamage !

    public float CurrentHealth { get; private set; }

    public event Action ZeroHealth;
    public event Action<float, float> HealthChanged;

    public void Init(float startHealth, float mainDamage, float secondaryDamage)
    {
        _startHealth = startHealth;
        _mainDamage = mainDamage;
        _secondaryDamage = secondaryDamage;

        ResetHealth();
    }

    public void DealMainDamage()
    {
        CurrentHealth -= GetDamageFromPercent(_mainDamage);
        Debug.Log("[UnitHealth] DealMainDamage");
        CheckHealth();

    }

    public void DealSecondaryDamage()
    {
        CurrentHealth -= GetDamageFromPercent(_secondaryDamage);
        Debug.Log("[UnitHealth] DealSecondaryDamage");
        CheckHealth();
    }
    public void ResetHealth()
    {
        CurrentHealth = _startHealth;

        HealthChanged?.Invoke(CurrentHealth, _startHealth);
    }

    private float GetDamageFromPercent(float percent) =>
        _startHealth * percent / 100f;

    private void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            ZeroHealth?.Invoke();
        }
        HealthChanged?.Invoke(CurrentHealth, _startHealth);


    }

}
