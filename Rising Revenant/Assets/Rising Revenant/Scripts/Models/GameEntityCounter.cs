using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntityCounter : ModelInstance
{

    public event Action OnValueChange;

    [ModelField("game_id")]
    public UInt32 gameId;
    [ModelField("revenant_count")]
    public UInt32 revenantCount;
    [ModelField("outpost_count")]
    public UInt32 outpostCount;
    [ModelField("event_count")]
    public UInt32 eventCount;
    [ModelField("outpost_exists_count")]
    public UInt32 outpostExistsCount;
    [ModelField("remain_life_count")]
    public UInt32 remainLifeCount;
    [ModelField("reinforcement_count")]
    public UInt32 reinforcementCount;
    [ModelField("trade_count")]
    public UInt32 tradeCount;
    [ModelField("contribution_score_count")]
    public UInt32 scoreCount;


    private void Start()
    {
        DojoEntitiesDataManager.gameEntityCounterInstance = this;
    }

    //set up the once call with the quick if stat
    // the invoke the event from here sending the whole model and do that same for the game adn and stuff like that
    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);
        OnValueChange?.Invoke();
    }

}
