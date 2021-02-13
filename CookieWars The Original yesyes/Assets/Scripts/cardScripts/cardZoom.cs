using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class cardZoom : NetworkBehaviour
{
    PlayerManager PlayerManager;
    private GameObject zoomedCard;
    private float downTime;
    private float timeGesamt;
    public GameObject prefabCard;

    public void OnClickDown()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        downTime = Time.time;  
        timeGesamt = Time.time - downTime;
        while(timeGesamt < 2)
        {
           OnClickUp(timeGesamt);
        }

    }
    
    public void OnClickUp(float _timeGesamt)
    {
        if(timeGesamt > 2f)
        {
            GameObject Canvas = GameObject.Find("Main Canvas");
            GameObject ZoomInstance = Instantiate(prefabCard, new Vector2(0, 90), Quaternion.identity);
            ZoomInstance.GetComponent<DisplayCard>().Karte = gameObject.GetComponent<DisplayCard>().Karte;
            ZoomInstance.transform.SetParent(Canvas.transform, false);
        }
    }

    public void OnHoverExit() 
    {
        Destroy(zoomedCard);
    }
   
}
