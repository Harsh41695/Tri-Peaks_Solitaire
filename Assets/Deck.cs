using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardParent; // UI Canvas or container
    [SerializeField] Sprite[] suitSprites; // 52 sprites, 1–13 per suit in order
    [SerializeField] Sprite backSprite;

   public List<Card> deck=new List<Card>();

    string[] suits = { "Club", "Diamond", "Heart", "Spades" };

    private void Start()
    {
       
        
        for (int s = 0; s < suits.Length; s++)
        {
            for (int v = 1; v <= 13; v++)
            {
                GameObject cardGO = Instantiate(cardPrefab, cardParent);
                Card card = cardGO.GetComponent<Card>();
                
                card.SetupCard(v, suits[s], suitSprites[s * 13 + (v - 1)], backSprite);
                deck.Add(card);
            }
        }
        Shuffle();

    }
    void Shuffle()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int rand = Random.Range(i, deck.Count);
            deck[i] = deck[rand];
            deck[rand] = temp;
        }
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].transform.SetSiblingIndex(i);
        }
    }
}
