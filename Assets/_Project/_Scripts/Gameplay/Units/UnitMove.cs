using UnityEngine;

public class UnitMove : MonoBehaviour
{
    private float _startMoveSpeed = 1;

    private float _currentMoveSpeed;

    private bool _canMove;

    public void Init(float startMoveSpeed)
    {
        _startMoveSpeed = startMoveSpeed;
        _currentMoveSpeed = startMoveSpeed;

        Run();
    }

    private void Update()
    {
        if (_canMove)
            transform.position += Vector3.left * _currentMoveSpeed * Time.deltaTime;
    }

    public void Immobilize() =>
     _canMove = false;

    public void Run() =>
     _canMove = true;

    public void ResetSpeed() =>
    _currentMoveSpeed = _startMoveSpeed;

    public void IncreaseSpeed(float value) =>
    _currentMoveSpeed += value;

    public void DecreaseSpeed(float value) =>
    _currentMoveSpeed -= value;

    public void HardChangeSpeed(float value) =>
    _currentMoveSpeed = value;

}
