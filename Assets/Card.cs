using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    
    public Image frontImage;
    public Image backImage;
    public int value;
    public string suit;
   
    public void SetupCard(int value, string suit, Sprite frontSprite, Sprite backSprite)
    {
        this.value = value;
        this.suit = suit;
        frontImage.sprite = frontSprite;
        backImage.sprite = backSprite;
        Flip(true); // start face down
    }

    public void Flip(bool isFaceUp)
    {
        frontImage.gameObject.SetActive(isFaceUp);
        backImage.gameObject.SetActive(!isFaceUp);
    }

    private void OnMouseDown()
    {
        print("I can Feel Touch");
    }
}
