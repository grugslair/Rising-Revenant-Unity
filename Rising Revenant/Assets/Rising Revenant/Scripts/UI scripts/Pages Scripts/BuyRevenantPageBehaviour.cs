using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BuyRevenantPageBehaviour : Menu
{
    // call the function

    [SerializeField]
    private CounterUiElement counterUiElement;

    [SerializeField]
    private RawImage backgroundImage;

    [SerializeField]
    private TMP_Text explenationText;
    [SerializeField]
    private TMP_Text confirmBuyText;
    [SerializeField]
    private TMP_Text staticPriceText;

    private string[] explanation = new string[2] { 
        "Summoning a Revenant will allow you to call forth a powerful ally from the realm of the undead",
        "This Revenant, after being summoned successfully, will settle and be responsible for protecting an outpost with the goal of being the last one alive" };

    //private void Start()
    //{
    //    int randomNum = Random.Range(1, 30);
    //    string path = "RandomBuyRevPics/" + randomNum;

    //    Texture2D image = Resources.Load<Texture2D>(path);

    //    if (image != null)
    //    {
    //        backgroundImage.texture = image;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Failed to load image at path: " + path);
    //    }
    //}

    private IEnumerator ChangeTextPeriodically()
    {
        int index = 0;
        while (true) // Infinite loop to keep changing the text
        {
            // Fade out
            yield return FadeTextToZeroAlpha(0.5f, explenationText);

            // Update text
            explenationText.text = explanation[index];
            index = (index + 1) % explanation.Length; // Cycle through the array

            // Fade in
            yield return FadeTextToOneAlpha(0.5f, explenationText);

            // Wait for 10 seconds before changing the text again
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
        confirmBuyText.text = "Summon (Tot: " + (DojoEntitiesDataManager.outpostMarketData.pricePerOutpost * counterUiElement.currentValue).ToString() + " $LORDS)";
    }

    public async void CallDojoSummonRevFunc()
    {
        var createRevenantsProps = new DojoCallsManager.SummonRevenantStruct
        {
            gameId = DojoEntitiesDataManager.currentGameId,
            count = (uint)counterUiElement.currentValue,
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "purchase",
            addressOfSystem = DojoCallsManager.outpostActionsAddress,
            account = DojoEntitiesDataManager.currentAccount,
        };

        var transaction = await DojoCallsManager.SummonRevenantsDojoCall(createRevenantsProps, endpoint);
    }

    private void OnDisable()
    {
        StopAllCoroutines(); // Stop the coroutine when the game object is disabled
    }

    private void OnEnable()
    {
        CalcNewTotal();
        StartCoroutine(ChangeTextPeriodically());

        staticPriceText.text = "1 Revenant = " + DojoEntitiesDataManager.outpostMarketData.pricePerOutpost + " $LORDS";


        int randomNum = Random.Range(1, 30);
        string path = "RandomBuyRevPics/" + randomNum;

        Texture2D image = Resources.Load<Texture2D>(path);

        if (image != null)
        {
            backgroundImage.texture = image;
        }
        else
        {
            Debug.LogWarning("Failed to load image at path: " + path);
        }
    }

}
