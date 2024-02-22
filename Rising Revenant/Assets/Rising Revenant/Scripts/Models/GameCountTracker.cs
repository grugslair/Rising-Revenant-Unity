using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCountTracker : ModelInstance
{

    public event Action<Model> OnValueChange;

    [ModelField("entity_id")]
    public FieldElement configID;
    [ModelField("game_count")]
    public UInt32 games_count;

    private void Start()
    {
        DojoEntitiesDataManager.gameCounterInstance = this;
    }
}
