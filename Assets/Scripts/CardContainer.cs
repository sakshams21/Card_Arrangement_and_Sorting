using DG.Tweening;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    public Card Ref_Card;
    public bool ToBeShifted;
    public void UpdateCardDetails(Sprite cardImage, string cardName)
    {
        Ref_Card.LoadCardTexture(cardImage, cardName);
    }

    public void MoveCard(Card cardToMove, bool isGrouping = false)
    {
        Ref_Card = cardToMove;
        Ref_Card.ShufflingCard(isGrouping);
        cardToMove.transform.SetParent(transform);
        cardToMove.transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.Linear);
    }
}