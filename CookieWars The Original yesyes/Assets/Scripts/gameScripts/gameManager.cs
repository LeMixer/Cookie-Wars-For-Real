using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class gameManager : NetworkBehaviour
{

    public bool endTurn = false;
    PlayerManager PlayerManager;
    
    
    [SyncVar]
    public bool isPlayerTurn = false;
    public int PlayerHp = 20;
    public int EnemyHp = 20;
    public int playerOperationen;
    public int enemyOperationen;
    void Start()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        endTurn = PlayerManager.GetComponent<PlayerManager>().isPlayerTurn;
    }

       

       

    
        
    

}
