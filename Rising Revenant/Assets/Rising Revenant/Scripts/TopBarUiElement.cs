using Dojo.Torii;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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
    public async void ChangeInGameData()
    {
        if (DojoEntitiesDataManager.gamePot != null)
        {
            var gamePot = await RisingRevenantUtils.gamePotInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString());
            jackpotText.text = "Jackpot: " +   RisingRevenantUtils.HexToFloat(gamePot[1]);
        }
    }

    //devWallet
    public async void ChangeInPlayerSpecificData()
    {
        if (DojoEntitiesDataManager.currentDevWallet == null)
        {
            walletAmount.text = "150";
        }
        else
        {
            var devWallet = await RisingRevenantUtils.devWalletInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString() , DojoEntitiesDataManager.currentAccount.Address.Hex());
            walletAmount.text = RisingRevenantUtils.HexToFloat(devWallet).ToString();
        }
    }


    //PlayerContribution
    //GameState
    public async void CalcContrib()
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

                var playerContrib = await RisingRevenantUtils.playerContributionInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString(), DojoEntitiesDataManager.currentAccount.Address.Hex());
                Debug.Log("playerContrib: " + playerContrib);
                Debug.Log("this si after the hex thing " + RisingRevenantUtils.HexToFloat(playerContrib) );

                var totContrib = await RisingRevenantUtils.gameStateInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString());
                Debug.Log("totContrib: " + totContrib);
                Debug.Log("this si after the hex thing " + RisingRevenantUtils.HexToFloat(totContrib) );

                //float percOfContrib = RisingRevenantUtils.HexToFloat(DojoEntitiesDataManager.playerContrib.scoreString, 3) / RisingRevenantUtils.HexToFloat(DojoEntitiesDataManager.gameEntCounter.contributionScoreTotalString, 3) * 100; // Multiply by 100 to get percentage
                contribText.text = $"Contribution: {0}%";
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
