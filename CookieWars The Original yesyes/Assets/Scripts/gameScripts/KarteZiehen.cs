using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class KarteZiehen : NetworkBehaviour
{
    public PlayerManager PlayerManager;


    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        
        if(!PlayerManager.isPlayerTurn) return;
        if(PlayerManager.Operationen<=0) return;
        PlayerManager.Operationen--;
        PlayerManager.updateOperationenDisplay();
        
        PlayerManager.CmdDealCards();
        

    }
}
