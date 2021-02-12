using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour

{
    //Karten
    public GameObject Kenedim, PrincessRolle,DoppelKing,Ratava,WaherFox,ESlime,Elektriker;
    
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
    
    //operationen
    public int Operationen = 0;
    
    public GameObject OperationenAnzeige;
    
    
    //Card Variables
    public List<GameObject> Cards = new List<GameObject>();
    
    //Gamelogic Variables
    public bool isPlayerTurn = false;
    public int cardsDealt;
    
    //KartenDeck wird festgelegt und gemischt;
    
    /// fucking Bastard:       public override void OnStartServer()      ///
    
    
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

    public GameObject MainCanvas;
    public override void OnStartClient()
    {
        base.OnStartClient();

        OperationenAnzeige = GameObject.Find("OperationenAnzeige1");    
        MainCanvas = GameObject.Find("Main Canvas");
        PlayerHand = GameObject.Find("PlayerHand");
        EnemyHand = GameObject.Find("EnemyHand");
        DropZoneP.Add(GameObject.Find("PDropZone1"));
        DropZoneP.Add(GameObject.Find("PDropZone2"));
        DropZoneP.Add(GameObject.Find("PDropZone3"));
        DropZoneE.Add(GameObject.Find("EDropZone1"));
        DropZoneE.Add(GameObject.Find("EDropZone2"));
        DropZoneE.Add(GameObject.Find("EDropZone3"));
        Cards.Add(Kenedim);
        Cards.Add(PrincessRolle);
        Cards.Add(DoppelKing);
        Cards.Add(Ratava);
        Cards.Add(WaherFox);
        Cards.Add(ESlime);
        Cards.Add(Elektriker);    
          

        Shuffle(Cards);     
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        
        if(isClientOnly)
        {
            isPlayerTurn = true;
            Operationen =4;
            updateOperationenDisplay(); 
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
    
    //KartenZiehen button; 
    public int KartenZiehAnzahl = 1;     
    [Command]
    public void CmdDealCards()
    {   
        for (int i = 0; i < KartenZiehAnzahl; i++)
        {
            GameObject Card = Instantiate(Cards[0], new Vector2(0,0), Quaternion.identity);
            Cards.RemoveAt(0);
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
                PlayerManager P = NetworkClient.connection.identity.GetComponent<PlayerManager>();
                P.cardsDealt++;
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
                    PlayerManager Huso = NetworkClient.connection.identity.GetComponent<PlayerManager>();
                    card.transform.SetParent(DropZoneP[dropZoneSuchen].transform, false);                           
                    Huso.Operationen -= Huso.CardCost;                   
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
        if(type == "zoom")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(MainCanvas.transform,true);
                card.layer = LayerMask.NameToLayer("Zoom");
                RectTransform rect = card.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(185, 254);
                
            }
            else
            {
                card.transform.SetParent(MainCanvas.transform,true);
            }
        }           
    }
    void Update()
    {

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
    public static void Shuffle<T>(List<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (int i = 0; i < last; i++)
        {
           var r = UnityEngine.Random.Range(i, count);
           var tmp = ts[i];
           ts[i] = ts[r];
           ts[r] = tmp; 
        }
    }
    [Command]
    public void CmdZoomCard(GameObject ZoomCard)
    {
        GameObject zoomedCard = Instantiate(ZoomCard, new Vector2(0,100), Quaternion.identity);
        NetworkServer.Spawn(zoomedCard, connectionToClient);

        //RpcShowCard(zoomedCard, "zoom"); 
    }

    
    
        
    
  
}

