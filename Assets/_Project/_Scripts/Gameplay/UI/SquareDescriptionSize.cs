using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SquareDescriptionSize : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Vector2 _padding = new(20f, 16f);
    [SerializeField] private float _widthToHeightRatio = 1.5f;
    [SerializeField] private float _minHeight = 100f;
    [SerializeField] private float _maxHeight = 360f;

    private RectTransform _rectTransform;
    private RectTransform _textRectTransform;

    private void Awake()
    {
        CacheComponents();
    }

    private void OnEnable()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        CacheComponents();

        if (_text == null || _rectTransform == null)
            return;

        ApplyTextPadding();
        _text.ForceMeshUpdate();

        float height = CalculateHeight();
        float width = height * GetSafeRatio();

        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }

    private float CalculateHeight()
    {
        float minHeight = Mathf.Max(1f, _minHeight);
        float maxHeight = Mathf.Max(minHeight, _maxHeight);
        float low = minHeight;
        float high = maxHeight;

        for (int i = 0; i < 12; i++)
        {
            float height = (low + high) * 0.5f;

            if (DoesTextFit(height))
                high = height;
            else
                low = height;
        }

        return high;
    }

    private bool DoesTextFit(float height)
    {
        float width = height * GetSafeRatio();
        float availableWidth = Mathf.Max(1f, width - _padding.x);
        float availableHeight = Mathf.Max(1f, height - _padding.y);
        Vector2 preferredSize = _text.GetPreferredValues(_text.text, availableWidth, Mathf.Infinity);

        return preferredSize.x <= availableWidth + 0.5f && preferredSize.y <= availableHeight + 0.5f;
    }

    private float GetSafeRatio()
    {
        return Mathf.Max(0.01f, _widthToHeightRatio);
    }

    private void ApplyTextPadding()
    {
        if (_textRectTransform == null)
            return;

        float horizontalPadding = _padding.x * 0.5f;
        float verticalPadding = _padding.y * 0.5f;

        _textRectTransform.anchorMin = Vector2.zero;
        _textRectTransform.anchorMax = Vector2.one;
        _textRectTransform.offsetMin = new Vector2(horizontalPadding, verticalPadding);
        _textRectTransform.offsetMax = new Vector2(-horizontalPadding, -verticalPadding);
    }

    private void CacheComponents()
    {
        if (_rectTransform == null)
            _rectTransform = (RectTransform)transform;

        if (_text == null)
            _text = GetComponentInChildren<TMP_Text>(true);

        if (_text != null && _textRectTransform == null)
            _textRectTransform = _text.transform as RectTransform;
    }
}
