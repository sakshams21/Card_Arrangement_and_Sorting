using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Transform Card_SpawnPoint;

    private List<Card> _currentlySelected = new List<Card>();
    private List<CardContainer> _allCardParentList = new List<CardContainer>();

    private Dictionary<string, Sprite> _cardsData;

    private int _index1 = 0;
    private int _index2 = 0;

    private void Start()
    {
        ManageCardsData();
        SpawnCardFromFile();

        GameEventManager.OnCardClicked += OnCardClicked;
        GameEventManager.OnDragBegin += StoreStartIndex;
        GameEventManager.OnDragEnd += StoreEndIndex;
        GameEventManager.OnButtonPressed += GenerateGroup;
    }

    private void OnDestroy()
    {
        GameEventManager.OnCardClicked -= OnCardClicked;
        GameEventManager.OnDragBegin -= StoreStartIndex;
        GameEventManager.OnDragEnd -= StoreEndIndex;
        GameEventManager.OnButtonPressed -= GenerateGroup;
    }

    private void SpawnCardFromFile()
    {
        RootData cardList = JsonUtility.FromJson<RootData>(CardLoadInfo.text);
        foreach (string cardName in cardList.data.deck)
        {
            CardContainer spawnedCard = Instantiate(Card_Prefab, Card_SpawnPoint);
            spawnedCard.UpdateCardDetails(GetCard(cardName), cardName);
            _allCardParentList.Add(spawnedCard);
        }
    }

    private void ManageCardsData()
    {
        _cardsData = new Dictionary<string, Sprite>();
        foreach (Sprite card in Ref_CardData.cards)
        {
            _cardsData.TryAdd(card.name, card);
        }
    }

    private Sprite GetCard(string cardName)
    {
        return _cardsData[cardName];
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

        //card moved from left to right
        if (direction < 0)
        {
            for (int i = _index1 + 1; i < _index2; i++)
            {
                if (_allCardParentList[i].Ref_Card == null)
                {
                    _allCardParentList[i - 1].Ref_Card = null;
                }
                else
                {
                    _allCardParentList[i - 1].MoveCard(_allCardParentList[i].Ref_Card);
                    _allCardParentList[i].Ref_Card = null;
                }
            }
        }
        else if (direction > 0) //card moved from right to left
        {
            for (int i = _index1 - 1; i > _index2; i--)
            {
                if (_allCardParentList[i].Ref_Card == null)
                {
                    _allCardParentList[i + 1].Ref_Card = null;
                }
                else
                {
                    _allCardParentList[i + 1].MoveCard(_allCardParentList[i].Ref_Card);
                    _allCardParentList[i].Ref_Card = null;
                }
            }
        }

        RemoveEmpty(_index1);
    }

    // check for empty group check from initial place towards left if there are any empty keep 2 and remove rest
    private void RemoveEmpty(int startingIndex)
    {
        int left = startingIndex - 1;
        int right = left + 1;
        int emptyCount = 0;
        while (left >= 0 && right < _allCardParentList.Count)
        {
            bool foundNull = false;

            if (_allCardParentList[left].Ref_Card == null)
            {
                emptyCount++;
                foundNull = true;
                if (emptyCount > 2)
                {
                    GameManager.Instance.BackToPool(_allCardParentList[left]);
                    _allCardParentList.RemoveAt(left);
                }
                else
                {
                    left--;
                }
            }

            if (right < _allCardParentList.Count && _allCardParentList[right].Ref_Card == null)
            {
                emptyCount++;
                foundNull = true;
                if (emptyCount > 2)
                {
                    GameManager.Instance.BackToPool(_allCardParentList[right]);
                    _allCardParentList.RemoveAt(right);
                }
                else
                {
                    right++;
                }
            }

            if (!foundNull) break;
        }
    }

    private void StoreStartIndex(int index)
    {
        _index1 = index;
    }


    private void OnCardClicked(Card cardRef, bool selected, int index)
    {
        _allCardParentList[index].ToBeShifted = selected;
        if (selected)
            _currentlySelected.Add(cardRef);
        else
            _currentlySelected.Remove(cardRef);

        GameEventManager.ToggleButton(_currentlySelected.Count > 1);
    }
    
    private void GenerateGroup()
    {
        //get empty container
        _allCardParentList.Add(GameManager.Instance.GetCardContainer(ref Card_SpawnPoint));
        _allCardParentList.Add(GameManager.Instance.GetCardContainer(ref Card_SpawnPoint));


        //make the selected cards container null
        foreach (Card item in _currentlySelected)
        {
            _allCardParentList[item.transform.parent.GetSiblingIndex()].Ref_Card = null;
        }

        //RemoveEmpty();

        ShiftRest();

        //shift the selected cards to end
        int cardParentLength = _allCardParentList.Count;
        for (int i = 0; i < _currentlySelected.Count; i++)
        {
            _allCardParentList[cardParentLength - _currentlySelected.Count + i].MoveCard(_currentlySelected[i], true);
        }
        _currentlySelected.Clear();
    }

    private void ShiftRest()
    {
        int index = 0;

        foreach (CardContainer item in _allCardParentList)
        {
            if (!item.ToBeShifted)
            {
                if (item.Ref_Card == null)
                {
                    _allCardParentList[index].Ref_Card = null;
                }
                else
                {
                    _allCardParentList[index].MoveCard(item.Ref_Card);
                }

                index++;
            }
            else
            {
                item.ToBeShifted = false;
            }
        }
    }
}