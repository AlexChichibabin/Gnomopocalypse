using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Configs/Unit")]
public class UnitConfig : ScriptableObject
{
	[SerializeField] private UnitType _unitType;
	[SerializeField] private float _startHealth = 100f;
	[SerializeField] private float _startMoveSpeed = 1f;
	[SerializeField] private float _mainDamagePercent = 25f;
	[SerializeField] private float _secondaryDamagePercent = 10f;
    [SerializeField] private float _spawnProbability = 25f;
    [SerializeField] private float _transformationProbability = 25f;
    [SerializeField] private float _minStayTime = 20f;

	public UnitType UnitType => _unitType;
	public float StartHealth => _startHealth;
	public float StartMoveSpeed => _startMoveSpeed;
	public float MainDamagePercent => _mainDamagePercent;
	public float SecondaryDamagePercent => _secondaryDamagePercent;
	public float SpawnProbability => _spawnProbability;
	public float TransformationProbability => _transformationProbability;
	public float MinStayTime => _minStayTime;
}
