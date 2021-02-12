using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class cardZoom : NetworkBehaviour
{
    PlayerManager PlayerManager;
    private GameObject zoomedCard;
    private float time;

    public void OnClickDown()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        while(Input.GetMouseButtonDown(0) && time<2f) 
        {
            time += Time.deltaTime;
            Debug.Log(time);
        }
        if(time > 2f)
        {
            PlayerManager.CmdZoomCard(gameObject);
        }
        time = 0f;
        
    }

    public void OnHoverExit() 
    {
        Destroy(zoomedCard);
    }
}
