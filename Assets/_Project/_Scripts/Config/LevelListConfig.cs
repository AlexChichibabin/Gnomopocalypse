using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelListConfig", menuName = "Configs/LevelListConfig")]
public class LevelListConfig : ScriptableObject
{
    public LevelConfig[] Levels;
}
