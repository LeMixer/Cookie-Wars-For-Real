using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardZoom : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject zoomCardTemplate;
    private GameObject zoomedCard;


    public void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
    }

    public void OnHoverEnter()
    {
        zoomedCard = Instantiate(gameObject, new Vector2(0,170), Quaternion.identity);
        zoomedCard.transform.SetParent(Canvas.transform,true);
        zoomedCard.layer = LayerMask.NameToLayer("Zoom");
        RectTransform rect = zoomedCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(160, 224);
        
    }

    public void OnHoverExit() 
    {
        Destroy(zoomedCard);
    }
}
