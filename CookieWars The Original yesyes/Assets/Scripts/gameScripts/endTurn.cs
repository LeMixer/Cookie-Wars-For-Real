using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class endTurn : NetworkBehaviour
{
    PlayerManager PlayerManager;
    public GameObject Canvas;
    public GameObject EndTurnButton;
    // Start is called before the first frame update
    
    public override void OnStartClient()
    {
    }
    public void OnClick()
    {
        
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        
        
        PlayerManager.ChangeTurn();
        if(!hasAuthority)
        {
            PlayerManager.setPlayerTurn();
        }

        
    }
    public void Spawnbutton()
    {
        Canvas = GameObject.Find("Main Canvas");
        EndTurnButton = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity );
        EndTurnButton.transform.SetParent(Canvas.transform, false);
    }
}

