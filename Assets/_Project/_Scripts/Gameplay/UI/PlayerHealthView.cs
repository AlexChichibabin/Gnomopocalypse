using System;
using UnityEngine;
using Zenject;

public class PlayerHealthView : MonoBehaviour
{
	[SerializeField] private GameObject[] healthViews;

    private IPlayerHealth health;

	[Inject]
    public void Construct(IPlayerHealth health)
    {
        this.health = health;

		health.OnHealthChanged += OnHealthChanged;
    }
	private void OnDestroy()
	{
		health.OnHealthChanged -= OnHealthChanged;
	}

	private void OnHealthChanged(int value)
	{
		for (int i = 0; i < healthViews.Length; i++)
		{
			if (i + 1 == value) healthViews[i].SetActive(true);
			else healthViews[i].SetActive(false);
		}
	}

	[ContextMenu("TestApplyDamage")]
	public void TestApplyDamage()
	{
		Debug.Log("daamge");
		health.ApplyDamage(1);
	}
	[ContextMenu("TestRestoreHealth")]
	public void TestRestoreHealth()
	{
		health.RestoreHealth();
	}
}
