using System;
using UnityEngine;

public class GameCondition : IGameCondition, IDisposable
{

	private IPlayerHealth heatlh;
	private ILevelStateMachine levelStateMachine;
	private IUnitTracker unitTracker;
	public GameCondition(
		IPlayerHealth heatlh, 
		ILevelStateMachine levelStateMachine,
		IUnitTracker unitTracker)
	{
		this.heatlh = heatlh;
		this.levelStateMachine = levelStateMachine;
		this.unitTracker = unitTracker;

		heatlh.OnDeath += OnPlayerDeath;
		unitTracker.OnAllUnitDeath += OnAllUnitsDeath;
	}

	public void Init()
	{
		
	}
	private void OnAllUnitsDeath()
	{
		levelStateMachine.ApplyState(LevelState.Win);
	}

	private void OnPlayerDeath()
	{
		levelStateMachine.ApplyState(LevelState.Lose);
	}

	public void Dispose()
	{
		heatlh.OnDeath -= OnPlayerDeath;
		unitTracker.OnAllUnitDeath -= OnAllUnitsDeath;
	}
}
