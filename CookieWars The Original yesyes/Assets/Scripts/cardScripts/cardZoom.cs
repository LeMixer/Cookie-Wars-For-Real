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

    public void OnClickDown()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        while(Input.GetMouseButtonDown(0)) 
        {
            downTime = Time.time;    
                 
        }    
    }
    
    public void OnClickUp()
    {
        timeGesamt = Time.time - downTime;
        Debug.Log(timeGesamt);
        if(timeGesamt > 2f)
        {
            PlayerManager.CmdZoomCard(gameObject);
            Debug.Log(timeGesamt);
        }
    }

    public void OnHoverExit() 
    {
        Destroy(zoomedCard);
    }
}
