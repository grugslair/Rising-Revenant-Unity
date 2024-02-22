
using TMPro;
using UnityEngine;

public class EventPageDataContainerBehaviour : Menu
{
    public TMP_Text idText;
    public TMP_Text coordinatesText;
    public TMP_Text reinforcementsText;
    public TMP_Text nameText;

    public Outpost outpost;

    public void Initialize(int id)
    {
        outpost = DojoEntitiesDataManager.outpostDictInstance[id];

        outpost.OnValueChange += LoadData;
        LoadData();
    }


    private void OnDestroy()
    {
        outpost.OnValueChange -= LoadData;
    }

    public void LoadData()
    {
        DojoEntitiesDataManager.GetLatestEvent();

        idText.text = $"Outpost Id: {RisingRevenantUtils.FieldElementToInt(outpost.entityId)}";
        coordinatesText.text = $"Coordinates: X:{outpost.xPosition}, Y:{outpost.yPosition}";
        reinforcementsText.text = $"Reinforcements: {outpost.lifes}";

        nameText.text = RisingRevenantUtils.GetFullRevenantName(RisingRevenantUtils.FieldElementToInt(outpost.entityId));
    }

    public async void ValidateOutpost()
    {
        var validateStruct = new DojoCallsManager.DamageOutpostStruct { eventId = DojoEntitiesDataManager.GetLatestEvent().entityId, gameId = 1, outpostId = outpost.entityId };
        var endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoCallsManager.eventActionsAddress, functionName = "destroy_outpost" };

        var transaction = await DojoCallsManager.DamageOutpostDojoCall(validateStruct, endpoint);
    }

    //sub to the ent
    //funciton to validate
    // text stuff
}
