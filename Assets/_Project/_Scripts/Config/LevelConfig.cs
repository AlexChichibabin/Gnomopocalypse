using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level")]
public class LevelConfig : ScriptableObject
{
	public string SceneName;
	public int UnitCountToWin;

	public List<EnemySpawnerData> enemySpawnerDatas;
}

[System.Serializable]
public class EnemySpawnerData
{
	//public EnemyId Id;
	public Vector3 position;
}
