using UnityEngine;

public enum LevelState
{
	None,
	Bootstrap,
	Gameplay,
	Win,
	Lose
}

public class SimpleStateMachine : ISimpleStateMachine
{
	public LevelState State => currentState;


	private LevelState currentState = LevelState.None;

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

	}
	private void ApplyBootstrap()
	{
		Debug.Log("LEVEL: Init");
	}
	private void ApplyGameplay()
	{
		if (currentState != LevelState.Bootstrap) return;
	}
	private void ApplyWin()
	{
		if (currentState != LevelState.Gameplay) return;
	}
	private void ApplyLose()
	{
		if (currentState != LevelState.Gameplay) return;
	}
}
