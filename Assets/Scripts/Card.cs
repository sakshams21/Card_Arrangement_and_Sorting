using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private RectTransform Card_RectTransform;
    [SerializeField] private Image CardImage;
    [SerializeField] private Outline CardOutline;
    [SerializeField] private float OffSet;

    public Transform NextParent;
    private string _cardName;
    private bool _isDragging;

    private bool _isSelected;

    public static event Action<Card, bool> OnCardClicked;
    public static event Action<int> OnDragBegin;
    public static event Action<int> OnDragEnd;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isSelected) return;
        NextParent = transform.parent;
        CardImage.raycastTarget = false;
        OnDragBegin?.Invoke(transform.parent.GetSiblingIndex());
        transform.SetParent(GameManager.Instance.MainCanvas.transform);
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isSelected) return;
        Card_RectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isSelected) return;
        Card_RectTransform.position = Input.mousePosition;
        transform.SetParent(NextParent);
        transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        CardImage.raycastTarget = true;
        _isDragging = false;
        if (_isSelected) ChangeState();
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDragEnd?.Invoke(transform.parent.GetSiblingIndex());
        CardContainer parent = transform.GetComponentInParent<CardContainer>();

        GameManager.Instance.GetNeighBoruCardContainer(transform.parent.GetSiblingIndex())
            .MoveCard(this);

        Card item = eventData.pointerDrag.GetComponent<Card>();
        parent.MoveCard(item);
        item.NextParent = parent.transform;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ChangeState();
        OnCardClicked?.Invoke(this, _isSelected);
    }

    public void LoadCardTexture(Sprite cardImage, string cardName)
    {
        CardImage.sprite = cardImage;
        _cardName = cardName;
    }

    private void ChangeState()
    {
        if (_isDragging) return;
        _isSelected = !_isSelected;
        CardOutline.enabled = _isSelected;
        transform.localPosition += Vector3.up * OffSet * (_isSelected ? 1 : -1);
    }

    public void MoveCard(int newIndex)
    {
        ChangeState();
        transform.parent.SetSiblingIndex(newIndex);
    }
}