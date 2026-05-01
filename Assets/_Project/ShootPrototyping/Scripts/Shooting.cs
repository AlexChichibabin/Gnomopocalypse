using UnityEngine;
using Zenject;

public class Shooting : MonoBehaviour
{
	private Transform anchor;
	public float power = 8f;
	public float maxDragDistance = 2f;

	private Rigidbody2D rb;
	private Camera cam;
	private bool dragging;

	private IInputService input;

	public bool IsMoving;

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
	
    public void Init(Transform transform)
    {
        anchor = transform;
		IsMoving = false;
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
		if(anchor == null) Debug.LogError("[Shooting] not inited!");

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

		IsMoving = true;
	}


}

