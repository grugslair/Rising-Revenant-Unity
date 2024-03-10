using Dojo.Starknet;
using TMPro;
using UnityEngine;

public class ChangePriceTooltip : MonoBehaviour
{
    private int type;
    private FieldElement tradeId;

    [SerializeField]
    TMP_InputField inputField;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">1 for outpost   2 for reinf</param>
    /// <param name="tradeId"></param>
    public void Initilize(int type, FieldElement tradeId)
    {
        this.type = type;
        this.tradeId = tradeId;
    }

    public async void ConfirmPriceChange()
    {
        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            account = DojoEntitiesDataManager.currentAccount,
            addressOfSystem = type == 1 ? DojoCallsManager.tradeOutpostActionsAddress : DojoCallsManager.tradeReinforcementActionsAddress,
            functionName = "modify_price",
        };

        Debug.Log(tradeId.Hex());
        Debug.Log(inputField.text);
        Debug.Log(int.Parse(inputField.text));

        if (type == 1)
        {
            var dataStruct =  new DojoCallsManager.ModifyTradeRevenantStruct
            {
                tradeId = tradeId,
                priceRevenant = new FieldElement(int.Parse(inputField.text).ToString("X")),
                gameId = DojoEntitiesDataManager.currentGameId

            };

            await DojoCallsManager.ModifyTradeRevenantDojoCall(dataStruct, endpoint);
        }
        else
        {
            var dataStruct = new DojoCallsManager.ModifyTradeReinforcementStruct
            {
                tradeId = tradeId,
                priceReinforcemnt = new FieldElement(int.Parse(inputField.text).ToString("X")),
                gameId = DojoEntitiesDataManager.currentGameId

            };

            await DojoCallsManager.ModifyTradeReinforcementDojoCall(dataStruct, endpoint);
        }

    }

    public void CloseTooltip()
    {
        UiEntitiesReferenceManager.tooltipCompBehaviour.CloseTooltip();
    }

}
