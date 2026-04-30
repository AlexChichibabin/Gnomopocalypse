using UnityEngine;
using Zenject;

public class GameBootstrapper : IInitializable
{
    private IGameStateSwitcher gameStateSwitcher;
    private GameBootstrapState gameBootstrapState;
    private LoadNextLevelState loadNextLevelState;


    public GameBootstrapper(
        IGameStateSwitcher gameStateSwitcher, 
        GameBootstrapState gameBootstrapState, 
        LoadNextLevelState loadNextLevelState
        )
    {
        this.gameStateSwitcher = gameStateSwitcher;
        this.gameBootstrapState = gameBootstrapState;
        this.loadNextLevelState = loadNextLevelState;
    }

    public void Initialize()
    {
        Debug.Log("GLOBAL: Boot");
        InitGameStateMachine();
    }

    private void InitGameStateMachine()
    {
        gameStateSwitcher.AddState(gameBootstrapState);
        gameStateSwitcher.AddState(loadNextLevelState);

        gameStateSwitcher.Enter<GameBootstrapState>();
    }
}