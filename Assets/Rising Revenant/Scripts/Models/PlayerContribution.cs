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
        var currentAddress = DojoEntitiesDataManager.currentAccount.Address.Hex();

        if (playerId.Hex() == currentAddress)
        {
            DojoEntitiesDataManager.playerContrib = this;
            UiEntitiesReferenceManager.topBarUiElement.CalcContrib();
        }
        else
        {
            Destroy(this);
        }
    }

    public override void OnUpdate(Model model)
    {
        Debug.Log("\nplayerContribution");
        Debug.Log("low: " + score.low.ToString() + "|| high: " + score.high.ToString());

        base.OnUpdate(model);
        OnValueChange?.Invoke();

        if (UiEntitiesReferenceManager.topBarUiElement != null)
        {
            UiEntitiesReferenceManager.topBarUiElement.ChangeInGameData();
            UiEntitiesReferenceManager.topBarUiElement.CalcContrib();
        }
    }

    
}
