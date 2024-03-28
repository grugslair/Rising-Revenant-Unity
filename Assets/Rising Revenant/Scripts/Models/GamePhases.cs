using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using System;
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
        Debug.Log("checking game phases");

        if (DojoEntitiesDataManager.currentGameId.Hex() != gameId.Hex())
        {
            Debug.Log($"GamePhases was destroyed {gameId.Hex()}");
            //Destroy(gameObject); return;
        }
        else
        {
            Debug.Log($"GamePhases was spared {gameId.Hex()}");
        }

        Debug.Log($"status: {status}");
        Debug.Log($"preparationBlockNumber: {preparationBlockNumber}");
        Debug.Log($"playBlockNumber: {playBlockNumber}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);
    }
}
