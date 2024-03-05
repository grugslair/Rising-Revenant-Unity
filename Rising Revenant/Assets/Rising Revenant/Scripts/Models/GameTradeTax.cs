using Dojo;
using Dojo.Starknet;
using Dojo.Torii; 
using System.Collections; 
using System.Collections.Generic;
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
