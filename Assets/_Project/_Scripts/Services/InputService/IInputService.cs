using System;
using UnityEngine;

public interface IInputService
{
    void EnableGameplay();
	void DisableGameplay();

	event Action<Vector2> MousePos;
}
