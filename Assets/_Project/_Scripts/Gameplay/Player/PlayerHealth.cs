using System;
using UnityEngine;
using Zenject;

public class PlayerHealth : IPlayerHealth
{
	public int Health => health;

	public event Action<int> OnHealthChanged;
	public event Action OnDeath;

	private int maxDamage = 2;
	private int maxHealth = 3;
	private int health = 3;

	[Inject] private IAudioService audioService;

	public void ApplyDamage(int count)
	{
		if (count <= 0) return;
		count = Mathf.Clamp(count, 1, maxDamage);

		if (health > 0 ) audioService.PlaySound(SoundId.HitPlayer);
		health -= count;
		health = Mathf.Clamp(health, 0, maxHealth);
		OnHealthChanged?.Invoke(health);
		
		if (health <= 0)
		{
			health = 0;
			OnDeath?.Invoke();
		}
	}
	public void RestoreHealth()
	{
		health = maxHealth;
		OnHealthChanged?.Invoke(health);
	}
}
