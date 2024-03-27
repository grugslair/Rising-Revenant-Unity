using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using System;
using UnityEngine;

public class GamePot : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("total_pot")]
    public RisingRevenantUtils.U256 totalPot;

    [ModelField("winners_pot")]
    public RisingRevenantUtils.U256 winnersPot;

    [ModelField("confirmation_pot")]
    public RisingRevenantUtils.U256 confirmationPot;

    [ModelField("ltr_pot")]
    public RisingRevenantUtils.U256 ltrPot;

    [ModelField("dev_pot")]
    public RisingRevenantUtils.U256 devPot;

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
        Debug.Log("\nGamePot");
        Debug.Log("totalPot: " + totalPot.low.ToString() + "|| high: " + totalPot.high.ToString());
        Debug.Log("winnersPot: " + winnersPot.low.ToString() + "|| high: " + winnersPot.high.ToString());
        Debug.Log("confirmationPot: " + confirmationPot.low.ToString() + "|| high: " + confirmationPot.high.ToString());
        Debug.Log("ltrPot: " + ltrPot.low.ToString() + "|| high: " + ltrPot.high.ToString());


        base.OnUpdate(model);
        OnValueChange?.Invoke();

        UiEntitiesReferenceManager.topBarUiElement.ChangeInGameData();
    }

    
}
