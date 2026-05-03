using System;

public interface IPauseState
{
	event Action<bool> IsPaused;

	void Construct(ILevelStateMachine levelStateMachine, IInputService inputService);
	void SwitchPause();
	void Pause();
	void UnPause();
}