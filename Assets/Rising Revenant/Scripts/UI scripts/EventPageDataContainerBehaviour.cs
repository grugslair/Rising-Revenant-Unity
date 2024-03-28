
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPageDataContainerBehaviour : Menu
{
    public TMP_Text idText;
    public TMP_Text coordinatesText;
    public TMP_Text reinforcementsText;
    public TMP_Text nameText;

    public RawImage profilePicRev;

    public Outpost outpost;

    public void Initialize(RisingRevenantUtils.Vec2 id)
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
        idText.text = $"Outpost Id: {RisingRevenantUtils.CantonPair((int)outpost.position.x, (int)outpost.position.y)}";
        coordinatesText.text = $"Coordinates: X:{outpost.position.x}, Y:{outpost.position.y}";
        reinforcementsText.text = $"Reinforcements: {outpost.life}";

        Texture2D revImage = Resources.Load<Texture2D>($"Revenants_Pics/{RisingRevenantUtils.GetConsistentRandomNumber((int)(outpost.position.x * outpost.position.y), RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId), 1, 24)}");
        profilePicRev.texture = revImage;

        nameText.text = RisingRevenantUtils.GetFullRevenantName(outpost.position);
    }

    public async void ValidateOutpost()
    {
        Debug.Log("called on valid");

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            account = DojoEntitiesDataManager.currentAccount,
            addressOfSystem = DojoEntitiesDataManager.worldManager.chainConfig.outpostActionsAddress,
            functionName = "verify",
            objectName = "Main_Canvas",
            callbackFunctionName = "OnChainTransactionCallbackFunction",
        };
        var validateStruct = new DojoCallsManager.DamageOutpostStruct {  gameId = DojoEntitiesDataManager.currentGameId, outpostId = outpost.position };
       
        var transaction = await DojoCallsManager.DamageOutpostDojoCall(validateStruct, endpoint);

        Destroy(gameObject);
    }

    //sub to the ent
    //funciton to validate
    // text stuff
}
