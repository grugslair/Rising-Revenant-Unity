using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using UnityEngine;

public class GameTradeTax : ModelInstance
{

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("trade_tax_percent")]
    public byte tradeTaxPercent;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"checking GameTradeTax ");

        if (DojoEntitiesDataManager.currentGameId.Hex() != gameId.Hex())
        {
            Debug.Log($"GameTradeTax was destroyed {gameId.Hex()}");
            //Destroy(gameObject); return;
        }
        else
        {
            Debug.Log($"GameTradeTax was spared {gameId.Hex()}");
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
