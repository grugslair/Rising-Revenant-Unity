using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutpostDataProfilePageListElement : MonoBehaviour
{
    public RawImage profilePicRev;
    public RawImage profilePicOut;
    public RawImage reinforcementPic;

    public GameObject goHereButton;
    public GameObject parentShields;

    public TMP_Text reinforcementTypeNameText;
    public TMP_Text coordinatesText;
    public TMP_Text reinforcementText;
    public TMP_Text nameText;

    public CounterUiElement counterUiElement;

    private RisingRevenantUtils.Vec2 entityId;
    private Outpost outpostData;


    public void LoadData()
    {
        coordinatesText.text = $"X:{outpostData.position.x}, Y:{outpostData.position.y}";
        reinforcementText.text = outpostData.life.ToString();

        RisingRevenantUtils.ReinforcementType reinfType = outpostData.reinforcementType;
        reinforcementTypeNameText.text = reinfType.ToCustomString();

        Texture2D reinfTypeImage = Resources.Load<Texture2D>($"Icons/{reinfType.ToCustomString()}");
        reinforcementPic.texture = reinfTypeImage;

        Texture2D revImage = Resources.Load<Texture2D>($"Revenants_Pics/{RisingRevenantUtils.GetConsistentRandomNumber((int)(outpostData.position.x * outpostData.position.y), RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId), 1, 24)}");
        profilePicRev.texture = revImage;

        int i = 1;
        foreach (Transform child in parentShields.transform)
        {
            if (i >= RisingRevenantUtils.CalculateShields(outpostData.life))
            {
                RawImage image = child.GetComponent<RawImage>();
                image.color = new Color(1, 1, 1, 0);
            }

            i++;
        }

        nameText.text = RisingRevenantUtils.GetFullRevenantName(entityId);
    }


    public void InitiateData(RisingRevenantUtils.Vec2 entityId, int phase)
    {
        if (phase == 1)
        {
            goHereButton.SetActive(false);
        }

        this.entityId = entityId;

        outpostData = DojoEntitiesDataManager.outpostDictInstance[entityId];

        LoadData();

        outpostData.OnValueChange += LoadData;
    }

    private void OnDestroy()
    {
        outpostData.OnValueChange -= LoadData;
    }

    private void OnDisable()
    {
        outpostData.OnValueChange -= LoadData;
    }

    public void GoHere()
    {
        CameraController.Instance.transform.position = new Vector3(outpostData.position.x, CameraController.Instance.transform.position.y, outpostData.position.y);
        //CameraController.Instance.MoveCameraToPosition(outpostData.position);
    }

    public async void ReinforceOutpost()
    {
        DojoCallsManager.ReinforceOutpostStruct callStructure = new DojoCallsManager.ReinforceOutpostStruct { 
            count = (UInt32)counterUiElement.currentValue, 
            gameId = DojoEntitiesDataManager.currentGameId, 
            outpostId = outpostData.position 
        };

        DojoCallsManager.EndpointDojoCallStruct endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoCallsManager.outpostActionsAddress, functionName = "reinforce" };

        var transaction = await DojoCallsManager.ReinforceOutpostDojoCall(callStructure, endpoint);
    }

    public void OpenReinforceTypeMenu()
    {
        UiEntitiesReferenceManager.profilePageBehaviour.currentlySelectedOutpost = entityId;
    }

}
