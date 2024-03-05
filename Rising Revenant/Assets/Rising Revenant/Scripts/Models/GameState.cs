using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using System;
using System.Numerics;

public class GameState : ModelInstance
{
    public event Action OnValueChange;

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
    public BigInteger contributionScoreTotal;

    void Start()
    {
        DojoEntitiesDataManager.gameEntCounter = this;
        DojoEntitiesDataManager.currentGameId = gameId;

        if (UiEntitiesReferenceManager.topBarUiElement != null)
        {
            UiEntitiesReferenceManager.topBarUiElement.ChangeInGameEntCounter();
        }
    }

    void Update()
    {
        
    }

    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);

        if (UiEntitiesReferenceManager.gamePhaseManager != null)
        {
            UiEntitiesReferenceManager.gamePhaseManager.CheckForWin();
        }

        if (UiEntitiesReferenceManager.topBarUiElement != null)
        {
            UiEntitiesReferenceManager.topBarUiElement.ChangeInGameEntCounter();
        }

        OnValueChange?.Invoke();
    }
}
