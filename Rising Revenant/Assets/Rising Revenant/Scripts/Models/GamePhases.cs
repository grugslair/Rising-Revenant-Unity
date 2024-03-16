using Dojo; using Dojo.Starknet; using Dojo.Torii;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePhases : ModelInstance
{

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("status")]
    public byte status;

    [ModelField("preparation_block_number")]
    public UInt64 preparationBlockNumber;

    [ModelField("play_block_number")]
    public UInt64 playBlockNumber;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);

        Debug.Log(status);
    }
}
