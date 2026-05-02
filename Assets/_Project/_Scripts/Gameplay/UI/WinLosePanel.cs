using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WinLosePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lable;
    [SerializeField] private Button nextLevelButton;

    private ILevelStateMachine levelStateMachine;

    [Inject]
    public void Construct(ILevelStateMachine levelStateMachine)
    {
        this.levelStateMachine = levelStateMachine;

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
            lable.text = "Victory";
			nextLevelButton.gameObject.SetActive(true);
		}
        else
        {
			lable.text = "Defeat";
            nextLevelButton.gameObject.SetActive(false);
		} 

		gameObject.SetActive(true);
	}
}
