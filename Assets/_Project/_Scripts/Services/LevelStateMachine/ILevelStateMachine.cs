using System;

public interface ILevelStateMachine
{
	LevelState State { get; }
	event Action<LevelState> StateChanged;
	void ApplyState(LevelState state);
}