using UnityEngine;

public class ProjectileDespawnTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Projectile>(out var projectile))
        projectile.Despawn();
    }
}
