using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;

public class GameMap : ModelInstance
{
    [ModelField("game_id")]
    public FieldElement gameId;

    // Assuming Dimensions is a known type that needs conversion or handling
    [ModelField("dimensions")]
    public RisingRevenantUtils.Vec2 dimensions;

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
