using UnityEngine;

public class CameraSize : MonoBehaviour
{
    void Update()
    {
        float baseSize = 20.1f;
        float targetAspect = 16f / 9f;

        float currentAspect = (float)Screen.width / Screen.height;
        Camera.main.orthographicSize = baseSize * (targetAspect / currentAspect);
    }
}
