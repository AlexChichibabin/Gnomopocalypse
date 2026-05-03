using System;
using UnityEngine;
using Zenject;

public class PauseState : IPauseState
{
	public event Action<bool> IsPausedEvent;
	public bool IsPaused => isPaused;

	private bool isPaused;
	ILevelStateMachine levelStateMachine;
	IInputService inputService;


	[Inject]
	public void Construct(
		ILevelStateMachine levelStateMachine,
		IInputService inputService)
	{
		this.levelStateMachine = levelStateMachine;
		this.inputService = inputService;
	}

	public void SwitchPause()
	{
		if (isPaused == true)
			UnPause();
		else
			Pause();
	}
	public void Pause()
	{
		isPaused = true;
		inputService.DisableGameplay();
		IsPausedEvent?.Invoke(isPaused);
	}
	public void UnPause()
	{
		isPaused = false;
		inputService.EnableGameplay();
		IsPausedEvent?.Invoke(isPaused);
	}
}
