using UnityEngine;

public class ProjectileView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private ProjectileConfig _projectileConfig;

    public void Init(ProjectileConfig projectileConfig)
    {
        _projectileConfig = projectileConfig;

        _spriteRenderer.sprite = _projectileConfig.ObjSprite;
    }
}
