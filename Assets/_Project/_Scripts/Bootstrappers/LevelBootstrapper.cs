using UnityEngine;
using Zenject;

public class LevelBootstrapper : IInitializable
{
    //private ILevelStateSwitcher levelStateSwitcher;
    //private LevelBootstrapState levelBootstrapState;
    //private LevelGameplayState levelResearchState;
    private ISimpleStateMachine simpleStateMachine;

    public LevelBootstrapper(
		//ILevelStateSwitcher levelStateSwitcher,
		//LevelBootstrapState levelBootstrapState,
		//LevelGameplayState levelResearchState
		ISimpleStateMachine simpleStateMachine
		)
    {
		//this.levelStateSwitcher = levelStateSwitcher;
		//this.levelBootstrapState = levelBootstrapState;
		//this.levelResearchState = levelResearchState;
		this.simpleStateMachine = simpleStateMachine;

	}

    public void Initialize()
    {
        Debug.Log("LEVEL: Boot");
        InitLevelStateMachine();
    }

    private void InitLevelStateMachine()
    {
		//levelStateSwitcher.AddState(levelBootstrapState);
		//levelStateSwitcher.AddState(levelResearchState);
		simpleStateMachine.ApplyState(LevelState.Bootstrap);
		//levelStateSwitcher.Enter<LevelBootstrapState>();
	}
}