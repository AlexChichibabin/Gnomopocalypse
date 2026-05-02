using System;
using UnityEngine;

public class GameCondition : IGameCondition, IDisposable
{

	private IPlayerHealth heatlh;
	private ILevelStateMachine levelStateMachine;
	public GameCondition(
		IPlayerHealth heatlh, 
		ILevelStateMachine levelStateMachine)
	{
		this.heatlh = heatlh;
		this.levelStateMachine = levelStateMachine;

		heatlh.OnDeath += OnPlayerDeath;
	}

	public void Init()
	{
		
	}

	private void OnPlayerDeath()
	{
		levelStateMachine.ApplyState(LevelState.Lose);
	}

	public void Dispose()
	{
		heatlh.OnDeath -= OnPlayerDeath;
	}
}
