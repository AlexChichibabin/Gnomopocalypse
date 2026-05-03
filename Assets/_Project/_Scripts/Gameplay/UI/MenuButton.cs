using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuButton : ButtonBase
{
	private Button button;
	private IGameStateMachine gameStateMachine;

	[Inject]
	public void Construct(
		IGameStateMachine gameStateMachine)
	{
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

		gameStateMachine.ApplyState(GameState.Menu);
	}
}
