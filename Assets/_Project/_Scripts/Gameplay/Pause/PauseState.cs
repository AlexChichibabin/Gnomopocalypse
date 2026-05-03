using System;
using UnityEngine;
using Zenject;

public class PauseState : IPauseState
{
	public event Action<bool> IsPausedEvent;
	public bool IsPaused => isPaused;

	private bool isPaused;
	IInputService inputService;


	[Inject]
	public void Construct(
		IInputService inputService)
	{
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
