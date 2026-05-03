using System;
using UnityEngine;

public class UnitAnimationEventReceiver : MonoBehaviour
{
    public event Action Ended;
    public event Action Pufed;

    public void End()
    {
        Ended?.Invoke();
    }

    public void Puf()
    {
        Pufed?.Invoke();
    }
}
