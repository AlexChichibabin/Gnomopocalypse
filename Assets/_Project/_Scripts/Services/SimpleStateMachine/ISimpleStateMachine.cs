public interface ISimpleStateMachine
{
	LevelState State { get; }

	void ApplyState(LevelState state);
}