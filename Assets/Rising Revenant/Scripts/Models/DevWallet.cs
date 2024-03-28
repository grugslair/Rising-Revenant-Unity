using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using System;

public class DevWallet : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("owner")]
    public FieldElement ownerAddress;

    [ModelField("balance")]
    public RisingRevenantUtils.U256 balance;

    [ModelField("init")]
    public bool init;

    //public string balanceString;

    // Start is called before the first frame update
    void Start()
    {
        if (DojoEntitiesDataManager.currentAccount != null)
        {
            if (ownerAddress.Hex() == DojoEntitiesDataManager.currentAccount.Address.Hex())
            {
                DojoEntitiesDataManager.currentDevWallet = this;

                UiEntitiesReferenceManager.topBarUiElement.ChangeInPlayerSpecificData();
            }
            else
            {
                Destroy(this);
            }
        }
    }

    public override void OnUpdate(Model model)
    {

        base.OnUpdate(model);
        OnValueChange?.Invoke();

        UiEntitiesReferenceManager.topBarUiElement.ChangeInPlayerSpecificData();
    }

}
