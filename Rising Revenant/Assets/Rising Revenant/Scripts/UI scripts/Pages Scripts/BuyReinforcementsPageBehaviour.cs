using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void CalcNewTotal()
    {
        confirmBuyText.text = "Purchase (Tot: " + pricePerReinforcement * counterUiElement.currentValue + " $LORDS)";
    }

    private async Task CallDojoBuyReinforcementsFunc()
    {
        var gamePot = await RisingRevenantUtils.gamePotInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString());
        var oldValueJackpot = RisingRevenantUtils.BigintToFloat(gamePot[1], 3);

        Debug.Log("old value " + oldValueJackpot);

        SoundEffectManager.Instance.PlaySoundEffect(soundEffects[0], true);

        var createRevenantsProps = new DojoCallsManager.PurchaseReinforcementsStruct
        {
            gameId = DojoEntitiesDataManager.currentGameId,
            count = (uint)counterUiElement.currentValue,
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "purchase",
            addressOfSystem = DojoCallsManager.reinforcementActionsAddress,
            account = DojoEntitiesDataManager.currentAccount,
        };

        var transaction = await DojoCallsManager.PurchaseReinforcementsDojoCall(createRevenantsProps, endpoint);

        // Wait for 1 second
        await Task.Delay(TimeSpan.FromSeconds(1));

        gamePot = await RisingRevenantUtils.gamePotInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString());
        var newValueJackpot = RisingRevenantUtils.BigintToFloat(gamePot[1], 3);

        var diff = newValueJackpot - oldValueJackpot;
        pricePerReinforcement = Math.Round(diff / counterUiElement.currentValue, 3);

        staticPriceText.text = $"1 Reinforcement = {pricePerReinforcement} $LORDS";
    }


    IEnumerator CallDojoBuyReinforcementsCoroutine()
    {
        yield return CallDojoBuyReinforcementsFunc();
    }

    public void OnButtonClick()
    {
        StartCoroutine(CallDojoBuyReinforcementsCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines(); // Stop the coroutine when the game object is disabled
    }
    private void OnEnable()
    {
        CalcNewTotal();
        StartCoroutine(ChangeTextPeriodically());
    }
}



