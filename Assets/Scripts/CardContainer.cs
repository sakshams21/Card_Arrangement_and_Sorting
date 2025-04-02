using DG.Tweening;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    public Card Ref_Card;

    public void UpdateCardDetails(Sprite cardImage, string cardName)
    {
        Ref_Card.LoadCardTexture(cardImage, cardName);
    }

    public void MoveCard(Card cardToMove)
    {
        Ref_Card = cardToMove;
        cardToMove.transform.SetParent(transform);
        Vector3 localPosition = cardToMove.transform.localPosition;
        localPosition.x = 0;
        cardToMove.transform.DOLocalMove(localPosition, 0.5f).SetEase(Ease.Linear);
    }
}