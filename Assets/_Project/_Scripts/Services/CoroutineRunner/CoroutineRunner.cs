using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
{
    public Coroutine Run(IEnumerator coroutine) =>
        StartCoroutine(coroutine);

    public void Stop(Coroutine coroutine)
    {
        if (this != null && coroutine != null)
            StopCoroutine(coroutine);
    }
}
