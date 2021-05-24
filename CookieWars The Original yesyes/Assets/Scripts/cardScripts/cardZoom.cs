using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class cardZoom : MonoBehaviour
{
    PlayerManager PlayerManager;
    public GameObject prefabCard;
    GameObject ZoomInstance;

    public void OnClickDown()
    {
        if(ZoomInstance == null)
        {
            spawnZoomMethod();
        }  
    }
    
    public void spawnZoomMethod()
    {   
        GameObject Canvas = GameObject.Find("Main Canvas");
        ZoomInstance = Instantiate(prefabCard, new Vector2(0, 90), Quaternion.identity);
        ZoomInstance.GetComponent<DisplayCard>().Karte = gameObject.GetComponent<DisplayCard>().Karte;
        ZoomInstance.transform.SetParent(Canvas.transform, false);

    }

    public void OnClickUp() 
    {
        if(ZoomInstance != null)
        {
            Destroy(ZoomInstance);
        } 
    }

}
