using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii; 

public class CurrentGame : ModelInstance
{
    [ModelField("owner")]
    public FieldElement ownerAddress;
    [ModelField("game_id")]
    public FieldElement gameId;

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
