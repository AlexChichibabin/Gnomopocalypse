using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitTracker : IUnitTracker
{
	public event Action OnAllUnitDeath;
	public int UnitDeaths;

	private int allUnitCount;

	private IConfigProvider configProvider;

	public UnitTracker(
		IConfigProvider configProvider)
	{
		this.configProvider = configProvider;
	}
	public void Init()
	{
		allUnitCount = configProvider.GetLevel(SceneManager.GetActiveScene().name).UnitCountToWin;
	}
	public void AddUnitDeath()
	{
		UnitDeaths++;

		if (UnitDeaths >= allUnitCount)
			OnAllUnitDeath?.Invoke();
	}
	public void ResetDeaths()
	{
		UnitDeaths = 0;
	}

}
