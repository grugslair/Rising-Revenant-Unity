using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using System;
using UnityEngine;

public class WorldEventSetup : ModelInstance
{
    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("radius_start")]
    public UInt32 radiusStart;

    [ModelField("radius_increase")]
    public UInt32 radiusIncrease;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("checking world event setup");

        if (DojoEntitiesDataManager.currentGameId.Hex() != gameId.Hex())
        {
            Debug.Log($"world event setup was destroyed  {gameId.Hex()}");
            //Destroy(gameObject); return;
        }
        else
        {
            Debug.Log($"world event setup was spared {gameId.Hex()}");
        }
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
