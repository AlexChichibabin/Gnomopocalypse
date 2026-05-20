using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ProjectileSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _frame;
    [SerializeField] private GameObject _description;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private SquareDescriptionSize _descriptionSize;
    [SerializeField] private float _boopScale = 1.15f;
    [SerializeField] private float _boopUpDuration = 0.08f;
    [SerializeField] private float _boopDownDuration = 0.12f;

    private ProjectileConfig _projectileConfig;
    private ProjectileSlotPool _pool;
    private bool _isDespawned;
    private Vector3 _initialScale;
    private Sequence _despawnSequence;

    public event Action<ProjectileSlot> Selected;
    public event Action<ProjectileSlot> PointerEnter;

    public ProjectileConfig ProjectileConfig => _projectileConfig;

    private void Awake()
    {
        _initialScale = transform.localScale;
        _button.onClick.AddListener(SlotSelected);
    }

    private void OnDestroy()
    {
        _despawnSequence?.Kill();
        _button.onClick.RemoveListener(SlotSelected);
    }

    public void FillSlot(ProjectileConfig projectileConfig)
    {
        _projectileConfig = projectileConfig;
        _image.sprite = _projectileConfig.UiSprite;
        
        if (_descriptionText != null)
            _descriptionText.text = _projectileConfig.Description;
        
        _descriptionSize?.Rebuild();
        UnHighlight();
    }

    private void SlotSelected()
    {
        Selected?.Invoke(this);
    }

    public void Highlight()
    {
        _frame.SetActive(true);

        if (_description != null)
            _description.SetActive(true);

        _descriptionSize?.Rebuild();
    }

    public void UnHighlight()
    {
        _frame.SetActive(false);
        
        if (_description != null)
            _description.SetActive(false);
    }

    public void DespawnSlot()
    {
        if (_isDespawned)
            return;

        _isDespawned = true;

        if (!isActiveAndEnabled)
        {
            DespawnImmediately();
            return;
        }

        StartCoroutine(DespawnSlotRoutine());
    }

    private IEnumerator DespawnSlotRoutine()
    {
        if (_button != null)
            _button.interactable = false;

        _despawnSequence?.Kill();
        transform.localScale = _initialScale;

        _despawnSequence = DOTween.Sequence()
            .Append(transform.DOScale(_initialScale * _boopScale, _boopUpDuration).SetEase(Ease.OutBack))
            .Append(transform.DOScale(Vector3.zero, _boopDownDuration).SetEase(Ease.InBack));

        yield return _despawnSequence.WaitForCompletion();

        _despawnSequence = null;
        DespawnImmediately();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnHighlight();
    }

    private void OnSpawned(ProjectileConfig projectileConfig)
    {
        _isDespawned = false;
        transform.localScale = _initialScale;

        if (_button != null)
            _button.interactable = true;

        FillSlot(projectileConfig);
    }

    private void OnDespawned()
    {
        _despawnSequence?.Kill();
        _despawnSequence = null;
        transform.localScale = _initialScale;

        if (_button != null)
            _button.interactable = true;

        Selected = null;
        PointerEnter = null;
        _projectileConfig = null;
        _image.sprite = null;

        if (_descriptionText != null)
            _descriptionText.text = string.Empty;

        UnHighlight();
    }

    private void DespawnImmediately()
    {
        if (_pool == null)
        {
            Debug.LogError("[ProjectileSlot] Pool is missing");
            gameObject.SetActive(false);
            return;
        }

        _pool.Despawn(this);
    }


    public class ProjectileSlotPool : MonoMemoryPool<ProjectileConfig, ProjectileSlot>
    {
        protected override void Reinitialize(ProjectileConfig projectileConfig, ProjectileSlot slot)
        {
            slot._pool = this;
            slot.OnSpawned(projectileConfig);
        }

        protected override void OnDespawned(ProjectileSlot slot)
        {
            slot.OnDespawned();
            base.OnDespawned(slot);
        }
    }
}
