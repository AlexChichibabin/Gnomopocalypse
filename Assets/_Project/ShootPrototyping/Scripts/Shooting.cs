using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Shooting : MonoBehaviour
{
	public Transform anchor;
	public float power = 8f;
	public float maxDragDistance = 2f;

	private Rigidbody2D rb;
	private Camera cam;
	private bool dragging;

	private IInputService input;

	Vector2 mousePos;

	[Inject]
	public void Construct(IInputService input)
	{
		this.input = input;
	}
	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		cam = Camera.main;
		rb.bodyType = RigidbodyType2D.Kinematic;
		input.MousePos += OnMouseMove;

	}

	private void OnMouseMove(Vector2 pos)
	{
		mousePos = pos;
	}

	void OnMouseDown()
	{
		dragging = true;
	}

	void OnMouseDrag()
	{
		if (!dragging) return;

		Vector2 mouseWorld = cam.ScreenToWorldPoint(mousePos);
		Vector2 anchorPos = anchor.position;

		Vector2 dragVector = mouseWorld - anchorPos;
		dragVector = Vector2.ClampMagnitude(dragVector, maxDragDistance);

		transform.position = anchorPos + dragVector;
	}

	void OnMouseUp()
	{
		dragging = false;

		Vector2 anchorPos = anchor.position;
		Vector2 releasePos = transform.position;

		Vector2 forceDir = anchorPos - releasePos;

		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.linearVelocity = Vector2.zero;
		rb.angularVelocity = 0f;
		rb.AddForce(forceDir * power, ForceMode2D.Impulse);
	}
}

