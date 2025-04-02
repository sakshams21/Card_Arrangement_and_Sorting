using System;
using System.Collections.Generic;
using UnityEngine;

public class CardPoolManager : MonoBehaviour
{
    [SerializeField] private int MaxPoolSize = 10;
    [SerializeField] private CardContainer CardParent_Prefab;
    [SerializeField] private Transform Spawn_Transform;
    private Queue<CardContainer> _parentPool = new Queue<CardContainer>();

    private void Start()
    {
        for (int i = 0; i < MaxPoolSize; i++)
        {
            _parentPool.Enqueue(Instantiate(CardParent_Prefab, Spawn_Transform));
        }
    }

    public CardContainer GetCardParent()
    {
        return _parentPool.Count <= 0 ? Instantiate(CardParent_Prefab, Spawn_Transform) : _parentPool.Dequeue();
    }

    public void BackToPool(CardContainer cardContainer)
    {
        cardContainer.transform.SetParent(Spawn_Transform);
        _parentPool.Enqueue(cardContainer);
    }
}