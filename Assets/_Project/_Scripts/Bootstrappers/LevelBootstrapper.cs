using UnityEngine;
using Zenject;

public class LevelBootstrapper : IInitializable
{
    private ILevelStateMachine simpleStateMachine;

    public LevelBootstrapper(ILevelStateMachine simpleStateMachine)
    {
		this.simpleStateMachine = simpleStateMachine;
	}

    public void Initialize()
    {
        Debug.Log("LEVEL: Boot");
        InitLevelStateMachine();
    }

    private void InitLevelStateMachine()
    {
		simpleStateMachine.ApplyState(LevelState.Bootstrap);
	}
}