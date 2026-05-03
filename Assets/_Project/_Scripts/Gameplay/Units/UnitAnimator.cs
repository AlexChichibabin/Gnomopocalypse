using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _smellyAnimator;
    [SerializeField] private Animator _dirtyAnimator;
    [SerializeField] private Animator _leakingAnimator;
    [SerializeField] private Animator _stickyAnimator;

    private static readonly int WalkState = Animator.StringToHash("Walk");
    private static readonly int DrinkTrigger = Animator.StringToHash("drink");
    private static readonly int DeathTrigger = Animator.StringToHash("death");

    private Animator _currentAnimator;
    private UnitAnimationEventReceiver _currentEventReceiver;

    public event Action AnimationEnded;

    private void Awake()
    {
        EnsureEventReceiver(_smellyAnimator);
        EnsureEventReceiver(_dirtyAnimator);
        EnsureEventReceiver(_leakingAnimator);
        EnsureEventReceiver(_stickyAnimator);
    }

    public void Init(UnitType unitType)
    {
        if (_currentEventReceiver != null)
            _currentEventReceiver.Ended -= OnAnimationEnded;

        _currentAnimator = GetAnimator(unitType);
        _currentEventReceiver = EnsureEventReceiver(_currentAnimator);

        if (_currentEventReceiver != null)
            _currentEventReceiver.Ended += OnAnimationEnded;

        ResetAnimation();
    }

    private void ResetAnimation()
    {
        if (_currentAnimator == null)
            return;

        ResetTriggers();
        _currentAnimator.Rebind();
        _currentAnimator.Update(0f);
        _currentAnimator.Play(WalkState, 0, 0f);
    }

    public void PlayWalk()
    {
        ResetAnimation();
    }

    public void PlayDrink()
    {
        if (_currentAnimator == null)
            return;

        ResetTriggers();
        _currentAnimator.SetTrigger(DrinkTrigger);
    }

    public void PlayDeath()
    {
        if (_currentAnimator == null)
            return;

        ResetTriggers();
        _currentAnimator.SetTrigger(DeathTrigger);
    }

    private void ResetTriggers()
    {
        _currentAnimator.ResetTrigger(DrinkTrigger);
        _currentAnimator.ResetTrigger(DeathTrigger);
    }

    private Animator GetAnimator(UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Smelly:
                return _smellyAnimator;
            case UnitType.Dirty:
                return _dirtyAnimator;
            case UnitType.Leaking:
                return _leakingAnimator;
            case UnitType.Sticky:
                return _stickyAnimator;
            default:
                return null;
        }
    }

    private UnitAnimationEventReceiver EnsureEventReceiver(Animator animator)
    {
        if (animator == null)
            return null;

        UnitAnimationEventReceiver eventReceiver = animator.GetComponent<UnitAnimationEventReceiver>();

        if (eventReceiver == null)
            eventReceiver = animator.gameObject.AddComponent<UnitAnimationEventReceiver>();

        return eventReceiver;
    }

    private void OnAnimationEnded()
    {
        AnimationEnded?.Invoke();
    }
}
