using UnityEngine;
using Zenject;

public class UnitPurpose : MonoBehaviour
{

    private IPlayerHealth health;

    [Inject]
    public void Construct(IPlayerHealth health)
    {
        this.health = health;
    }
	private void OnTriggerEnter2D(Collider2D other)
	{
        if (other.TryGetComponent(out Unit unit) == false) return;

        health.ApplyDamage(1);
	}

}
