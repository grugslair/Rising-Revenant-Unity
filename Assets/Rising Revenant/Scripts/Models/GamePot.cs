using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using System;
using UnityEngine;

public class GamePot : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("total_pot")]
    public RisingRevenantUtils.U256 totalPot;

    [ModelField("winners_pot")]
    public RisingRevenantUtils.U256 winnersPot;

    [ModelField("confirmation_pot")]
    public RisingRevenantUtils.U256 confirmationPot;

    [ModelField("ltr_pot")]
    public RisingRevenantUtils.U256 ltrPot;

    [ModelField("dev_pot")]
    public RisingRevenantUtils.U256 devPot;

    [ModelField("claimed")]
    public bool claimed;


    void Start()
    {
        Debug.Log("checking GamePot");

        if (DojoEntitiesDataManager.currentGameId.Hex() != gameId.Hex())
        {
            Debug.Log($"GamePot was destroyed {gameId.Hex()}");
            //Destroy(gameObject); return;
        }
        else
        {
            Debug.Log($"GamePot was spared {gameId.Hex()}");


            DojoEntitiesDataManager.gamePot = this;

            if (UiEntitiesReferenceManager.topBarUiElement != null)
            {
                UiEntitiesReferenceManager.topBarUiElement.ChangeInGameData();
            }
        }


    }

    public override void OnUpdate(Model model)
    {

        base.OnUpdate(model);
        OnValueChange?.Invoke();

        UiEntitiesReferenceManager.topBarUiElement.ChangeInGameData();
    }
}
