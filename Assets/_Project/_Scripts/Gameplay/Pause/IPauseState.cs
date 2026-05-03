using System;

public interface IPauseState
{
	event Action<bool> IsPausedEvent;
	bool IsPaused { get; }
	void Construct(ILevelStateMachine levelStateMachine, IInputService inputService);
	void SwitchPause();
	void Pause();
	void UnPause();
}