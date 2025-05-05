using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HY_Cards : MonoBehaviour,IPointerDownHandler
{
    [SerializeField]
    public int value { get; private set; }
    [SerializeField]
    private string suit;

    [SerializeField]
    Image front, back;
    public bool _isFacedUp; //{ get; private set; }

    public Dictionary<HY_Cards,int> dependency = new Dictionary<HY_Cards,int>();

   // List<int> dependencyIndexs = new List<int>();
    public bool isRemoved { get; private set; } 
    private void Awake()
    {
        //front = transform.Find("Front").GetComponent<Image>();
        //back = transform.Find("Back").GetComponent<Image>();
        front = transform.GetChild(0).GetComponent<Image>();
        back = transform.GetChild(1).GetComponent<Image>();
    }

    private void Start()
    {
        FlipCardManager.AddListener(OnCardCollectedCalled);
       
    }

    private void OnDestroy()
    {
        FlipCardManager.RemoveListener(OnCardCollectedCalled);

    }
    public void SetValue(int val, string suit)
    {
        value = val;
        this.suit = suit;
    }

    public void Flip(bool isFaceUp)
    {
        this._isFacedUp = isFaceUp;
        front.gameObject.SetActive(isFaceUp);
        back.gameObject.SetActive(!isFaceUp);
    }

    public void ClickCheck()
    {
        
        if (WastePileManager.Instance.CanPlayCard(this) && _isFacedUp)
        {
            WastePileManager.Instance.WastePile.Add(this);
            gameObject.transform.position = WastePileManager.Instance.transform.position;
            transform.SetAsLastSibling();
            HY_DeckManager._instacne.RemoveCard(this);
            FlipCardManager.InvokeCardFlip(this);

            // print(HY_DeckManager._instacne.GetCardIndex(this));
            if (WastePileManager.Instance.WastePile.Contains(this))
            {
                HY_DeckManager._instacne.checkFacedCard.Remove(this); 
            }

        }
            HY_DeckManager._instacne.CheckWinCondition();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ClickCheck();
        
    }
    
   private void OnCardCollectedCalled(HY_Cards index)
    {
        if (HY_DeckManager._instacne.IsDrawCard(this))
        {
            return;
        }
        if (dependency.ContainsKey(index))
        {
            dependency.Remove(index);
        }

        if (dependency.Count <= 0)
        {
            Flip(true);
            //HY_DeckManager._instacne.AddCard(this);
        }
    }
}
