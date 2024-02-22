using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutpostDataProfilePageListElement : MonoBehaviour
{
    public RawImage profilePicRev;
    public RawImage profilePicOut;

    public GameObject goHereButton;
    public GameObject parentShields;

    public TMP_Text outpostIdText;
    public TMP_Text coordinatesText;
    public TMP_Text reinforcementText;
    public TMP_Text nameText;

    public CounterUiElement counterUiElement;

    private int entityId;
    private Revenant revData;
    private Outpost outpostData;
    

    public void LoadData()
    {
        //var randomPP = RisingRevenantUtils.GenerateRandomNumber(entityId, 25);
        outpostIdText.text = RisingRevenantUtils.FieldElementToInt(outpostData.entityId).ToString();
        coordinatesText.text = $"X:{outpostData.xPosition}, Y:{outpostData.yPosition}";
        reinforcementText.text = outpostData.lifes.ToString();

        int i = 1;
        foreach (Transform child in parentShields.transform)
        {
            if (i >= outpostData.shield)
            {
                RawImage image = child.GetComponent<RawImage>();
                image.color = new Color(1, 1, 1, 0);
            }

            i++;
        }

        nameText.text = RisingRevenantUtils.GetFullRevenantName(entityId);
    }


    public void InitiateData(int entityId, int phase)
    {

        if (phase == 1)
        {
            goHereButton.SetActive(false);
        }

        this.entityId = entityId;

        revData = DojoEntitiesDataManager.revDictInstance[entityId];
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
        Debug.Log("call to move cam");
    }

    public async void ReinforceOutpost()
    {
        DojoCallsManager.ReinforceOutpostStruct callStructure = new DojoCallsManager.ReinforceOutpostStruct { count = (UInt32)counterUiElement.currentValue, gameId = 1, outpostId = new Dojo.Starknet.FieldElement(entityId) };
        DojoCallsManager.EndpointDojoCallStruct endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoCallsManager.revenantActionsAddress, functionName = "reinforce_outpost" };

        var transaction = await DojoCallsManager.ReinforceOutpostDojoCall(callStructure, endpoint);
    }

}
