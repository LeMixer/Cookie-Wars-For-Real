using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour

{
    public GameObject DoppelKing;
    public GameObject Kenedim;
    public GameObject PrincessRolle;
    public GameObject ESlime;
    public GameObject Elektriker;
    public GameObject WaherFox;
    public GameObject Ratava;

    //DragDrop Variables
    public GameObject PlayerHand;
    public GameObject EnemyHand;
    public List<GameObject> DropZoneP = new List<GameObject>();
    public List<GameObject> DropZoneE = new List<GameObject>();
    public int dropZoneSuchen;
    public bool[] Besetzt ={false,false,false};
    public string dropZoneName;
    public bool Return = false;
    public GameObject endTurnButton;
    public int Operationen = 0;
    public GameObject OperationenAnzeige;
    
    //Card Variables
    private List<GameObject> Cards = new List<GameObject>();
    
    //Gamelogic Variables
    public bool isPlayerTurn = false;

    //KartenDeck wird festgelegt und gemischt;
    [Server]
    public override void OnStartServer()
    {
        Cards.Add(Kenedim);
        Cards.Add(PrincessRolle);
        Cards.Add(DoppelKing);
        Cards.Add(Ratava);
        Cards.Add(WaherFox);
        Cards.Add(ESlime);
        Cards.Add(Elektriker);  
    }
    
    
    
    public void ChangeTurn()
    {
        if(!isPlayerTurn) return;
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
        if(Player.isPlayerTurn)
        {
            Player.Operationen += 4;
            updateOperationenDisplay();
        }
        
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        OperationenAnzeige = GameObject.Find("OperationenAnzeige1");
        
        PlayerHand = GameObject.Find("PlayerHand");
        EnemyHand = GameObject.Find("EnemyHand");
        DropZoneP.Add(GameObject.Find("PDropZone1"));
        DropZoneP.Add(GameObject.Find("PDropZone2"));
        DropZoneP.Add(GameObject.Find("PDropZone3"));
        DropZoneE.Add(GameObject.Find("EDropZone1"));
        DropZoneE.Add(GameObject.Find("EDropZone2"));
        DropZoneE.Add(GameObject.Find("EDropZone3"));
    
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        
        Shuffle(Cards);
        
        if(isClientOnly)
        {
            isPlayerTurn = true;
            Operationen =4;
            updateOperationenDisplay();
            
        }
        
  
    }
    
    
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
        if(whichDropzone.name == "PDropZone1" && !Besetzt[0])
        {                        
            dropZoneSuchen = 0;
            Besetzt[0] = true;
           
        }
        else if(whichDropzone.name == "PDropZone2" && !Besetzt[1])
        {
            dropZoneSuchen = 1;
            Besetzt[1] = true;
            
        }
        else if(whichDropzone.name == "PDropZone3" && !Besetzt[2])
        {
            dropZoneSuchen = 2;
            Besetzt[2] = true;
            
        }
        else
        {
            Return = true;
        }
    }
    public static void Shuffle<T>(List<T> TS)
    {
        var count = TS.Count;
        var last = count-1;
        for (var i = 0; i < last; i++)
        {
            var R = UnityEngine.Random.Range(i,count);
            var tmp = TS[i];
            TS[i] = TS[R];
            TS[R] = tmp;
        }
    }
    
    //KartenZiehen button; 
    public int KartenZiehAnzahl = 1;     
    [Command]
    public void CmdDealCards()
    {
        
            for (int i = 0; i < KartenZiehAnzahl; i++)
            {
                GameObject Card = Instantiate(Cards[0], new Vector2(0,0), Quaternion.identity);
                NetworkServer.Spawn(Card, connectionToClient);
                RpcShowCard(Card, "Dealt");               
            }


    }
    public int CardCost = 1;   
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
                   PlayerManager huso = NetworkClient.connection.identity.GetComponent<PlayerManager>();
                   huso.Operationen -= huso.CardCost;
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
    //befehlskette Operationen Display
    public void updateOperationenDisplay()
    {
        CmdUpdateOperationenDisplay();
    }
    [Command]
    private void CmdUpdateOperationenDisplay()
    {
        RpcupdateOperationenDisplay();
    } 
    [ClientRpc]
    public void RpcupdateOperationenDisplay()
    {
        PlayerManager ODisplay= NetworkClient.connection.identity.GetComponent<PlayerManager>();
        Text OperationenText = OperationenAnzeige.GetComponent<Text>();
        OperationenText.text = ODisplay.Operationen.ToString();
    }
  
}
