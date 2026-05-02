using System;

public interface IPlayerHealth
{
	int Health { get; }

	event Action OnDeath;
	event Action<int> OnHealthChanged;

	void ApplyDamage(int count);
	void RestoreHealth();
}