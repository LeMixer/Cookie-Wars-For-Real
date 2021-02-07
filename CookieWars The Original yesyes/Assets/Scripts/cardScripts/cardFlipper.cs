using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cardFlipper : MonoBehaviour
{
    public Sprite cardFront;
    public Sprite cardBack;

    public void flip()
    {
        Sprite currentSprite = gameObject.GetComponent<Image>().sprite; 

        if (currentSprite == cardFront)
        {
            gameObject.GetComponent<Image>().sprite = cardBack;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = cardFront;
        }
    }
    
}
