using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class ProjectileTrigger : MonoBehaviour
{
    private ProjectileType _projectileType;

    public void Init(ProjectileType projectileType)
    {
        _projectileType = projectileType;
    }

    void Awake()
    {
        var collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)// temp/ todo -> if unit in collider at the moment of explosion
    {

        if (collision.TryGetComponent<Unit>(out var unit))
        {
            if (CompatibilityExtention.IsMainDamage(unit.UnitType, _projectileType))
                unit.Damageble.DealMainDamage();
            else
                unit.Damageble.DealSecondaryDamage();
        }
    }
}
