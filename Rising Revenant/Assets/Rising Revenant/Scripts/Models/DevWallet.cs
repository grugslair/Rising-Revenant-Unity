 using Dojo; using Dojo.Starknet; using Dojo.Torii;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class DevWallet : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("owner")]
    public FieldElement ownerAddress;

    [ModelField("balance")]
    public BigInteger balance;

    [ModelField("init")]
    public bool init;

    // Start is called before the first frame update
    void Start()
    {
        if (DojoEntitiesDataManager.currentAccount != null)
        {
            var currentAddress = DojoEntitiesDataManager.currentAccount.Address.Hex();

            if (ownerAddress.Hex() == currentAddress)
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
