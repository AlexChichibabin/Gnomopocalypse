using System;
using UnityEngine;

[Serializable]
public class SpawnRateStep 
{
    [SerializeField] private float _minute;
    [SerializeField] private float _unitsPerMinute;

    public float Minute => _minute;
    public float UnitsPerMinute => _unitsPerMinute;
}
