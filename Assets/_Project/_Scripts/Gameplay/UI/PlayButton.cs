using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class PlayButton : MonoBehaviour
{
    private Button button;
    private IPlayerProgress progress;
	private IGameStateMachine gameStateMachine;

	[Inject]
    public void Construct(
        IPlayerProgress progress,
        IGameStateMachine gameStateMachine)
    {
        this.progress = progress;
        this.gameStateMachine = gameStateMachine;
    }
	private void Awake()
	{
		button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
	}
    private void OnClick()
    {
		var sceneName = SceneManager.GetActiveScene().name;

		//if (sceneName == Constants.BootstrapSceneName || sceneName == Constants.GameplaySceneName)
			gameStateMachine.ApplyState(GameState.LoadLevel);
	}
}
