using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using System;
using UnityEngine;

public class PlayerContribution : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("player_id")]
    public FieldElement playerId;

    [ModelField("score")]
    public RisingRevenantUtils.U256 score;

    [ModelField("claimed")]
    public bool claimed;

    void Start()
    {
        Debug.Log("checking PlayerContribution");

        if (DojoEntitiesDataManager.currentGameId.Hex() != gameId.Hex())
        {
            Debug.Log($"PlayerContribution was destroyed {gameId.Hex()}");
            //Destroy(gameObject); return;
        }
        else
        {
            Debug.Log($"PlayerContribution was spared {gameId.Hex()}");
            if (playerId.Hex() == DojoEntitiesDataManager.currentAccount.Address.Hex())
            {
                DojoEntitiesDataManager.playerContrib = this;
                UiEntitiesReferenceManager.topBarUiElement.CalcContrib();
            }
            else
            {
                Destroy(this);
            }
        }




       
    }

    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);
        OnValueChange?.Invoke();

        if (UiEntitiesReferenceManager.topBarUiElement != null)
        {
            UiEntitiesReferenceManager.topBarUiElement.ChangeInGameData();
            UiEntitiesReferenceManager.topBarUiElement.CalcContrib();
        }
    }

    
}
