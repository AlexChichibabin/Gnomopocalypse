using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelState
{
	None,
	Bootstrap,
	Gameplay,
	Win,
	Lose
}

public class LevelStateMachine : ILevelStateMachine
{
	public LevelState State => currentState;
	public event Action<LevelState> StateChanged;

	private LevelState currentState = LevelState.None;
	private IConfigProvider configProvider;
	private IInputService inputService;

	public LevelStateMachine(
		IConfigProvider configProvider,
		IInputService inputService)
	{
		this.configProvider = configProvider;
		this.inputService = inputService;
	}

	public void ApplyState(LevelState state)
	{
		switch (state)
		{
			case LevelState.None:
				ApplyNone();
				break;
			case LevelState.Bootstrap:
				ApplyBootstrap();
				break;
			case LevelState.Gameplay:
				ApplyGameplay();
				break;
			case LevelState.Win:
				ApplyWin();
				break;
			case LevelState.Lose:
				ApplyLose();
				break;

			default:
				ApplyNone();
				break;
		}
	}

	private void ApplyNone()
	{
		currentState = LevelState.None;
	}
	private void ApplyBootstrap()
	{
		Debug.Log("LEVEL: Init");
		currentState = LevelState.Bootstrap;

		string sceneName = SceneManager.GetActiveScene().name;
		LevelConfig levelConfig = configProvider.GetLevel(sceneName);

		StateChanged?.Invoke(currentState);

		ApplyState(LevelState.Gameplay);
	}
	private void ApplyGameplay()
	{
		if (currentState != LevelState.Bootstrap) return;

		Debug.Log("LEVEL: Gameplay");
		currentState = LevelState.Gameplay;

		inputService.EnableGameplay();

		StateChanged?.Invoke(currentState);
	}
	private void ApplyWin()
	{
		if (currentState != LevelState.Gameplay) return;

		currentState = LevelState.Win;


		StateChanged?.Invoke(currentState);
	}
	private void ApplyLose()
	{
		if (currentState != LevelState.Gameplay) return;

		currentState = LevelState.Lose;


		StateChanged?.Invoke(currentState);
	}
}
