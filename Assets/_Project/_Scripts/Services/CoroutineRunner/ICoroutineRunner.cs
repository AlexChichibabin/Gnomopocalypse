using System.Collections;
using UnityEngine;

public interface ICoroutineRunner
{
    Coroutine Run(IEnumerator coroutine);
    void Stop(Coroutine coroutine);
}
