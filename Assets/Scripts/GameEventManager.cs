using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static event Action OnButtonPressed;
    public static event Action<bool> OnToggleButton;

    public static event Action<Card, bool, int> OnCardClicked;
    public static event Action<int> OnDragBegin;
    public static event Action<int> OnDragEnd;

    public static void ButtonPressed()
    {
        OnButtonPressed?.Invoke();
    }

    public static void ToggleButton(bool isVisible)
    {
        OnToggleButton?.Invoke(isVisible);
    }

    public static void InvokeCardClicked(Card card, bool selected, int index)
    {
        OnCardClicked?.Invoke(card, selected, index);
    }

    public static void InvokeDragBegin(int index)
    {
        OnDragBegin?.Invoke(index);
    }

    public static void InvokeDragEnd(int index)
    {
        OnDragEnd?.Invoke(index);
    }
}