using UnityEngine;
using Zenject;

public class LevelBootstrapper : IInitializable
{
    //private ILevelStateSwitcher levelStateSwitcher;
    //private LevelBootstrapState levelBootstrapState;
    //private LevelGameplayState levelResearchState;

    public LevelBootstrapper(
        //ILevelStateSwitcher levelStateSwitcher,
        //LevelBootstrapState levelBootstrapState,
        //LevelGameplayState levelResearchState
        )
    {
        //this.levelStateSwitcher = levelStateSwitcher;
        //this.levelBootstrapState = levelBootstrapState;
        //this.levelResearchState = levelResearchState;
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

        //levelStateSwitcher.Enter<LevelBootstrapState>();
    }
}