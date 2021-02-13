using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cardZoom : MonoBehaviour
{
    PlayerManager PlayerManager;
    private GameObject zoomedCard;
    private float downTime;
    private float timeGesamt;
    public GameObject prefabCard;
    private bool spawnedZoom = false;

    public void OnClickDown()
    {
        spawnedZoom = false;
        downTime = Time.time;     
        while(timeGesamt < 2 && Input.GetMouseButtonDown(0))
        {
            spawnZoomMethod();
        }

    }
    
    public void spawnZoomMethod()
    {   
        timeGesamt = Time.time - downTime;
        if(timeGesamt > 2f && !spawnedZoom)
        {
            GameObject Canvas = GameObject.Find("Main Canvas");
            GameObject ZoomInstance = Instantiate(prefabCard, new Vector2(0, 90), Quaternion.identity);
            ZoomInstance.GetComponent<DisplayCard>().Karte = gameObject.GetComponent<DisplayCard>().Karte;
            ZoomInstance.transform.SetParent(Canvas.transform, false);
            spawnedZoom = true;
            downTime = 0f;
        }
        
    }

    public void OnHoverExit() 
    {
        Destroy(zoomedCard);
    }
   
}
