using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public UInt32 gameId;
    [ModelField("owner")]
    public FieldElement ownerAddress;
    [ModelField("score")]
    public UInt32 score;
    [ModelField("player_wallet_amount")]
    public FieldElement playerWalletAmount;
    [ModelField("earned_prize")]
    public FieldElement earnedPrize;
    [ModelField("revenant_count")]
    public UInt32 revenantCount;
    [ModelField("outpost_count")]
    public UInt32 outpostCount;
    [ModelField("reinforcements_available_count")]
    public UInt32 reinforcementAvailableCount;
    [ModelField("score_claim_status")]
    public Boolean scoreClaimStatus;


    private void Start()
    {
        var currentAddress = DojoEntitiesDataManager.currentAccount.Address.Hex();

        if (ownerAddress.Hex() == currentAddress)
        {
            DojoEntitiesDataManager.playerSpecificData = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);
        OnValueChange?.Invoke();
    }
}
