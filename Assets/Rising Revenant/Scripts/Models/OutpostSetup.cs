using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using System;

public class OutpostSetup : ModelInstance
{
    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("life")]
    public UInt32 life;

    [ModelField("max_reinforcements")]
    public UInt32 maxReinforcements;

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
