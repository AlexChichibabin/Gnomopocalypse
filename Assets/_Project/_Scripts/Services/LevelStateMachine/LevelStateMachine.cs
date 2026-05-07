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
	private IPlayerHealth playerHealth;
	private IUnitTracker unitTracker;
	private IAudioService audioService;
	private IPauseState pauseState;
	private IPlayerProgress playerProgress;

	public LevelStateMachine(
		IConfigProvider configProvider,
		IInputService inputService,
		IPlayerHealth playerHealth,
		IUnitTracker unitTracker,
		IAudioService audioService,
		IPauseState pauseState,
		IPlayerProgress playerProgress)
	{
		this.configProvider = configProvider;
		this.inputService = inputService;
		this.playerHealth = playerHealth;
		this.unitTracker = unitTracker;
		this.audioService = audioService;
		this.pauseState = pauseState;
		this.playerProgress = playerProgress;
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
		//Debug.Log("LEVEL: Init");
		currentState = LevelState.Bootstrap;

		string sceneName = SceneManager.GetActiveScene().name;
		LevelConfig levelConfig = configProvider.GetLevel(sceneName);
		playerHealth.RestoreHealth();
		unitTracker.Init();

		if (sceneName == Constants.Level1SceneName)
			audioService.PlayMusic(MusicId.Gameplay1);
		if (sceneName == Constants.Level2SceneName)
			audioService.PlayMusic(MusicId.Gameplay2);
		pauseState.UnPause();

		StateChanged?.Invoke(currentState);

		ApplyState(LevelState.Gameplay);
	}
	private void ApplyGameplay()
	{
		if (currentState != LevelState.Bootstrap) return;

		//Debug.Log("LEVEL: Gameplay");
		currentState = LevelState.Gameplay;

		inputService.EnableGameplay();

		StateChanged?.Invoke(currentState);
	}
	private void ApplyWin()
	{
		if (currentState != LevelState.Gameplay) return;

		currentState = LevelState.Win;
		audioService.PlaySound(SoundId.Win);
		audioService.StopMusic();
		pauseState.Pause();

		string sceneName = SceneManager.GetActiveScene().name;
		LevelConfig levelConfig = configProvider.GetLevel(sceneName);
		playerProgress.AddScore(levelConfig, 3);

		StateChanged?.Invoke(currentState);
	}
	private void ApplyLose()
	{
		if (currentState != LevelState.Gameplay) return;

		currentState = LevelState.Lose;
		audioService.PlaySound(SoundId.Lose);
		audioService.StopMusic();
		pauseState.Pause();

		StateChanged?.Invoke(currentState);
	}
}
