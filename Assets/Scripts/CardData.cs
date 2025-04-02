using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
   public Sprite[] cards;
   private Dictionary<string, Sprite> _cardsData = new Dictionary<string, Sprite>();

   public Sprite GetCard(string cardName)
   {
       return _cardsData[cardName];
   }
   
   [ContextMenu("Load Cards")]
   public void StoreData()
   {
       foreach (Sprite card in cards)
       {
           _cardsData.TryAdd(card.name, card);
       }
   }
}