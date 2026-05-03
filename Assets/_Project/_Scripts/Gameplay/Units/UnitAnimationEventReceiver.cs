using System;
using UnityEngine;

public class UnitAnimationEventReceiver : MonoBehaviour
{
    public event Action Ended;

    public void End()
    {
        Ended?.Invoke();
    }
}
