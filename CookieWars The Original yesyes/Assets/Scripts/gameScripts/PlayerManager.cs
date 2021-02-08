using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour

{
    //DragDrop Variables
    
    public GameObject card;
    public GameObject PlayerHand;
    public GameObject EnemyHand;
    public List<GameObject> DropZoneP = new List<GameObject>();
    public List<GameObject> DropZoneE = new List<GameObject>();
    public int dropZoneSuchen;
    public bool[] Besetzt ={false,false,false};
    public string dropZoneName;
    public bool Return = false;
    public GameObject endTurnButton;
    
    //Card Variables
    private List<GameObject> Cards = new List<GameObject>();
    public GameObject ZugText;
    private Text text;
    
    //Gamelogic Variables
    public bool isPlayerTurn = false;
    
    
    public void ChangeTurn()
    {
        CmdchangeTurn();
    }
    [Command]
    public void CmdchangeTurn()
    {
        
        RpcChangeTurn();

    }
    [ClientRpc]
    public void RpcChangeTurn()
    {
        PlayerManager Player = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        
        Player.isPlayerTurn = !Player.isPlayerTurn;
        
        if(isPlayerTurn)
        {
            text.text = "Du bist am Zug.";
        }
        else
        {
            text.text = "Du bist nicht am Zug";
        }
        

    }
    public void setPlayerTurn()
    {
        isPlayerTurn = true;
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        PlayerHand = GameObject.Find("PlayerHand");
        EnemyHand = GameObject.Find("EnemyHand");
        DropZoneP.Add(GameObject.Find("PDropZone1"));
        DropZoneP.Add(GameObject.Find("PDropZone2"));
        DropZoneP.Add(GameObject.Find("PDropZone3"));
        DropZoneE.Add(GameObject.Find("EDropZone1"));
        DropZoneE.Add(GameObject.Find("EDropZone2"));
        DropZoneE.Add(GameObject.Find("EDropZone3"));
        ZugText = GameObject.Find("ZugText");


        
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if(isClientOnly)
        {
            isPlayerTurn = true;
        }
        
  
    }


    //DragDrop Kette Search DropZone
    
    
    //DragDrop Kette Search DropZone
    public void SearchDropZone(GameObject whichDropzone)
    {        
        CmdSearchCard(whichDropzone);
    }
    
    [Command]
    public void CmdSearchCard(GameObject whichDropzone)
    {
        RpcSearchDropzone(whichDropzone);
    }

        [ClientRpc]
    public void RpcSearchDropzone(GameObject whichDropzone)
    {
        dropZoneName = whichDropzone.name;
        if(whichDropzone.name == "PDropZone1" && Besetzt[0] == false)
        {                        
            dropZoneSuchen = 0;
            Besetzt[0] = true;
           
        }
        else if(whichDropzone.name == "PDropZone2" && Besetzt[1] == false)
        {
            dropZoneSuchen = 1;
            Besetzt[1] = true;
            
        }
        else if(whichDropzone.name == "PDropZone3" && Besetzt[2] == false)
        {
            dropZoneSuchen = 2;
            Besetzt[2] = true;
            
        }
        else
        {
            Return = true;
        }
    }

    //KartenDeck wird festgelegt und gemischt;
    [Server]
    public override void OnStartServer()
    {
        Cards.Add(card);
        
    }
    
    //KartenZiehen button;      
    [Command]
    public void CmdDealCards()
    {

        for (int i = 0; i < 3; i++)
        {
        GameObject Card = Instantiate(Cards[0], new Vector2(0,0), Quaternion.identity);
        NetworkServer.Spawn(Card, connectionToClient);
        RpcShowCard(Card, "Dealt");
        }

    }
    
    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
             
    }
    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "played");
    }
    //KartenLogik u. Mirror;
    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if(type == "Dealt")
        {
            if(hasAuthority)
            {
                card.transform.SetParent(PlayerHand.transform, false);
            }
            else
            {
                card.transform.SetParent(EnemyHand.transform, false);
                card.GetComponent<cardFlipper>().flip();
            }
        }
        else if(type == "played")
        {             
            if (hasAuthority) 
            {
               if (!Return) 
               {
                   card.transform.SetParent(DropZoneP[dropZoneSuchen].transform, false);                  
               }
               else 
               {
                   card.transform.SetParent(PlayerHand.transform, true);
                   card.GetComponent<DragDrop>().changeIsdragTrue(); 
                   Return = false;                                    
               }
            } 
            else
            {
                if (!Return)
                {
                        card.GetComponent<cardFlipper>().flip();
                        card.transform.SetParent(DropZoneE[dropZoneSuchen].transform, false);                   
                }
                

                else
                {
                    card.transform.SetParent(EnemyHand.transform, true);
                    Return = false;
                }
            }

            
        }
               
    }
    void Update()
    {
        
    }
  
}
