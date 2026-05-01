using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
	None,
	Bootstrap,
	LoadLevel
}
public class GameStateMachine : IGameStateMachine
{
	public GameState State => currentState;
	public event Action<GameState> StateChanged;

	private GameState currentState = GameState.None;
	private IConfigProvider configProvider;

	public GameStateMachine(IConfigProvider configProvider)
	{
		this.configProvider = configProvider;
	}

	public void ApplyState(GameState state)
	{
		switch (state)
		{
			case GameState.None:
				ApplyNone();
				break;
			case GameState.Bootstrap:
				ApplyBootstrap();
				break;
			case GameState.LoadLevel:
				ApplyLoadLevel();
				break;

			default:
				ApplyNone();
				break;
		}
	}

	private void ApplyNone()
	{
		currentState = GameState.None;
	}
	private void ApplyBootstrap()
	{
		Debug.Log("GLOBAL: Init");
		currentState = GameState.Bootstrap;

		Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.numerator;

		configProvider.Load();

		var sceneName = SceneManager.GetActiveScene().name;
		

		StateChanged?.Invoke(currentState);

		if (sceneName == Constants.BootstrapSceneName || sceneName == Constants.GameplaySceneName)
			ApplyState(GameState.LoadLevel);

	}
	private void ApplyLoadLevel()
	{
		if (currentState != GameState.Bootstrap) return;

		Debug.Log("GLOBAL: LoadLevel");
		currentState = GameState.LoadLevel;

		string sceneName = configProvider.GetLevel(0).SceneName;

		if (SceneManager.GetActiveScene().name != sceneName)
		{
			SceneManager.LoadScene(sceneName);
			Debug.Log("GLOBAL: LoadLevel_SceneLoaded");
		}

		StateChanged?.Invoke(currentState);
	}
}
