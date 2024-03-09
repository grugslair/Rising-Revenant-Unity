 using Dojo; using Dojo.Starknet; using Dojo.Torii;
using SimpleGraphQL;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

public class GamePot : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("total_pot")]
    public BigInteger totalPot;

    [ModelField("winners_pot")]
    public BigInteger winnersPot;

    [ModelField("confirmation_pot")]
    public BigInteger confirmationPot;

    [ModelField("ltr_pot")]
    public BigInteger ltrPot;

    [ModelField("dev_pot")]
    public BigInteger devPot;

    [ModelField("claimed")]
    public bool claimed;

    // Start is called before the first frame update
    void Start()
    {
        DojoEntitiesDataManager.gamePot = this;

        if (UiEntitiesReferenceManager.topBarUiElement != null)
        {
            UiEntitiesReferenceManager.topBarUiElement.ChangeInGameData();
        }
    }

    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);
        OnValueChange?.Invoke();

        UiEntitiesReferenceManager.topBarUiElement.ChangeInGameData();
    }

    
}
