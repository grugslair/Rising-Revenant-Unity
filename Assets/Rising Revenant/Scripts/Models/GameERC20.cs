using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;

public class GameERC20 : ModelInstance
{


    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("address")]
    public FieldElement address;

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
    }
}
