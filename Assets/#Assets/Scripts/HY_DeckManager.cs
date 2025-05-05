using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HY_DeckManager : MonoBehaviour, IPointerDownHandler
{
    private static HY_DeckManager Instacne;

    [SerializeField]
    TextMeshProUGUI win_LooseTxt;

    public static HY_DeckManager _instacne
    {
        get
        {
            if (Instacne == null)
            {
                Debug.LogError("HY_DeckManager is not initialized!");
            }
            return Instacne;
        }
    }

    int count;
    //    [SerializeField]
    //    private List<GameObject> deck = new List<GameObject>(); 
    private string[] suits = { "Club", "Diamond", "Heart", "Spades" };

    [SerializeField]
    private List<HY_Cards> cardObj = new List<HY_Cards>(); // Card objects for the scene
    [SerializeField]
    private List<Vector2> originalPositions = new List<Vector2>(); // To store original card positions

    [SerializeField]
    private List<HY_Cards> tableauCards = new List<HY_Cards>();

    /// <summary>
    /// Tableau Cards public field for removing element from any scripts
    /// </summary>
    public List<HY_Cards> TableauCards
    {
        get => tableauCards;
        set => tableauCards = value;
    }


    [SerializeField]
    private List<HY_Cards> drawCards = new List<HY_Cards>();
    [SerializeField]
    private List<HY_Cards> faceUpCards = new List<HY_Cards>();// Faced up Cards


    public List<HY_Cards> checkFacedCard = new List<HY_Cards>();


    private void Awake()
    {
        if (Instacne != null && Instacne != this)
        {
            Destroy(gameObject);
            return;
        }

        Instacne = this;
        DontDestroyOnLoad(gameObject);
        win_LooseTxt.gameObject.SetActive(false);
    }


    private void Start()
    {
        // Assign values and suits to the cards
        for (int s = 0; s < suits.Length; s++)
        {
            for (int v = 1; v <= 13; v++)
            {
                cardObj[s * 13 + (v - 1)].SetValue(v, suits[s]);
            }
        }

        // Save the original positions for restoring the layout
        SaveOriginalPositions();

        // Shuffle the cards
        Shuffle();
        //Tableau Card 
        TableauCard();
        SetDependecies();
    }

    /// <summary>
    /// Saves the original positions of the card objects in the pyramid layout.
    /// </summary>
    void SaveOriginalPositions()
    {
        originalPositions.Clear();
        foreach (HY_Cards card in cardObj)
        {
            // Debug.Log(card.value);
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

    private void Update()
    {
        // CheckWinCondition();
    }

    public bool IsDrawCard(HY_Cards card)
    {
        return drawCards.Contains(card);
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
        if (drawCards.Count <= 0)
        {
            return;
        }
        else
        {
            HY_Cards card = drawCards[drawCards.Count - 1];
            WastePileManager.Instance.AddToWastePile(card);
            drawCards.Remove(card);
        }
        CheckWinCondition();
    }


    bool CanCardFlip(HY_Cards firstCard, HY_Cards secondCard)
    {
        if (firstCard.isRemoved && secondCard.isRemoved)
        {
            return true;
        }
        return false;
    }

    void SetDependecies()
    {
        cardObj[0].dependency.Add(cardObj[1], GetCardIndex(cardObj[1]));
        cardObj[0].dependency.Add(cardObj[2], GetCardIndex(cardObj[2]));
        cardObj[1].dependency.Add(cardObj[3], GetCardIndex(cardObj[3]));
        cardObj[1].dependency.Add(cardObj[4], GetCardIndex(cardObj[4]));
        cardObj[2].dependency.Add(cardObj[4], GetCardIndex(cardObj[4]));
        cardObj[2].dependency.Add(cardObj[5], GetCardIndex(cardObj[5]));
        cardObj[3].dependency.Add(cardObj[18], GetCardIndex(cardObj[18]));
        cardObj[3].dependency.Add(cardObj[19], GetCardIndex(cardObj[19]));
        cardObj[4].dependency.Add(cardObj[19], GetCardIndex(cardObj[19]));
        cardObj[4].dependency.Add(cardObj[20], GetCardIndex(cardObj[20]));
        cardObj[5].dependency.Add(cardObj[20], GetCardIndex(cardObj[20]));
        cardObj[5].dependency.Add(cardObj[21], GetCardIndex(cardObj[21]));
        cardObj[6].dependency.Add(cardObj[7], GetCardIndex(cardObj[7]));
        cardObj[6].dependency.Add(cardObj[8], GetCardIndex(cardObj[8]));
        cardObj[7].dependency.Add(cardObj[9], GetCardIndex(cardObj[9]));
        cardObj[7].dependency.Add(cardObj[10], GetCardIndex(cardObj[10]));
        cardObj[8].dependency.Add(cardObj[10], GetCardIndex(cardObj[10]));
        cardObj[8].dependency.Add(cardObj[11], GetCardIndex(cardObj[11]));
        cardObj[9].dependency.Add(cardObj[21], GetCardIndex(cardObj[21]));
        cardObj[9].dependency.Add(cardObj[22], GetCardIndex(cardObj[22]));
        cardObj[10].dependency.Add(cardObj[22], GetCardIndex(cardObj[22]));
        cardObj[10].dependency.Add(cardObj[23], GetCardIndex(cardObj[23]));
        cardObj[11].dependency.Add(cardObj[23], GetCardIndex(cardObj[23]));
        cardObj[11].dependency.Add(cardObj[24], GetCardIndex(cardObj[24]));
        cardObj[12].dependency.Add(cardObj[13], GetCardIndex(cardObj[13]));
        cardObj[12].dependency.Add(cardObj[14], GetCardIndex(cardObj[14]));
        cardObj[13].dependency.Add(cardObj[15], GetCardIndex(cardObj[15]));
        cardObj[13].dependency.Add(cardObj[16], GetCardIndex(cardObj[16]));
        cardObj[14].dependency.Add(cardObj[16], GetCardIndex(cardObj[16]));
        cardObj[14].dependency.Add(cardObj[17], GetCardIndex(cardObj[17]));
        cardObj[15].dependency.Add(cardObj[24], GetCardIndex(cardObj[24]));
        cardObj[15].dependency.Add(cardObj[25], GetCardIndex(cardObj[25]));
        cardObj[16].dependency.Add(cardObj[25], GetCardIndex(cardObj[25]));
        cardObj[16].dependency.Add(cardObj[26], GetCardIndex(cardObj[26]));
        cardObj[17].dependency.Add(cardObj[26], GetCardIndex(cardObj[26]));
        cardObj[17].dependency.Add(cardObj[27], GetCardIndex(cardObj[27]));
    }

    public void RemoveCard(HY_Cards cardToRemove)
    {
        if (_instacne.TableauCards.Contains(cardToRemove))
        {
            _instacne.TableauCards.Remove(cardToRemove);
            Debug.Log($"{cardToRemove.name} has been removed from tableauCards.");
        }
        if (_instacne.faceUpCards.Contains(cardToRemove))
        {
            _instacne.faceUpCards.Remove(cardToRemove);
            Debug.Log($"{cardToRemove.name} has been removed from tableauCards.");
        }
        else
        {
            Debug.LogWarning($"{cardToRemove.name} is not in the tableauCards list.");
        }
        if (_instacne == null)
        {
            Debug.LogError("HY_DeckManager instance is null!");
            return;
        }

        if (cardToRemove == null)
        {
            Debug.LogError("Card to remove is null!");
            return;
        }

        if (_instacne.TableauCards == null || _instacne.TableauCards.Count == 0)
        {
            Debug.LogWarning("TableauCards list is empty!");
            return;
        }


    }
    public void AddCard(HY_Cards cardToAdd)
    {
        if (!_instacne.tableauCards.Contains(cardToAdd))
        {
            _instacne.tableauCards.Add(cardToAdd);
        }
    }
    public int GetCardIndex(HY_Cards obj)
    {
        return cardObj.FindIndex(x => x == obj);
    }


    public void CheckWinCondition()
    {
        if (tableauCards.Count == 0)
        {
            print("You WOn");
            win_LooseTxt.text = "'YOU WON'";
            win_LooseTxt.gameObject.SetActive(true);
            return;
        }

        if (drawCards.Count == 0)
        {
            foreach (HY_Cards cards in _instacne.tableauCards)
            {
                if (cards._isFacedUp)
                {
                    if (!checkFacedCard.Contains(cards))
                    {
                        checkFacedCard.Add(cards);
                    }
                }

            }
            foreach (HY_Cards cards in faceUpCards)
            {
                if (cards._isFacedUp)
                {
                    if (!checkFacedCard.Contains(cards) && !WastePileManager.Instance.WastePile.Contains(cards))
                    {
                        checkFacedCard.Add(cards);
                    }
                }
            }

            foreach (HY_Cards cards in checkFacedCard)
            {
                count = 0;
                if (WastePileManager.Instance.CanPlayCard(cards))
                {
                    print("YOU Still Can Play");
                    count++;
                    print(count);
                     return;
                }
                
            }

            count = 0;
            if (count == 0)
            {
                print("Cant make Move");
                win_LooseTxt.text = "'Loose'";
                win_LooseTxt.gameObject.SetActive(true);
            }





        }


    }



}
