using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level")]
public class LevelConfig : ScriptableObject
{
	public string SceneName;
	public Vector3 HeroSpawnPoint;
	//public Vector3 FinishPoint;

	public List<EnemySpawnerData> enemySpawnerDatas;
}

[System.Serializable]
public class EnemySpawnerData
{
	//public EnemyId Id;
	public Vector3 position;
}