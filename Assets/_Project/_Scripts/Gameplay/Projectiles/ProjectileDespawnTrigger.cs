using UnityEngine;

public class ProjectileDespawnTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Projectile>(out var projectile) &&
             collision.GetComponent<Shooting>().IsMoving)
            projectile.Despawn();
    }
}
