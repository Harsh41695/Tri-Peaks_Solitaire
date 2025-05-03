using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HY_DeckManager : MonoBehaviour,IPointerDownHandler
{
    //    [SerializeField]
    //    private List<GameObject> deck = new List<GameObject>(); 
    private string[] suits = { "Club", "Diamond", "Heart", "Spades" };

    [SerializeField]
    private List<GameObject> cardObj = new List<GameObject>(); // Card objects for the scene
    [SerializeField]
    private List<Vector2> originalPositions = new List<Vector2>(); // To store original card positions

    [SerializeField]
    private List<GameObject> tableauCards = new List<GameObject>();
    [SerializeField]
    private List<HY_Cards> drawCards = new List<HY_Cards>();
    [SerializeField]
    private List<GameObject> faceUpCards = new List<GameObject>();

    private void Start()
    {
        // Assign values and suits to the cards
        for (int s = 0; s < suits.Length; s++)
        {
            for (int v = 1; v <= 13; v++)
            {
                cardObj[s * 13 + (v - 1)].GetComponent<HY_Cards>().SetValue(v, suits[s]);
            }
        }

        // Save the original positions for restoring the layout
        SaveOriginalPositions();

        // Shuffle the cards
        Shuffle();
        //Tableau Card 
        TableauCard();
    }

    /// <summary>
    /// Saves the original positions of the card objects in the pyramid layout.
    /// </summary>
    void SaveOriginalPositions()
    {
        originalPositions.Clear();
        foreach (var card in cardObj)
        {
            RectTransform cardRT = card.GetComponent<RectTransform>();
            if (cardRT != null)
            {
                originalPositions.Add(cardRT.anchoredPosition);
            }
        }
    }

    /// <summary>
    /// Shuffles the cards and updates their positions and sibling order.
    /// </summary>
    /// 
    //Tableau Card placement Check
    void TableauCard()
    {
        for (int i = 0; i < cardObj.Count; i++)
        {
            if (i < 18)
            {
                cardObj[i].GetComponent<HY_Cards>().Flip(false);
                tableauCards.Add(cardObj[i]);

            }
            else if (i >= 18 && i < 28)
            {
                cardObj[i].GetComponent<HY_Cards>().Flip(true);
                faceUpCards.Add(cardObj[i]);


            }
            else if (i >= 28 && i < 52)
            {
                cardObj[i].GetComponent<HY_Cards>().Flip(false);
                drawCards.Add(cardObj[i].GetComponent<HY_Cards>());


            }
        }
    }
    void Shuffle()
    {
        // Step 1: Shuffle the card list
        for (int i = 0; i < cardObj.Count; i++)
        {
            int randomIndex = Random.Range(0, cardObj.Count);
            var temp = cardObj[i];
            cardObj[i] = cardObj[randomIndex];
            cardObj[randomIndex] = temp;
        }

        // Step 2: Update the sibling order in the hierarchy
        for (int i = 0; i < cardObj.Count; i++)
        {
            cardObj[i].transform.SetSiblingIndex(i);
        }

        // Step 3: Restore positions in the pyramid layout
        for (int i = 0; i < cardObj.Count; i++)
        {
            RectTransform cardRT = cardObj[i].GetComponent<RectTransform>();
            if (cardRT != null)
            {
                cardRT.anchoredPosition = originalPositions[i];
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // print("DeckManager Calling");
        if (drawCards.Count <=0)
        {
            return;
        }
        else
        {
            HY_Cards card = drawCards[drawCards.Count - 1];
            WastePileManager.Instance.AddToWastePile(card);
            drawCards.Remove(card);
        }



            
        
       

    }
}
