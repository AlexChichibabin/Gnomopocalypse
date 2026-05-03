using UnityEngine;

public class UnitsSpawnSettings : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnRadius = 3f;

    public Vector3 SpawnPoint => _spawnPoint != null ? _spawnPoint.position : transform.position;
    public float SpawnRadius => _spawnRadius;


#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(SpawnPoint, SpawnRadius);
	}
#endif
}
