using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cardFlipper : MonoBehaviour
{
    public Image cardBack;

    public void flip()
    {
        var tempColor = cardBack.GetComponent<Image>().color;

        if(tempColor.a == 1f) 
        {
            tempColor.a = 0f;
            
        }
        else
        {
            tempColor.a = 1f;
        }
        cardBack.color = tempColor;
    }
        
    
}
