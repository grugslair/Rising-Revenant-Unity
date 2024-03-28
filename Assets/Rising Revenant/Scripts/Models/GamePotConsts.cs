using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using UnityEngine;

public class GamePotConsts : ModelInstance
{

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("pot_address")]
    public FieldElement potAddress;

    [ModelField("dev_percent")]
    public byte devPercent;

    [ModelField("confirmation_percent")]
    public byte confirmationPercent;

    [ModelField("ltr_percent")]
    public byte ltrPercent;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("checking GamePotConsts ");

        if (DojoEntitiesDataManager.currentGameId.Hex() != gameId.Hex())
        {
            Debug.Log($"GamePotConsts was destroyed {gameId.Hex()}");
            //Destroy(gameObject); return;
        }
        else
        {
            Debug.Log($"GamePotConsts was spared {gameId.Hex()}");
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
