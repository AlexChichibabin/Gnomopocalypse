using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class Unit : MonoBehaviour
{

    [SerializeField] private UnitMove _unitMove;
    [SerializeField] private UnitHealth _unitHealth;
    [SerializeField] private UnitView _unitView;
    [SerializeField] private UnitAnimator _unitAnimator;

    private UnitsFactory factory;
    private UnitConfig _config;
    private IConfigProvider _configProvider;
    private IAudioService _audioService;
    private IPauseState _pauseState;
    private Coroutine _lifePhaseRoutine;
    private Coroutine _deathRoutine;
    private bool _isDead;

    public event Action Mutated;

    public UnitHealth Damageble => _unitHealth;

    public UnitType UnitType { get; private set; }

    [Inject]
    public void Construct(
        IConfigProvider configProvider,
		IAudioService audioService,
        IPauseState pauseState)
    {
		_configProvider = configProvider;
        _audioService = audioService;
        _pauseState = pauseState;
	}
       

    public void SetPool(UnitsFactory factory) => this.factory = factory;

    private void OnSpawned(UnitConfig config)
    {
        _isDead = false;
        StopDeathRoutine();
        _unitHealth.ZeroHealth -= OnZeroHealth;
        ApplyConfig(config, true);
        _unitHealth.ZeroHealth += OnZeroHealth;
        _pauseState.IsPaused += OnPauseChanged;

        StartLifePhase();
    }

	private void OnPauseChanged(bool isPaused)
	{
		if (isPaused == true)
			_unitMove.enabled = false;
        else
            _unitMove.enabled = true;
	}

	private void StartLifePhase()
    {
        StopLifePhase();
        _lifePhaseRoutine = StartCoroutine(LifePhaseRoutine());
    }

    private IEnumerator LifePhaseRoutine()
    {
        while (_config != null && !_isDead)
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

            if (IsDead())
                yield break;

            yield return DrinkRoutine();

            if (IsDead())
                yield break;

            Mutation(mutationUnitConfig);
            _unitMove.Run();
        }
    }

     private void Mutation(UnitConfig config)
    {
        ApplyConfig(config, false);
        Mutated?.Invoke();
    }

    private void StopLifePhase()
    {
        if (_lifePhaseRoutine == null)
            return;

        StopCoroutine(_lifePhaseRoutine);
        _lifePhaseRoutine = null;
    }

    private IEnumerator DrinkRoutine()
    {
        if (IsDead())
            yield break;

        _unitMove.Immobilize();

        if (_unitAnimator == null)
            yield break;

        yield return PlayAnimationAndWaitForEnd(_unitAnimator.PlayDrink);
    }

    private IEnumerator DeathRoutine()
    {
        _unitMove.Immobilize();

        if (_unitAnimator != null)
            yield return PlayAnimationAndWaitForEnd(_unitAnimator.PlayDeath);

        _deathRoutine = null;
        factory.DespawnUnit(this);
    }

    private IEnumerator PlayAnimationAndWaitForEnd(Action playAnimation)
    {
        bool isAnimationEnded = false;

        void OnAnimationEnded() => isAnimationEnded = true;

        try
        {
            _unitAnimator.AnimationEnded += OnAnimationEnded;
            playAnimation?.Invoke();

            // temp
            float timeoutTime = Time.time + 3f;
            yield return new WaitUntil(() => isAnimationEnded || Time.time >= timeoutTime);
        }
        finally
        {
            _unitAnimator.AnimationEnded -= OnAnimationEnded;
        }
    }

    private void StopDeathRoutine()
    {
        if (_deathRoutine == null)
            return;

        StopCoroutine(_deathRoutine);
        _deathRoutine = null;
    }

    private bool IsDead()
    {
        return _isDead || _unitHealth.CurrentHealth <= 0;
    }



    private void ApplyConfig(UnitConfig config, bool resetHealth)
    {
        if (config == null)
            return;

        _config = config;
        UnitType = config.UnitType;

        _unitView.Init(config.UnitType);
        if (_unitAnimator != null)
            _unitAnimator.Init(config.UnitType);

        _unitMove.Init(config.StartMoveSpeed);

        _unitHealth.Init(
            config.StartHealth, 
            config.MainDamagePercent, 
            config.SecondaryDamagePercent,
            _audioService,
			resetHealth);
	}

	private void OnZeroHealth()
    {
        Debug.Log("Gnome is dead");

        if (_isDead)
            return;

        _isDead = true;
        StopLifePhase();
        StopDeathRoutine();
        _deathRoutine = StartCoroutine(DeathRoutine());
    }

    private void OnDespawned()
    {
        StopLifePhase();
        StopDeathRoutine();
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
