using System;
using UnityEngine;
using Zenject;

public class PausePanel : MonoBehaviour
{
	private IPauseState pauseState;
	[Inject] private ILevelStateMachine levelStateMachine;

	[Inject]
	public void Construct(
		IPauseState pauseState)
	{
		this.pauseState = pauseState;
	}
	private void Start()
	{
		pauseState.IsPausedEvent += OnPausedChanged;
		//gameObject.SetActive(false);
	}

	private void OnPausedChanged(bool isPaused)
	{
		if (levelStateMachine.State != LevelState.Win && levelStateMachine.State != LevelState.Lose)
			gameObject.SetActive(isPaused);
	}
	private void OnDestroy()
	{
		pauseState.IsPausedEvent -= OnPausedChanged;
	}
}
