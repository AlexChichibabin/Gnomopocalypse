using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrapState : IEnterableState
{
    private IGameStateSwitcher gameStateSwitcher;
    private IConfigProvider configProvider;

    private ISimpleStateMachine simpleStateMachine;

	public GameBootstrapState(
        IGameStateSwitcher gameStateSwitcher, 
        IConfigProvider configProvider,
		ISimpleStateMachine simpleStateMachine)
    {
        this.gameStateSwitcher = gameStateSwitcher;
        this.configProvider = configProvider;

        this.simpleStateMachine = simpleStateMachine;
    }

    public void Enter()
    {
		Init();
	}
    private void Init()
    {
		Debug.Log("GLOBAL: Init");

		Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.numerator;

		configProvider.Load();

		var sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == Constants.BootstrapSceneName || sceneName == Constants.GameplaySceneName)
        {
			gameStateSwitcher.Enter<LoadNextLevelState>();
			simpleStateMachine.ApplyState(LevelState.Bootstrap);
		}
            
	}
}