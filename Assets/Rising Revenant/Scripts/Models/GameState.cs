using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using System;
using UnityEngine;

public class GameState : ModelInstance
{

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("outpost_created_count")]
    public UInt32 outpostCreatedCount;

    [ModelField("outpost_remaining_count")]
    public UInt32 outpostRemainingCount;

    [ModelField("remain_life_count")]
    public UInt32 remainLifeCount;

    [ModelField("reinforcement_count")]
    public UInt32 reinforcementCount;

    [ModelField("contribution_score_total")]
    public RisingRevenantUtils.U256 contributionScoreTotal;


    void Start()
    {
        DojoEntitiesDataManager.gameEntCounter = this;
        DojoEntitiesDataManager.currentGameId = gameId;

        if (UiEntitiesReferenceManager.topBarUiElement != null)
        {
            UiEntitiesReferenceManager.topBarUiElement.ChangeInGameEntCounter();
            UiEntitiesReferenceManager.topBarUiElement.CalcContrib();
        }
    }

    public override void OnUpdate(Model model)
    {
        Debug.Log("\nGameState");
        Debug.Log("low: " + contributionScoreTotal.low.ToString() + "|| high: " + contributionScoreTotal.high.ToString());



        base.OnUpdate(model);

        if (UiEntitiesReferenceManager.gamePhaseManager != null)
        {
            UiEntitiesReferenceManager.gamePhaseManager.CheckForWin();
        }

        if (UiEntitiesReferenceManager.topBarUiElement != null)
        {
            UiEntitiesReferenceManager.topBarUiElement.ChangeInGameEntCounter();
            UiEntitiesReferenceManager.topBarUiElement.CalcContrib();
        }   
    }
}
