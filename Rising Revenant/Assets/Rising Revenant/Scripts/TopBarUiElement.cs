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

    /*
        GameState 
        outpostMarketData
        gamepot
        devwallet
        playerocntub
     */

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

        UiEntitiesReferenceManager.topBarUiElement = this;

        ChangeInGameEntCounter();
        CalcContrib();
        ChangeInPlayerSpecificData();
        CalcContrib();
        ChangeInGameEntCounter();
        ChangeInGameData();
    }

    //GameState
    //outpostMarketData
    public void ChangeInGameEntCounter()
    {
        if (DojoEntitiesDataManager.gameEntCounter != null && DojoEntitiesDataManager.outpostMarketData != null) 
        {
            if (phase == 1)
            {
                entGameDataText.text = "Revenants Summoned: " + DojoEntitiesDataManager.gameEntCounter.outpostCreatedCount + "/" + ( DojoEntitiesDataManager.gameEntCounter.outpostCreatedCount + DojoEntitiesDataManager.outpostMarketData.maxAmountOfOutposts ) + "\nReinforcements in game: " + (DojoEntitiesDataManager.gameEntCounter.reinforcementCount + DojoEntitiesDataManager.gameEntCounter.remainLifeCount);
            }
            else
            {
                entGameDataText.text = "Revenants Alive: " + DojoEntitiesDataManager.gameEntCounter.outpostRemainingCount + "/" + DojoEntitiesDataManager.gameEntCounter.outpostCreatedCount + "\nReinforcements in game: " + (DojoEntitiesDataManager.gameEntCounter.reinforcementCount + DojoEntitiesDataManager.gameEntCounter.remainLifeCount);
            }
        }
    }

    //gamepot
    public void ChangeInGameData()
    {
        if (DojoEntitiesDataManager.gamePot != null)
        {
            jackpotText.text = "Jackpot: " + RisingRevenantUtils.ConvertLargeNumberToString(DojoEntitiesDataManager.gamePot.winnersPot,2);
        }
    }

    //devWallet
    public void ChangeInPlayerSpecificData()
    {
        if (DojoEntitiesDataManager.currentDevWallet == null)
        {
            walletAmount.text = "150";
        }
        else
        {
            walletAmount.text = RisingRevenantUtils.ConvertLargeNumberToString(DojoEntitiesDataManager.currentDevWallet.balance,2);
        }
    }


    //PlayerContribution
    //GameState
    public void CalcContrib()
    {
        if (DojoEntitiesDataManager.playerContrib != null && DojoEntitiesDataManager.gameEntCounter != null)
        {
            Debug.Log("DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal: " + DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal);
            Debug.Log("DojoEntitiesDataManager.playerContrib.score: " + DojoEntitiesDataManager.playerContrib.score);

            if (DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal > 0)
            {
                if (contribText.transform.gameObject.activeSelf == false)
                {
                    contribText.transform.gameObject.SetActive(true);
                }

                float percOfContrib = (float)DojoEntitiesDataManager.playerContrib.score / (float)DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal * 100; // Multiply by 100 to get percentage
                contribText.text = $"Contribution: {percOfContrib}%";
            }
            else
            {
                if (contribText.transform.gameObject.activeSelf == true)
                {
                    contribText.transform.gameObject.SetActive(false);
                    //return;
                }

                return;
                //contribText.text = "Contribution: 0%";
            }
        }
    }


}
