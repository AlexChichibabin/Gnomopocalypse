using System.Collections;
using UnityEngine;

public interface ICoroutineRunner
{
    void SetActive(bool isEnable);
    Coroutine Run(IEnumerator coroutine);
    void Stop(Coroutine coroutine);
}
