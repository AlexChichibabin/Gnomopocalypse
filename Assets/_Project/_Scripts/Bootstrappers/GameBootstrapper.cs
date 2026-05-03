using UnityEngine;
using Zenject;

public class GameBootstrapper : IInitializable
{
    private IGameStateMachine gameStateMachine;


    public GameBootstrapper(
        IGameStateMachine gameStateMachine
        )
    {
        this.gameStateMachine = gameStateMachine;
    }

    public void Initialize()
    {
        //Debug.Log("GLOBAL: Boot");
        InitGameStateMachine();
    }

    private void InitGameStateMachine()
    {
        gameStateMachine.ApplyState(GameState.Bootstrap);
    }
}