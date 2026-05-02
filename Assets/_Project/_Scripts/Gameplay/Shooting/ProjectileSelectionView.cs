using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ProjectileSelectionView : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private RectTransform[] _slots;
    [SerializeField] private float _fallDuration = 0.35f;
    [SerializeField] private Ease _fallEase = Ease.OutCubic;

    private ProjectileSelection _projectileSelection;
    private LayoutGroup _layoutGroup;
    private ContentSizeFitter _contentSizeFitter;
    private Sequence _fallSequence;
    private Coroutine _initialRenderRoutine;
    private bool _isConstructed;
    private bool _isReadyToAnimate;
    private bool _hasRenderedStock;

    [Inject]
    private void Construct(ProjectileSelection projectileSelection)
    {
        _projectileSelection = projectileSelection;
        _projectileSelection.StockChanged += OnStockChanged;
        _isConstructed = true;
    }

    private void Awake()
    {
        CacheSlots();
        CacheLayoutComponents();
    }

    private void Start()
    {
        if (_isConstructed)
            _initialRenderRoutine = StartCoroutine(RenderInitialStock());
    }

    private void OnDestroy()
    {
        _fallSequence?.Kill();
        SetLayoutEnabled(true);

        if (_initialRenderRoutine != null)
            StopCoroutine(_initialRenderRoutine);

        if (_slots != null)
        {
            foreach (RectTransform slot in _slots)
                slot?.DOKill();
        }

        if (_projectileSelection != null)
            _projectileSelection.StockChanged -= OnStockChanged;
    }

    private void OnStockChanged(IReadOnlyList<ProjectileConfig> projectileStock)
    {
        if (!_isReadyToAnimate)
            return;

        CacheSlots();

        bool hasStock = projectileStock.Count > 0;

        if (_hasRenderedStock && hasStock)
            MoveBottomSlotToTopAnimated();
        else if (hasStock)
            _hasRenderedStock = true;

        RenderStock(projectileStock);
    }

    private IEnumerator RenderInitialStock()
    {
        yield return null;
        RebuildLayout();
        RenderStock(_projectileSelection.ProjectileStock);
        _hasRenderedStock = _projectileSelection.ProjectileStock.Count > 0;
        _isReadyToAnimate = true;
        _initialRenderRoutine = null;
    }

    private void RenderStock(IReadOnlyList<ProjectileConfig> projectileStock)
    {
        for (int slotIndex = 0; slotIndex < _slots.Length; slotIndex++)
        {
            int stockIndex = _slots.Length - 1 - slotIndex;
            ProjectileConfig projectileConfig = stockIndex < projectileStock.Count ? projectileStock[stockIndex] : null;
            Image slotImage = GetSlotImage(_slots[slotIndex]);

            if (slotImage == null)
                continue;

            slotImage.sprite = projectileConfig != null ? projectileConfig.UiSprite : null;
            slotImage.enabled = projectileConfig != null && projectileConfig.UiSprite != null;
        }
    }

    private void MoveBottomSlotToTop()
    {
        if (_slots.Length == 0)
            return;

        RectTransform bottomSlot = _slots[_slots.Length - 1];

        for (int i = _slots.Length - 1; i > 0; i--)
            _slots[i] = _slots[i - 1];

        _slots[0] = bottomSlot;
        bottomSlot.SetSiblingIndex(0);
    }

    private void MoveBottomSlotToTopAnimated()
    {
        if (_slots.Length == 0)
            return;

        Vector3[] oldWorldPositions = new Vector3[_slots.Length];

        for (int i = 0; i < _slots.Length; i++)
            oldWorldPositions[i] = _slots[i].position;

        MoveBottomSlotToTop();
        SetLayoutEnabled(true);
        RebuildLayout();

        Vector2[] targetPositions = new Vector2[_slots.Length];

        for (int i = 0; i < _slots.Length; i++)
            targetPositions[i] = _slots[i].anchoredPosition;

        _slots[0].anchoredPosition = targetPositions[0];

        for (int i = 1; i < _slots.Length; i++)
            _slots[i].position = oldWorldPositions[i - 1];

        SetLayoutEnabled(false);

        _fallSequence?.Kill();
        _fallSequence = DOTween.Sequence();

        for (int i = 1; i < _slots.Length; i++)
        {
            RectTransform slot = _slots[i];
            slot.DOKill();
            _fallSequence.Join(slot.DOAnchorPos(targetPositions[i], _fallDuration).SetEase(_fallEase));
        }

        _fallSequence.OnComplete(() =>
        {
            for (int i = 0; i < _slots.Length; i++)
                _slots[i].anchoredPosition = targetPositions[i];

            SetLayoutEnabled(true);
            RebuildLayout();
        });
    }

    private void RebuildLayout()
    {
        if (_content is RectTransform contentRect)
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }

    private void SetLayoutEnabled(bool isEnabled)
    {
        CacheLayoutComponents();

        if (_layoutGroup != null)
            _layoutGroup.enabled = isEnabled;

        if (_contentSizeFitter != null)
            _contentSizeFitter.enabled = isEnabled;
    }

    private void CacheLayoutComponents()
    {
        if (_content == null)
            return;

        if (_layoutGroup == null)
            _layoutGroup = _content.GetComponent<LayoutGroup>();

        if (_contentSizeFitter == null)
            _contentSizeFitter = _content.GetComponent<ContentSizeFitter>();
    }

    private void CacheSlots()
    {
        if (_slots != null && _slots.Length > 0)
            return;

        Transform content = _content != null ? _content : transform;
        List<RectTransform> slots = new();

        for (int i = 0; i < content.childCount; i++)
        {
            Transform slot = content.GetChild(i);
            RectTransform slotRect = slot as RectTransform;

            if (slotRect != null)
                slots.Add(slotRect);
        }

        _slots = slots.ToArray();
    }

    private Image GetSlotImage(RectTransform slot)
    {
        if (slot == null || slot.childCount == 0)
            return null;

        return slot.GetChild(0).GetComponent<Image>();
    }
}
