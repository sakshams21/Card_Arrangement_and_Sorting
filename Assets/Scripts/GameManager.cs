using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CardManager Ref_CardManager;
    [SerializeField] private CardPoolManager Ref_CardPoolManager;
    [SerializeField] private UserInterface Ref_UserInterface;
    public Canvas MainCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public CardContainer GetNeighBoruCardContainer(int neighbourCardIndex)
    {
        return Ref_CardManager.GetNeighbor(neighbourCardIndex);
    }

    public CardContainer GetCardContainer(ref Transform setParent)
    {
        return Ref_CardPoolManager.GetCardContainer(ref setParent);
    }

    public void BackToPool(CardContainer container)
    {
        Ref_CardPoolManager.BackToPool(container);
    }
}