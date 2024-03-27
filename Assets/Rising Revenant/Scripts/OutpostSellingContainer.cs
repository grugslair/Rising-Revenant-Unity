using Dojo.Starknet;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OutpostSellingContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject shieldParentContainer;
    [SerializeField]
    private GameObject tooltipObj;

    [SerializeField]
    private TMP_Text outpostIDText;
    [SerializeField]
    private TMP_Text outpostDataText;
    [SerializeField]
    private TMP_Text outpostPriceText;

    [SerializeField]
    private Button actionButton;

    private RisingRevenantUtils.Vec2 outpostID;
    private FieldElement tradeId;
    private bool owner;

    [SerializeField]
    private TooltipAsker tooltipAsker;

    public void Initilize(RisingRevenantUtils.Vec2 id, string price, string tradeId)
    {
        outpostID = id;

        Outpost outpost = DojoEntitiesDataManager.outpostDictInstance[id];

        outpostIDText.text = $"ID: {RisingRevenantUtils.CantonPair((int)outpost.position.x, (int)outpost.position.y)}";
        outpostDataText.text = $"Reinforcements: {outpost.life}\n" +
            $"Reinforcement left: {outpost.reinforcesRemaining}\n" +
            $"Reinforcement type: {outpost.reinforcementType.ToCustomString()}\n" + 
            $"Owner: {outpost.ownerAddress.Hex().Substring(0,7)}";

        outpostPriceText.text = RisingRevenantUtils.GeneralHexToInt(price) + " $LORDS";

        actionButton.onClick.RemoveAllListeners();

        int i = 1;
        foreach (Transform child in shieldParentContainer.transform)
        {
            if (i >= RisingRevenantUtils.CalculateShields(outpost.life))
            {
                RawImage image = child.GetComponent<RawImage>();
                image.color = new Color(1, 1, 1, 0);
            }

            i++;
        }

        this.tradeId = new Dojo.Starknet.FieldElement(tradeId);

        if (outpost.ownerAddress.Hex() == DojoEntitiesDataManager.currentAccount.Address.Hex())
        {
            owner = true;
            actionButton.onClick.AddListener(RevokeTrade);

            var tmproText = actionButton.GetComponentInChildren<TextMeshProUGUI>();
            if (tmproText != null)
            {
                tmproText.text = "REVOKE";
            }

            tooltipAsker.OnTooltipShown += OnTooltipEnable;
        }
        else
        {
            owner = false;
            actionButton.onClick.AddListener(PurchaseTrade);

            var tmproText = actionButton.GetComponentInChildren<TextMeshProUGUI>();
            if (tmproText != null)
            {
                tmproText.text = "BUY NOW";
            }
        }
    }

    public void GoHereWithCam()
    {
        CameraController.Instance.transform.position = new Vector3(outpostID.x, CameraController.Instance.transform.position.y, outpostID.y);
    }

    public async void RevokeTrade()
    {
        if (owner)
        {
            var revokeStruct = new DojoCallsManager.RevokeTradeRevenantStruct { gameId = new Dojo.Starknet.FieldElement(0), tradeId = tradeId };
            var endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoEntitiesDataManager.worldManager.chainConfig.tradeOutpostActionsAddress, functionName = "revoke" };

            await DojoCallsManager.RevokeTradeRevenantDojoCall(revokeStruct, endpoint);
        }
    }

    private void OnTooltipEnable(GameObject tooltip)
    {
        tooltip.GetComponent<ChangePriceTooltip>().Initilize(1, tradeId);
    }

    public async void PurchaseTrade()
    {
        if (!owner)
        {
            var purchaseStruct = new DojoCallsManager.PurchaseTradeRevenantStruct { gameId = new Dojo.Starknet.FieldElement(0), tradeId = tradeId };
            var endpoint = new DojoCallsManager.EndpointDojoCallStruct { account = DojoEntitiesDataManager.currentAccount, addressOfSystem = DojoEntitiesDataManager.worldManager.chainConfig.tradeOutpostActionsAddress, functionName = "purchase" };

            await DojoCallsManager.PurchaseTradeRevenantDojoCall(purchaseStruct, endpoint);
        }
    }
}
