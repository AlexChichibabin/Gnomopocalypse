using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : IInputService, IDisposable
{
	public event Action<Vector2> MousePos;

	private PlayerActions input;


	public InputService(PlayerActions input)
	{
		this.input = input;

		this.input.Gameplay.MousePos.performed += OnMouseMove;
	}

	public void EnableGameplay() => input.Gameplay.Enable();
	public void DisableGameplay() => input.Gameplay.Disable();

	private void OnMouseMove(InputAction.CallbackContext context)
	{
		Vector2 mousePos = context.ReadValue<Vector2>();
		MousePos?.Invoke(mousePos);
	}

    public void Dispose()
	{

	}
}
