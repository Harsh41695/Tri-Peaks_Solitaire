using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Shufulle : MonoBehaviour
{
    // Start is called before the first frame update

    List<Sprite> sprites = new List<Sprite>();

    List<Image> cardsFrontImg = new List<Image>();
    string[] suits = { "Club", "Diamond", "Heart", "Spades" };


    void Start()
    {// for(int s=)
        for (int s = 0; s < suits.Length; s++)
        {
            for (int i = 1; i <= 52; i++)
            {
                int index = Random.Range(0, sprites.Count);
                cardsFrontImg[i].sprite = sprites[index];
                //cardsFrontImg[i].GetComponentInParent<HY_Cards>().SetValue(index,suits)


        }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
