using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class Unit : MonoBehaviour
{

    [SerializeField] private UnitMove _unitMove;
    [SerializeField] private UnitHealth _unitHealth;
    [SerializeField] private UnitView _unitView;

    private UnitsFactory factory;
    private UnitConfig _config;
    private IConfigProvider _configProvider;
    private Coroutine _lifePhaseRoutine;

    public event Action Mutated;

    public UnitHealth Damageble => _unitHealth;

    public UnitType UnitType { get; private set; }

    [Inject]
    public void Construct(IConfigProvider configProvider) =>
       _configProvider = configProvider;

    public void SetPool(UnitsFactory factory) => this.factory = factory;

    private void OnSpawned(UnitConfig config)
    {
        _unitHealth.ZeroHealth -= OnZeroHealth;
        ApplyConfig(config);
        _unitHealth.ZeroHealth += OnZeroHealth;

        StartLifePhase();
    }

     private void StartLifePhase()
    {
        StopLifePhase();
        _lifePhaseRoutine = StartCoroutine(LifePhaseRoutine());
    }

     private IEnumerator LifePhaseRoutine()
    {
        while (_config != null)
        {
            UnitConfig currentConfig = _config;

            if (currentConfig.MinStayTime <= 0)
            {
                Debug.LogWarning("[Unit] Min stay time should be greater than zero");
                yield break;
            }

            yield return new WaitForSeconds(currentConfig.MinStayTime);

            UnitConfig mutationUnitConfig = _configProvider.GetRandomUnitMutationConfig();

            if (mutationUnitConfig == null || mutationUnitConfig == _config)
                continue;

            Mutation(mutationUnitConfig);
        }
    }

     private void Mutation(UnitConfig config)
    {
        ApplyConfig(config);
        Mutated?.Invoke();
    }

    private void StopLifePhase()
    {
        if (_lifePhaseRoutine == null)
            return;

        StopCoroutine(_lifePhaseRoutine);
        _lifePhaseRoutine = null;
    }



    private void ApplyConfig(UnitConfig config)
    {
        if (config == null)
            return;

        _config = config;
        UnitType = config.UnitType;

        _unitView.Init(config.UnitType);
        _unitMove.Init(config.StartMoveSpeed);
        _unitHealth.Init(config.StartHealth, config.MainDamagePercent, config.SecondaryDamagePercent);
    }

    private void OnZeroHealth()
    {
        Debug.Log("Gnome is dead");

        factory.DespawnUnit(this);
    }

    private void OnDespawned()
    {
        _unitHealth.ZeroHealth -= OnZeroHealth;
        Debug.Log("[Unit] Despawned");
    }

    public class UnitPool : MonoMemoryPool<UnitConfig, Unit>
    {
        protected override void Reinitialize(UnitConfig config, Unit unit)
        {
            unit.OnSpawned(config);
        }

        protected override void OnDespawned(Unit unit)
        {
            unit.OnDespawned();
            base.OnDespawned(unit);
        }
    }
}
