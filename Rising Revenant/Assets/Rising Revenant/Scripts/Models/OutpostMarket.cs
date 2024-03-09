using Dojo; using Dojo.Starknet; using Dojo.Torii;
using SimpleGraphQL;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

public class OutpostMarket : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("price")]
    public BigInteger pricePerOutpost;

    [ModelField("available")]
    public UInt32 maxAmountOfOutposts;


    void Start()
    {
        DojoEntitiesDataManager.outpostMarketData = this;

        if (UiEntitiesReferenceManager.topBarUiElement != null)
        {
            UiEntitiesReferenceManager.topBarUiElement.ChangeInGameEntCounter();
        }
    }

    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);

        if (UiEntitiesReferenceManager.topBarUiElement != null)
        {
            UiEntitiesReferenceManager.topBarUiElement.ChangeInGameEntCounter();
        }
    }

  
}
