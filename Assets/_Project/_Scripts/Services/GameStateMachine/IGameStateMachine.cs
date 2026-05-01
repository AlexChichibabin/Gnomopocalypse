using System;

public interface IGameStateMachine
{
	GameState State { get; }

	event Action<GameState> StateChanged;
	void ApplyState(GameState state);
}