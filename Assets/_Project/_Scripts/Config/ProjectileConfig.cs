using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/Projectile")]
public class ProjectileConfig : ScriptableObject
{
    [SerializeField] private ProjectileType _projectileType;
    [SerializeField] private float _shootPower = 8f;
    [SerializeField] private float _maxDragDistance = 2f;
    [SerializeField] private Sprite _uiSprite;
    [SerializeField] private Sprite _objSprite;


    public ProjectileType ProjectileType => _projectileType;
    public float ShootPower => _shootPower;
    public float MaxDragDistance => _maxDragDistance;
    public Sprite UiSprite => _uiSprite;
    public Sprite ObjSprite => _objSprite;
}
