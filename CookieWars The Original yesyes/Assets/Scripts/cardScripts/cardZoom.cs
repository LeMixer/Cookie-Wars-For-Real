﻿using System.Collections;
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
    }
    
    public void OnClickUp()
    {
        timeGesamt = Time.time - downTime;
        Debug.Log(timeGesamt);
        if(timeGesamt > 2f)
        {
            GameObject ZoomInstance = Instantiate(prefabCard, transform.position, Quaternion.identity);
            ZoomInstance.GetComponent<DisplayCard>().Karte = gameObject.GetComponent<DisplayCard>().Karte;

            PlayerManager.CmdZoomCard(ZoomInstance);
            Debug.Log(timeGesamt);
        }
    }

    public void OnHoverExit() 
    {
        Destroy(zoomedCard);
    }
}
