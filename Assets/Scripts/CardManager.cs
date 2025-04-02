using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class DeckData
{
    public List<string> deck;
}

[Serializable]
public class RootData
{
    public DeckData data;
}

public class CardManager : MonoBehaviour
{
    [SerializeField] private TextAsset CardLoadInfo;
    [SerializeField] private CardData Ref_CardData;
    [SerializeField] private CardContainer Card_Prefab;
    [SerializeField] private CardContainer EmptyPrefab;
    [SerializeField] private RectTransform Card_SpawnPoint;

    private List<Card> _currentlySelected = new List<Card>();
    private List<CardContainer> _allCardParentList = new List<CardContainer>();

    private int _index1 = 0;
    private int _index2 = 0;

    private void Start()
    {
        Ref_CardData.StoreData();
        RootData cardList = JsonUtility.FromJson<RootData>(CardLoadInfo.text);
        foreach (string cardName in cardList.data.deck)
        {
            CardContainer spawnedCard = Instantiate(Card_Prefab, Card_SpawnPoint);
            spawnedCard.UpdateCardDetails(Ref_CardData.GetCard(cardName), cardName);
            _allCardParentList.Add(spawnedCard);
        }

        Card.OnCardClicked += OnCardClicked;
        Card.OnDragBegin += StoreStartIndex;
        Card.OnDragEnd += StoreEndIndex;
    }

    public CardContainer GetNeighbor(int current)
    {
        int direction = _index1 - current;
        return direction < 0 ? _allCardParentList[--current] : _allCardParentList[++current];
    }

    private void StoreEndIndex(int index)
    {
        _index2 = index;
        CardMovement();
    }

    private void CardMovement()
    {
        int direction = _index1 - _index2;

        if (direction < 0)
        {
            for (int i = _index1 + 1; i < _index2; i++)
            {
                _allCardParentList[i - 1].MoveCard(_allCardParentList[i].Ref_Card);
                _allCardParentList[i].Ref_Card = null;
            }
        }
        else if (direction > 0)
        {
            for (int i = _index1 - 1; i < _index2; i--)
            {
                _allCardParentList[i + 1].MoveCard(_allCardParentList[i].Ref_Card);
                _allCardParentList[i].Ref_Card = null;
            }
        }
    }

    private void StoreStartIndex(int index)
    {
        _index1 = index;
    }

    private void OnDestroy()
    {
        Card.OnCardClicked -= OnCardClicked;
    }

    private void OnCardClicked(Card cardRef, bool selected)
    {
        if (selected)
            _currentlySelected.Add(cardRef);
        else
            _currentlySelected.Remove(cardRef);

        if (_currentlySelected.Count > 0)
        {
            //show group btn
        }
        else
        {
            //dont show group btn
        }
    }

    [ContextMenu("Group")]
    private void GenerateGroup()
    {
        Instantiate(EmptyPrefab, Card_SpawnPoint);
        Instantiate(EmptyPrefab, Card_SpawnPoint);
        int childCount = Card_SpawnPoint.childCount;
        foreach (Card item in _currentlySelected)
        {
            item.MoveCard(childCount++);
        }

        _currentlySelected.Clear();
    }
}