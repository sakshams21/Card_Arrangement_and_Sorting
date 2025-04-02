using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private CardManager Ref_CardManager;
    public GraphicRaycaster MainCanvas_GraphicRaycaster;
    public Canvas MainCanvas;

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public CardContainer GetNeighBoruCardContainer(int neighbourCardIndex)
    {
        return Ref_CardManager.GetNeighbor(neighbourCardIndex);
    }
}