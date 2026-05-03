using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
{
    public Coroutine Run(IEnumerator coroutine) =>
        StartCoroutine(coroutine);

	public void SetActive(bool isEnable)
	{
		gameObject.SetActive(isEnable);
	}

	public void Stop(Coroutine coroutine)
    {
        if (this != null && coroutine != null)
            StopCoroutine(coroutine);
    }
}
