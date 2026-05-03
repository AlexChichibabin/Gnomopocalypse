using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Unit : MonoBehaviour
{
    private const int SortingConflictOffset = 1;
    private const int MaxUnitSortingContacts = 8;

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
    private Coroutine _hurtRoutine;
    private readonly Queue<Action> _pendingDamage = new Queue<Action>();
    private readonly Collider2D[] _sortingContactResults = new Collider2D[MaxUnitSortingContacts];
    private Collider2D _collider;
    private int _baseSortingOrder;
    private int _currentSortingOrder;
    private bool _hasSortingConflictOffset;
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
       
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void LateUpdate()
    {
        ResolveSortingContacts();
    }


    public void SetPool(UnitsFactory factory) => this.factory = factory;

    public void DealMainDamage()
    {
        EnqueueDamage(_unitHealth.DealMainDamage);
    }

    public void DealSecondaryDamage()
    {
        EnqueueDamage(_unitHealth.DealSecondaryDamage);
    }

    public void SetSortingOrder(int sortingOrder)
    {
        _baseSortingOrder = sortingOrder;
        _currentSortingOrder = sortingOrder;
        _hasSortingConflictOffset = false;
        _unitView.SetSortingOrder(_currentSortingOrder);
    }

    private void OnSpawned(UnitConfig config)
    {
        _isDead = false;
        _pendingDamage.Clear();
        StopDeathRoutine();
        _unitHealth.ZeroHealth -= OnZeroHealth;
        ApplyConfig(config, true);
        _unitHealth.ZeroHealth += OnZeroHealth;
        _pauseState.IsPausedEvent += OnPauseChanged;

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
        StopHurtRoutine();
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

    private IEnumerator HurtRoutine()
    {
        while (_pendingDamage.Count > 0 && !IsDead())
        {
            Action dealDamage = _pendingDamage.Dequeue();

            _unitMove.Immobilize();

            if (_unitAnimator != null)
                yield return PlayHurtAnimationAndWaitForEnd();

            dealDamage?.Invoke();

            if (IsDead())
                break;

            if (_unitAnimator != null)
                _unitAnimator.PlayWalk();

            _unitMove.Run();
        }

        _hurtRoutine = null;
    }

    private IEnumerator PlayHurtAnimationAndWaitForEnd()
    {
        bool isAnimationEnded = false;

        void OnAnimationEnded() => isAnimationEnded = true;

        try
        {
            _unitAnimator.AnimationEnded += OnAnimationEnded;
            _unitAnimator.PlayHurt();

            // temp
            float timeoutTime = Time.time + 2f;
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

    private void StopHurtRoutine()
    {
        if (_hurtRoutine == null)
            return;

        StopCoroutine(_hurtRoutine);
        _hurtRoutine = null;
    }

    private void EnqueueDamage(Action dealDamage)
    {
        if (IsDead())
            return;

        _pendingDamage.Enqueue(dealDamage);

        if (_hurtRoutine == null)
            _hurtRoutine = StartCoroutine(HurtRoutine());
    }

    private void ResolveSortingContacts()
    {
        if (_collider == null || !_collider.enabled)
            return;

        bool shouldUseSortingConflictOffset = false;
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.NoFilter();
        int contactsCount = Physics2D.OverlapCollider(_collider, contactFilter, _sortingContactResults);

        for (int i = 0; i < contactsCount; i++)
        {
            Collider2D contact = _sortingContactResults[i];

            if (contact == null)
                continue;

            Unit otherUnit = contact.GetComponentInParent<Unit>();

            if (otherUnit == null || otherUnit == this)
                continue;

            if (otherUnit._baseSortingOrder != _baseSortingOrder)
                continue;

            if (GetInstanceID() > otherUnit.GetInstanceID())
            {
                shouldUseSortingConflictOffset = true;
                break;
            }
        }

        SetSortingConflictOffset(shouldUseSortingConflictOffset);
    }

    private void SetSortingConflictOffset(bool shouldUseSortingConflictOffset)
    {
        if (_hasSortingConflictOffset == shouldUseSortingConflictOffset)
            return;

        _hasSortingConflictOffset = shouldUseSortingConflictOffset;
        _currentSortingOrder = _baseSortingOrder + (_hasSortingConflictOffset ? SortingConflictOffset : 0);
        _unitView.SetSortingOrder(_currentSortingOrder);
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
            _unitAnimator.Init(config.UnitType, resetHealth);

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
        _pendingDamage.Clear();
        StopLifePhase();
        StopHurtRoutine();
        StopDeathRoutine();
        _deathRoutine = StartCoroutine(DeathRoutine());
    }

    private void OnDespawned()
    {
        StopLifePhase();
        StopDeathRoutine();
        StopHurtRoutine();
        _pendingDamage.Clear();
        SetSortingConflictOffset(false);
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
