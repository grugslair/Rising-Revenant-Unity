using Dojo;
using Dojo.Starknet;
using System;

public class Revenant : ModelInstance
{
    [ModelField("game_id")]
    public UInt32 gameId;
    [ModelField("entity_id")]
    public FieldElement entityId;
    [ModelField("owner")]
    public FieldElement ownerAddress;
    [ModelField("first_name_idx")]
    public UInt32 firstNameIndex;
    [ModelField("last_name_idx")]
    public UInt32 lastNameIndex;
    [ModelField("status")]
    public UInt32 status;

    // this gets loaded before the outpost
    private void Start()
    {
        DojoEntitiesDataManager.revDictInstance.Add(RisingRevenantUtils.FieldElementToInt(entityId), this);

        if (ownerAddress.Hex() == DojoEntitiesDataManager.currentAccount.Address.Hex())
        {
            DojoEntitiesDataManager.ownOutpostIndex.Add(RisingRevenantUtils.FieldElementToInt(entityId));
        }
    }
}
