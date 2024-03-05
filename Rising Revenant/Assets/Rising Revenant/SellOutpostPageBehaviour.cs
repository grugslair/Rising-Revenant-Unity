using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellOutpostPageBehaviour : Menu
{
    [SerializeField]
    private GameObject shieldContainer;
    [SerializeField]
    private RawImage outpostPPImage;

    [SerializeField]
    private TMP_Text outpostDataText;
    [SerializeField]
    private TMP_Text outpostPositionText;

    [SerializeField]
    private TMP_InputField costInputField;

    [SerializeField]
    private Outpost currentlySelectedOutpost;
    
    private int currentOutpostIndex;


    private void OnEnable()
    {
        // get the first owned outpost data
        // if there are no outposts, return

        if (DojoEntitiesDataManager.ownOutpostIndex.Count == 0) {
            currentOutpostIndex = -1;
            return; 
        }

        SetTextsAndData(0);
        currentOutpostIndex = 0;
    }

    public void CycleThroughOutposts(int dir)
    {
        var newIdx = currentOutpostIndex + dir;

        if (newIdx < 0 || newIdx >= DojoEntitiesDataManager.ownOutpostIndex.Count) { return; }

        SetTextsAndData(newIdx);
    }

    private void SetTextsAndData(int idx)
    {
        currentOutpostIndex = idx;
        currentlySelectedOutpost = DojoEntitiesDataManager.outpostDictInstance[DojoEntitiesDataManager.ownOutpostIndex[idx]];

        outpostDataText.text = $"Outpost Id: {-1} \n" +
                             $"Lifes: {currentlySelectedOutpost.life} \n" +
                             $"Reinf Left: {currentlySelectedOutpost.reinforcesRemaining} \n" +
                             $"Rev name: {RisingRevenantUtils.GetFullRevenantName(currentlySelectedOutpost.position)}";

        outpostPositionText.text = $"Position:\nX: {currentlySelectedOutpost.position.x} Y: {currentlySelectedOutpost.position.y}";
    }

    public async void SellOutpost()
    {
        if (costInputField.text == "") { return; }

        DojoCallsManager.CreateTradeRevenantStruct structToSellReinf = new DojoCallsManager.CreateTradeRevenantStruct
        {
            revenantId = currentlySelectedOutpost.position,
            gameId = DojoEntitiesDataManager.currentGameId,
            price = new Dojo.Starknet.FieldElement(int.Parse(costInputField.text))
        };

        DojoCallsManager.EndpointDojoCallStruct endPoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            account = DojoEntitiesDataManager.currentAccount,
            addressOfSystem = DojoCallsManager.tradeOutpostActionsAddress,
            functionName = "create",
        };

        await DojoCallsManager.CreateTradeRevenantDojoCall(structToSellReinf, endPoint);
    }



}
