using System.Collections;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        NextParent = transform.parent;
        CardImage.raycastTarget = false;
        //ChangeState(false);
        GameEventManager.InvokeDragBegin(transform.parent.GetSiblingIndex());
        transform.SetParent(GameManager.Instance.MainCanvas.transform);
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Card_RectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Card_RectTransform.position = Input.mousePosition;
        transform.SetParent(NextParent);
        transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.Linear);
        CardImage.raycastTarget = true;

        StartCoroutine(FrameWait());

        IEnumerator FrameWait()
        {
            yield return new WaitForEndOfFrame();
            _isDragging = false;
        }
        
        if (_isSelected) ChangeState();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameEventManager.InvokeDragEnd(transform.parent.GetSiblingIndex());
        CardContainer parent = transform.GetComponentInParent<CardContainer>();

        GameManager.Instance.GetNeighBoruCardContainer(transform.parent.GetSiblingIndex())
            .MoveCard(this);

        Card item = eventData.pointerDrag.GetComponent<Card>();
        parent.MoveCard(item);
        item.NextParent = parent.transform;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isDragging) return;
        ChangeState();
        GameEventManager.InvokeCardClicked(this, _isSelected, transform.parent.GetSiblingIndex());
    }

    public void LoadCardTexture(Sprite cardImage, string cardName)
    {
        CardImage.sprite = cardImage;
        _cardName = cardName;
    }

    private void ChangeState(bool move = true)
    {
        if (_isDragging) return;
        _isSelected = !_isSelected;
        CardOutline.enabled = _isSelected;
        if (move)
            transform.localPosition += Vector3.up * OffSet * (_isSelected ? 1 : -1);
    }

    public void ShufflingCard(bool grouping = false)
    {
        _isSelected = false;
        CardOutline.enabled = _isSelected;
        if (!grouping)
            GameEventManager.InvokeCardClicked(this, _isSelected, transform.parent.GetSiblingIndex());
    }
}