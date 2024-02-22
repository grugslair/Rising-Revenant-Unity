using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public UInt32 gameId;
    [ModelField("start_block_number")]
    public UInt64 startBlockNumber;
    [ModelField("jackpot")]
    public FieldElement jackpot;
    [ModelField("preparation_phase_interval")]
    public UInt64 preparationPhaseInterval;
    [ModelField("event_interval")]
    public UInt64 eventInterval;
    [ModelField("coin_erc_address")]
    public FieldElement coinErcAddress;
    [ModelField("jackpot_pool_addr")]
    public FieldElement jackpotPoolAddress;
    [ModelField("revenant_init_price")]
    public FieldElement revenantInitPrice;
    [ModelField("jackpot_claim_status")]
    public UInt32 jackpotClaimStatus;
    [ModelField("status")]
    public UInt32 status;
    [ModelField("max_amount_of_revenants")]
    public UInt32 maxAmountOfRevenants;
    [ModelField("transaction_fee_percent")]
    public UInt32 transactionFeePercent;
    [ModelField("winner_prize_percent")]
    public UInt32 winnerPrizePercent;

    private void Start()
    {
        DojoEntitiesDataManager.gameDataInstance = this;
    }

    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);
        OnValueChange?.Invoke();
    }

}
