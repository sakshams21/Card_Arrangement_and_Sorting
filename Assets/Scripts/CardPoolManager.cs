using System;
using System.Collections.Generic;
using UnityEngine;

public class CardPoolManager : MonoBehaviour
{
    [SerializeField] private int MaxPoolSize = 10;
    [SerializeField] private CardContainer CardContainer_Prefab;
    [SerializeField] private Transform Spawn_Transform;
    private Queue<CardContainer> _containerPool = new Queue<CardContainer>();

    private void Start()
    {
        for (int i = 0; i < MaxPoolSize; i++)
        {
            _containerPool.Enqueue(Instantiate(CardContainer_Prefab, Spawn_Transform));
        }
    }

    public CardContainer GetCardContainer(ref Transform setParent)
    {
        CardContainer card = _containerPool.Count <= 0
            ? Instantiate(CardContainer_Prefab, Spawn_Transform)
            : _containerPool.Dequeue();

        card.transform.SetParent(setParent);
        card.gameObject.SetActive(true);
        return card;
    }

    public void BackToPool(CardContainer cardContainer)
    {
        cardContainer.transform.SetParent(Spawn_Transform);
        cardContainer.gameObject.SetActive(false);
        _containerPool.Enqueue(cardContainer);
    }
}