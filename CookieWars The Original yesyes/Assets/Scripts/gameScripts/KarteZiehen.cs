using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class KarteZiehen : NetworkBehaviour
{
    public PlayerManager PlayerManager;


    public void OnClick()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();

        if(!PlayerManager.isPlayerTurn) return;
        if(PlayerManager.Operationen<=0) return;   
        PlayerManager.updateOperationenDisplay();
        
        if(PlayerManager.cardsDealt == PlayerManager.Cards.Count)
        {
            Debug.Log("Dein Deck hat keine Karten mehr");
            return;
        }
        else
        {
            PlayerManager.CmdDealCards();
            PlayerManager.Operationen--;
        }
        
        

    }
}
