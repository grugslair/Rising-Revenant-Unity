using System.Collections;
using System.Collections.Generic;
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
        confirmBuyText.text = "Purchase (Tot: " + RisingRevenantUtils.ConvertLargeNumberToString(DojoEntitiesDataManager.outpostMarketData.pricePerOutpost, 2) + " $LORDS)";
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
            addressOfSystem = DojoCallsManager.reinforcementActionsAddress,
            account = DojoEntitiesDataManager.currentAccount,
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

        staticPriceText.text = "1 Reinforcement = " + "VRGDA goes here" + " $LORDS";
    }
}



