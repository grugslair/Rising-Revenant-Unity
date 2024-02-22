using Dojo.Torii;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarUiElement : MonoBehaviour
{
    [SerializeField]
    private InitializeDojoEntities dojoEnts;

    public int phase = 1;

    public GameObject loggedIn;
    public Button notLoggedIn;

    public TMP_Text jackpotText;
    public TMP_Text contribText;
    public TMP_Text entGameDataText;
    public TMP_Text walletAddressText;

    public TMP_Text walletAmount;

    private PlayerInfo playerInfo = null;


    private void Update()
    {
        if (playerInfo == null && DojoEntitiesDataManager.playerSpecificData != null)
        {
            playerInfo = DojoEntitiesDataManager.playerSpecificData;
            playerInfo.OnValueChange += ChangeInPlayerSpecificData;
            ChangeInPlayerSpecificData();
        }
    }

    void OnEnable()
    {
        if (dojoEnts.burnerManager.CurrentBurner != null)
        {
            notLoggedIn.gameObject.SetActive(false);
            loggedIn.gameObject.SetActive(true);
            walletAddressText.text = DojoEntitiesDataManager.currentAccount.Address.Hex().Substring(0,7);
        }
        else
        {
            notLoggedIn.gameObject.SetActive(true);
            loggedIn.gameObject.SetActive(false);
        }


        if (DojoEntitiesDataManager.gameEntityCounterInstance != null)
        {
            ChangeInGameEntCounter();
            DojoEntitiesDataManager.gameEntityCounterInstance.OnValueChange += ChangeInGameEntCounter;
        }
        if (DojoEntitiesDataManager.gameDataInstance != null)
        {
            ChangeInGameData();
            DojoEntitiesDataManager.gameDataInstance.OnValueChange += ChangeInGameData;
        }
    }


    void OnDisable()
    {
        if (DojoEntitiesDataManager.gameEntityCounterInstance != null)
        {
            DojoEntitiesDataManager.gameEntityCounterInstance.OnValueChange -= ChangeInGameEntCounter;
        }

        if (DojoEntitiesDataManager.gameDataInstance != null)
        {
            DojoEntitiesDataManager.gameDataInstance.OnValueChange -= ChangeInGameData;
        }

        if (playerInfo != null)
        {
            playerInfo.OnValueChange -= ChangeInPlayerSpecificData;
        }
    }

    public void ChangeInGameEntCounter()
    {
        // Code to execute when any value changes in GameEntityCounter
        
        if (phase == 1)
        {
            entGameDataText.text = "Revenants Summoned: " + DojoEntitiesDataManager.gameEntityCounterInstance.revenantCount + "/" + DojoEntitiesDataManager.gameDataInstance.maxAmountOfRevenants + "\nReinforcements in game: " + (DojoEntitiesDataManager.gameEntityCounterInstance.reinforcementCount + DojoEntitiesDataManager.gameEntityCounterInstance.remainLifeCount);
        }
        else
        {
            entGameDataText.text = "Revenants Alive: " + DojoEntitiesDataManager.gameEntityCounterInstance.outpostExistsCount + "/" + DojoEntitiesDataManager.gameDataInstance.maxAmountOfRevenants + "\nReinforcements in game: " + (DojoEntitiesDataManager.gameEntityCounterInstance.reinforcementCount + DojoEntitiesDataManager.gameEntityCounterInstance.remainLifeCount);
        }

        CalcContrib(); 
    }

    public void ChangeInGameData()
    {

        if (phase == 1)
        {
            jackpotText.text = "Jackpot: " + RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.gameDataInstance.jackpot).ToString();
        }
        else
        {
            jackpotText.text = "Jackpot: " + RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.gameDataInstance.jackpot).ToString();
        }
    }

    public void ChangeInPlayerSpecificData()
    {
        if (playerInfo == null)
        {
            walletAmount.text = "0";
        }
        else
        {
            walletAmount.text = RisingRevenantUtils.FieldElementToInt(playerInfo.playerWalletAmount).ToString();
        }

        CalcContrib();
    }


    private void CalcContrib()
    {
        if (playerInfo != null && DojoEntitiesDataManager.gameEntityCounterInstance != null)
        {
            // Check if scoreCount is greater than 0 to avoid divide-by-zero error
            if (DojoEntitiesDataManager.gameEntityCounterInstance.scoreCount > 0)
            {
                // Cast score and scoreCount to a larger floating-point type to handle division correctly
                float percOfContrib = (float)playerInfo.score / DojoEntitiesDataManager.gameEntityCounterInstance.scoreCount * 100; // Multiply by 100 to get percentage
                contribText.text = $"Contribution: {percOfContrib}%";
            }
            else
            {
                // Handle the case where scoreCount is 0, e.g., by setting contribution to 0% or displaying a message
                contribText.text = "Contribution: 0%";
            }
        }
    }


}
