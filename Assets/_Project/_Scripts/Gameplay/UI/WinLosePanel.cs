using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class WinLosePanel : MonoBehaviour
{
	[SerializeField] private Sprite winPanelSprite;
	[SerializeField] private Sprite losePanelSprite;
	[SerializeField] private Image panelImage;
	[SerializeField] private Button nextLevelButton;
	[SerializeField] private TextMeshProUGUI nextLevelText;

	private ILevelStateMachine levelStateMachine;
	private IGameStateMachine gameStateMachine;

	[Inject]
    public void Construct(
		ILevelStateMachine levelStateMachine,
		IGameStateMachine gameStateMachine)
    {
        this.levelStateMachine = levelStateMachine;
		this.gameStateMachine = gameStateMachine;

        levelStateMachine.StateChanged += OnLevelEnd;

	}
	private void Awake()
	{
        gameObject.SetActive(false);
	}
	private void OnDestroy()
	{
		levelStateMachine.StateChanged -= OnLevelEnd;
	}
	private void OnLevelEnd(LevelState state)
    {
        if (state != LevelState.Win && state != LevelState.Lose) return;

		if (state == LevelState.Win)
        {
			panelImage.sprite = winPanelSprite;
			nextLevelText.text = "ƒ¿À≈≈";
			nextLevelButton.onClick.AddListener(LoadNext);
		}
        else
        {
			panelImage.sprite = losePanelSprite;
			nextLevelText.text = "«¿ÕŒ¬Œ";
			nextLevelButton.onClick.AddListener(RestartLevel);
		} 

		gameObject.SetActive(true);
	}
	private void LoadNext()
	{
		gameStateMachine.ApplyState(GameState.LoadLevel);
		nextLevelButton.onClick.RemoveAllListeners();
	}
	private void RestartLevel()
	{
		var sceneName = SceneManager.GetActiveScene().name;

		SceneManager.LoadScene(sceneName);

		nextLevelButton.onClick.RemoveAllListeners();
	}
}
