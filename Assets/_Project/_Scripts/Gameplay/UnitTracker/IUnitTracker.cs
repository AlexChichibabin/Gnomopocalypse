using System;

public interface IUnitTracker
{
	event Action OnAllUnitDeath;

	void Init();
	void ResetDeaths();
	void AddUnitDeath();
}