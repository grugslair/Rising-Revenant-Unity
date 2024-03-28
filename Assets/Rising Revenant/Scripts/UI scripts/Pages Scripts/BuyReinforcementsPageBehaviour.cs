using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class BuyReinforcementsPageBehaviour : Menu
{
    // call the function

    [SerializeField]
    private CounterUiElement counterUiElement;

    [SerializeField]
    private TMP_Text explenationText;
    [SerializeField]
    private TMP_Text confirmBuyText;
    [SerializeField]
    private TMP_Text staticPriceText;

    [SerializeField]
    private AudioClip[] soundEffects;


    private double pricePerReinforcement = 6;

    private string[] explanation = new string[2] {
        "Reinforcements provide an additional extra life to your outpost, enhancing the player's ability to withstand hostile attacks",
        "An outpost can only have a maximum of 20 reinforcements applied during its existance"};

    #region text update region
    private IEnumerator ChangeTextPeriodically()
    {
        int index = 0;
        while (true)
        {
            yield return FadeTextToZeroAlpha(0.5f, explenationText);

            explenationText.text = explanation[index];
            index = (index + 1) % explanation.Length; 

            yield return FadeTextToOneAlpha(0.5f, explenationText);

            yield return new WaitForSeconds(10);
        }
    }

    private IEnumerator FadeTextToZeroAlpha(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    private IEnumerator FadeTextToOneAlpha(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
    #endregion

    #region price update region
    
    private IEnumerator UpdatePrice()
    {
        while (true)
        {
            GetVrgdaPrice();
            yield return new WaitForSeconds(5);
            CalcNewTotal();
        }
    }

    public void GetVrgdaPrice()
    {

        if (DojoEntitiesDataManager.worldManager.chainConfig.environmentType != Dojo.EnvironmentType.TESTNET)
        {
            return;
        }

        try
        {
            string[] calldata = new string[0];
            string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
            JSInteropManager.CallContract(DojoEntitiesDataManager.worldManager.chainConfig.reinforcementActionsAddress, "balanceOf", calldataString, gameObject.transform.name, "VRGDACallback");
        }
        catch (Exception ex)
        {
            Debug.LogError($"error on the main function: {ex.Message}");
        }
    }

    public void VRGDACallback(string response)
    {
        try
        {
            JsonResponse jsonResponse = JsonUtility.FromJson<JsonResponse>(response);
            BigInteger balance = BigInteger.Parse(jsonResponse.result[0].Substring(2), NumberStyles.HexNumber);
            Debug.Log($"this si the price per reinf {RisingRevenantUtils.BigIntToFloat(balance,5)}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"error on the callback {ex.Message}");
        }
    }

    #endregion

    public void CalcNewTotal()
    {
        confirmBuyText.text = "Purchase (Tot: " + pricePerReinforcement * counterUiElement.currentValue + " $LORDS)";
    }

    public async void CallDojoBuyReinforcementsFunc()
    {
        SoundEffectManager.Instance.PlaySoundEffect(soundEffects[0], true);

        var createRevenantsProps = new DojoCallsManager.PurchaseReinforcementsStruct
        {
            gameId = DojoEntitiesDataManager.currentGameId,
            count = (uint)counterUiElement.currentValue,
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "purchase",
            addressOfSystem = DojoEntitiesDataManager.worldManager.chainConfig.reinforcementActionsAddress,
            account = DojoEntitiesDataManager.currentAccount,
            objectName = "Main_Canvas",
            callbackFunctionName = "OnChainTransactionCallbackFunction",
        };

        var transaction = await DojoCallsManager.PurchaseReinforcementsDojoCall(createRevenantsProps, endpoint);
    }



    private void OnDisable()
    {
        StopAllCoroutines(); // Stop the coroutine when the game object is disabled
    }
    private void OnEnable()
    {
        CalcNewTotal();
        StartCoroutine(ChangeTextPeriodically());
        StartCoroutine(UpdatePrice());
    }


}



