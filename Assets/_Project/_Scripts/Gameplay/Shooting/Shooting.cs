using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Shooting : MonoBehaviour
{
    [SerializeField] private float _power = 8f;
    [SerializeField] private float _maxDragDistance = 2f;

    [Inject] private IAudioService audioService;

    private Transform _anchor;
    private Rigidbody2D _rigidbody;
    private Camera _camera;
    private IInputService _input;
    private Vector2 _mousePosition;
    private bool _dragging;
    private bool _isSubscribed;

    public bool IsMoving { get; private set; }

    public event Action Released;

    [Inject]
    public void Construct(IInputService input)
    {
        _input = input;
        SubscribeInput();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        SubscribeInput();
    }

    private void OnDisable()
    {
        UnsubscribeInput();
    }

    public void Init(Transform anchor, ProjectileConfig projectileConfig)
    {
        _anchor = anchor;
        _power = projectileConfig.ShootPower;
        _maxDragDistance = projectileConfig.MaxDragDistance;
        IsMoving = false;
        _dragging = false;

        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;

        // if (_anchor != null)
        //     transform.position = _anchor.position;
    }

    private void OnMouseMove(Vector2 position)
    {
        _mousePosition = position;
    }

    private void OnMouseDown()
    {
        if (IsMoving)
            return;

        _dragging = true;
    }

    private void OnMouseDrag()
    {
        if (!_dragging)
            return;

        if (_anchor == null)
        {
            Debug.LogError("[Shooting] Not initialized");
            return;
        }

        Vector2 mouseWorld = _camera.ScreenToWorldPoint(_mousePosition);
        Vector2 anchorPosition = _anchor.position;

        Vector2 dragVector = mouseWorld - anchorPosition;
        dragVector = Vector2.ClampMagnitude(dragVector, _maxDragDistance);

        transform.position = anchorPosition + dragVector;
    }

    private void OnMouseUp()
    {
        if (!_dragging || _anchor == null)
            return;

        _dragging = false;

        Vector2 anchorPosition = _anchor.position;
        Vector2 releasePosition = transform.position;
        Vector2 forceDirection = anchorPosition - releasePosition;

        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.AddForce(forceDirection * _power, ForceMode2D.Impulse);
        //audioService.PlaySound(SoundId.Shooting);

		IsMoving = true;
        Released?.Invoke();
    }

    private void SubscribeInput()
    {
        if (_input == null || _isSubscribed)
            return;

        _input.MousePos += OnMouseMove;
        _isSubscribed = true;
    }

    private void UnsubscribeInput()
    {
        if (_input == null || !_isSubscribed)
            return;

        _input.MousePos -= OnMouseMove;
        _isSubscribed = false;
    }
}
