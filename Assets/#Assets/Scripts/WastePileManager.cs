using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WastePileManager : MonoBehaviour
{
    public static WastePileManager Instance { get; private set; }
    [SerializeField]
    private Transform wasteParent;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // Optional: DontDestroyOnLoad(gameObject);
    }
    [SerializeField]
    private List<HY_Cards> wastePile = new List<HY_Cards>();
    public List<HY_Cards> WastePile
    {
        get { return wastePile; }
        set { wastePile = value; }
    }

    public HY_Cards GetTopCard()
    {
        if (wastePile.Count == 0) return null;
        return wastePile[wastePile.Count - 1];
    }
    public bool CanPlayCard(HY_Cards selectedCard)
    {
        HY_Cards topCard = GetTopCard();
       
        if (topCard == null) return true;

        int topVal = topCard.value;
        int selVal = selectedCard.value;
        print($"topVal: {topVal}, SelVal: {selVal}");
        // TriPeaks allows play if selected is one higher or lower, wrap around from King to Ace
        return Mathf.Abs(topVal - selVal) == 1 || (topVal == 13 && selVal == 1) || (topVal == 1 && selVal == 13);
    }

    public void AddToWastePile(HY_Cards card)
    {
        //card.transform.SetParent(transform);
        // RectTransform rt = card.GetComponent<RectTransform>();
        // rt.anchoredPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
        card.transform.position = transform.position;
        card.transform.SetAsLastSibling(); // Draw on top
        card.Flip(true);
        wastePile.Add(card);
    }
}
