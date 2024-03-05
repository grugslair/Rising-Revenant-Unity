using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using System;

public class PlayerInfo : ModelInstance
{
    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("player_id")]
    public FieldElement playerId;

    [ModelField("outpost_count")]
    public UInt32 outpostCount;

    [ModelField("reinforcements_available_count")]
    public UInt32 reinforcementsAvailableCount;

    [ModelField("init")]
    public bool init;

    // Start is called before the first frame update
    void Start()
    {
        if (playerId.Hex() == DojoEntitiesDataManager.currentAccount.Address.Hex())
        {
            DojoEntitiesDataManager.playerInfo = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);
        if (UiEntitiesReferenceManager.reinforcementCounterElement != null)
        {
            UiEntitiesReferenceManager.reinforcementCounterElement.SetTextValue(this);
        }
    }
}
