using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii; 

public class OutpostVerified : ModelInstance
{

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("event_id")]
    public FieldElement eventId;

    // Assuming OutpostId (Position) is a known type that needs conversion or handling
    [ModelField("outpost_id")]
    public RisingRevenantUtils.Vec2 outpostId;

    [ModelField("verified")]
    public bool verified;


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
