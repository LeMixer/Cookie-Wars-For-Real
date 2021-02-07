using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragDrop : NetworkBehaviour
{
    
    public PlayerManager PlayerManager;
    public gameManager GameManager;

    bool isDragging = false;
    private bool isDraggable = true;
    bool isOverDropZone = false;
    private GameObject DropZone;    
    private Vector2 startPosition;
    private GameObject startParent;
    public GameObject Canvas;
    public bool isPlayerTurn = true;

    private void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<gameManager>();
        Canvas = GameObject.Find("Main Canvas");
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        
        
        
        if(!hasAuthority)
        {
            isDraggable = false;
        }
    }
    
    public void StartDrag()
    {   

        if(!isDraggable) return;
        if(!isPlayerTurn) return;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;   
        isDragging = true;
    }   
    public void EndDrag()
    {

        if(!isDraggable) return;

        isDragging = false;
        if(isOverDropZone)
        { 
            NetworkIdentity networkIdentity =NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();
            
            PlayerManager.SearchDropZone(DropZone);
            
            isDraggable = false;
            PlayerManager.PlayCard(gameObject);     
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        
    isOverDropZone = true;
    DropZone = collision.gameObject;
        
        

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        DropZone = null;    
    }
    
    



    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x , Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }  

    }
    public void changeIsdragTrue()
    {       
        if(!isDraggable)
        {
           isDraggable = true; 
        }
    }
    
}   
