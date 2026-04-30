using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBootstrapState : IEnterableState
{
    private ILevelStateSwitcher levelStateSwitcher;
    private IConfigProvider configProvider;

	public LevelBootstrapState( 
        ILevelStateSwitcher levelStateSwitcher,
        IConfigProvider configProvider
		)
    {
        this.levelStateSwitcher = levelStateSwitcher;
        this.configProvider = configProvider;
    }

    public void Enter()
    {
        Debug.Log("LEVEL: Init");

        string sceneName = SceneManager.GetActiveScene().name;
        LevelConfig levelConfig = configProvider.GetLevel(sceneName);


		levelStateSwitcher.Enter<LevelGameplayState>();
	}

}