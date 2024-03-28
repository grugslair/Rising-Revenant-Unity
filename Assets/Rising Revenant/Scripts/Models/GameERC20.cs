using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using UnityEngine;

public class GameERC20 : ModelInstance
{
    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("address")]
    public FieldElement address;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("checking GameERC20 ");

        if (DojoEntitiesDataManager.currentGameId.Hex() != gameId.Hex())
        {
            Debug.Log($"GameERC20 was destroyed {gameId.Hex()}");
            //Destroy(gameObject); return;
        }
        else
        {
            Debug.Log($"GameERC20 was spared {gameId.Hex()}");
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
