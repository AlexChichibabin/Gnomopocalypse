using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class PauseButton : ButtonBase
{
	private Button button;
	private IPauseState pauseState;

	[Inject]
	public void Construct(
		IPauseState pauseState)
	{
		this.pauseState = pauseState;
	}
	private void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
	}
	private void OnDestroy()
	{
		button.onClick.RemoveListener(OnClick);
	}


	private void OnClick()
	{
		var sceneName = SceneManager.GetActiveScene().name;

		pauseState.SwitchPause();
	}
}
