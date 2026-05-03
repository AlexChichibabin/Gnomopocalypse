using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class ProjectileTrigger : MonoBehaviour
{
    [SerializeField] private Shooting _shooting;

    private ProjectileType _projectileType;

    public void Init(ProjectileConfig projectileConfig)
    {
        _projectileType = projectileConfig.ProjectileType;
    }

    void Awake()
    {
        var collider = GetComponent<Collider2D>();
        collider.isTrigger = true;

        if(_shooting == null)
        _shooting = GetComponent<Shooting>();
    }

    void OnTriggerEnter2D(Collider2D collision)// temp/ todo -> if unit in collider at the moment of explosion
    {

        if (collision.TryGetComponent<Unit>(out var unit)  && _shooting.IsMoving)
        {
            if (CompatibilityExtention.IsMainDamage(unit.UnitType, _projectileType))
                unit.DealMainDamage();
            else
                unit.DealSecondaryDamage();
        }
    }
}
