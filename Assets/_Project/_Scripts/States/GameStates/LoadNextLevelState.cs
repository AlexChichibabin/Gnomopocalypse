using UnityEngine.SceneManagement;

public class LoadNextLevelState : IEnterableState
{
    private IConfigProvider configProvider;

    public LoadNextLevelState(IConfigProvider configProvider)
    {
        this.configProvider = configProvider;
    }

    public void Enter()
    {
        string sceneName = configProvider.GetLevel(0).SceneName;

        if (SceneManager.GetActiveScene().name != sceneName)
            SceneManager.LoadScene(sceneName);
    }
}