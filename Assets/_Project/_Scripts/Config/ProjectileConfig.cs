using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/Projectile")]
public class ProjectileConfig : ScriptableObject // I created it, but it’s unlikely to be needed
{
    [SerializeField] private ProjectileType _projectileType;
    [SerializeField] private float _shootPower = 8f;
    [SerializeField] private float _maxDragDistance = 2f;

    public ProjectileType ProjectileType => _projectileType;
    public float ShootPower => _shootPower;
    public float MaxDragDistance => _maxDragDistance;
}
