using System.Collections;
using UnityEngine;

public class LearningText : MonoBehaviour
{
    void Start()
    {
		gameObject.SetActive(true);
		StartCoroutine(TimerToRemoveText(15));
	}
    IEnumerator TimerToRemoveText(int dur)
    {
        yield return new WaitForSeconds(dur);

        gameObject.SetActive(false);
    }

}
