//using System.Collections.Generic;
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager instance;

//    public Deck deck;
//    public RectTransform wastePile;
//    public GameObject cardPrefab;
//    public RectTransform canvasRoot;

//    private List<Card> tableauCards = new();
//    private Card topWasteCard;

//    private void Awake()
//    {
//        instance = this;
//    }

//    private void Start()
//    {
//        InitGame();
//    }

//    public void InitGame()
//    {
//        deck.Shuffle();
//        CreateTableau();
//        DrawToWaste();
//    }

//    void CreateTableau()
//    {
//        int[] pyramidStructure = { 3, 6, 9 };
//        Vector2 startPos = new Vector2(-300f, 250f);
//        float xOffset = 100f, yOffset = -120f;

//        int cardIndex = 0;

//        for (int row = 0; row < pyramidStructure.Length; row++)
//        {
//            for (int col = 0; col < pyramidStructure[row]; col++)
//            {
//                GameObject cardGO = Instantiate(cardPrefab, canvasRoot);
//                Card card = cardGO.GetComponent<Card>();
//                card.Setup(deck.DrawCard());
//                RectTransform rt = cardGO.GetComponent<RectTransform>();
//                rt.anchoredPosition = startPos + new Vector2(col * xOffset - row * 50f, row * yOffset);
//                card.Flip(false);
//                tableauCards.Add(card);
//                cardIndex++;
//            }
//        }

//        // Add 10 face-up base cards
//        for (int i = 0; i < 10; i++)
//        {
//            GameObject cardGO = Instantiate(cardPrefab, canvasRoot);
//            Card card = cardGO.GetComponent<Card>();
//            card.Setup(deck.DrawCard());
//            RectTransform rt = cardGO.GetComponent<RectTransform>();
//            rt.anchoredPosition = new Vector2(-450f + i * 100f, -350f);
//            card.Flip(true);
//            tableauCards.Add(card);
//        }

//        foreach (Card card in tableauCards)
//        {
//            card.isLocked = false;
//            card.UpdateVisual();
//        }
//    }

//    public void DrawToWaste()
//    {
//        int next = deck.DrawCard();
//        if (next == -1)
//        {
//            Debug.Log("Deck Empty!");
//            return;
//        }

//        if (topWasteCard != null)
//            Destroy(topWasteCard.gameObject);

//        GameObject cardGO = Instantiate(cardPrefab, wastePile);
//        Card card = cardGO.GetComponent<Card>();
//        card.Setup(next);
//        card.Flip(true);
//        card.button.interactable = false;
//        topWasteCard = card;

//        // Reset position in case RectTransform is offset
//        cardGO.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
//    }

//    public bool IsPlayable(int val)
//    {
//        if (topWasteCard == null) return false;

//        int topVal = topWasteCard.value;
//        return (val == topVal + 1 || val == topVal - 1) ||
//               (topVal == 13 && val == 1) || (topVal == 1 && val == 13);
//    }

//    public void PlayCard(Card card)
//    {
//        card.Flip(false);
//        card.button.interactable = false;
//        tableauCards.Remove(card);

//        if (topWasteCard != null)
//            Destroy(topWasteCard.gameObject);

//        GameObject cardGO = Instantiate(cardPrefab, wastePile);
//        Card newTop = cardGO.GetComponent<Card>();
//        newTop.Setup(card.value);
//        newTop.Flip(true);
//        newTop.button.interactable = false;
//        newTop.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
//        topWasteCard = newTop;

//        CheckWinLose();
//    }

//    public void CheckWinLose()
//    {
//        if (tableauCards.FindAll(c => c.isFaceUp).Count == 0)
//        {
//            Debug.Log("You Win!");
//        }
//        else if (NoMoreMoves())
//        {
//            Debug.Log("No Moves. Buy New Deck?");
//        }
//    }

//    public bool NoMoreMoves()
//    {
//        foreach (Card c in tableauCards)
//        {
//            if (c.isFaceUp && IsPlayable(c.value)) return false;
//        }
//        return true;
//    }
//}
