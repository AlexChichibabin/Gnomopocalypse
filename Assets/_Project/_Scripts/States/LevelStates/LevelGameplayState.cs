using UnityEngine;

public class LevelGameplayState : IEnterableState, ITickableState, IExitableState
{
    public void Enter()
    {
        Debug.Log("LEVEL: Gameplay");

        // 
	}
    public void Exit() { }

    public void Tick() { }
}