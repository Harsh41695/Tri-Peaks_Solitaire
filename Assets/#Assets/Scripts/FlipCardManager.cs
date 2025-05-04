using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlipCardManager : MonoBehaviour
{
    private static FlipCardManager _instace;
    public delegate void CardFlipHandler(HY_Cards index);
    private CardFlipHandler onCardCollected;

    private void Awake()
    {
        _instace = this;
        
    }

    private void OnDestroy()
    {
        _instace = null;
    }
    public static void InvokeCardFlip(HY_Cards card)
    {
        _instace.onCardCollected?.Invoke(card);
    }
    public static void AddListener(CardFlipHandler onCardCollectCallback)
    {
        _instace.onCardCollected += onCardCollectCallback;
    }
    public static void RemoveListener(CardFlipHandler onCardCollectCallback)
    {
        _instace.onCardCollected -= onCardCollectCallback;
    }
}
