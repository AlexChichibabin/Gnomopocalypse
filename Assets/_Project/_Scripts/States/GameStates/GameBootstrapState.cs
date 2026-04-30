using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrapState : IEnterableState
{
    private IGameStateSwitcher gameStateSwitcher;
    private IConfigProvider configProvider;

	public GameBootstrapState(
        IGameStateSwitcher gameStateSwitcher, 
        IConfigProvider configProvider)
    {
        this.gameStateSwitcher = gameStateSwitcher;
        this.configProvider = configProvider;
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
			gameStateSwitcher.Enter<LoadNextLevelState>();   
	}
}