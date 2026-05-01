using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image _barImage;

private void Awake()
{
    if (_barImage == null)
        _barImage = GetComponent<Image>();
}

public void SetValue(float current, float max)
{
    if (_barImage == null)
        return;

    if (max <= 0)
    {
        _barImage.fillAmount = 0;
        return;
    }

    _barImage.fillAmount = Mathf.Clamp01(current / max);
}
}
