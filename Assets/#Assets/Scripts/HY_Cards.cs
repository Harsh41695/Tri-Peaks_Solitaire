using System;
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
    [SerializeField]bool _isFacedUp;
    private void Awake()
    {
        //front = transform.Find("Front").GetComponent<Image>();
        //back = transform.Find("Back").GetComponent<Image>();
        front = transform.GetChild(0).GetComponent<Image>();
        back = transform.GetChild(1).GetComponent<Image>();
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
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ClickCheck();
    }
}
