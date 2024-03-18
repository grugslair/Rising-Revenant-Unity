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
            jackpotText.text = $"Jackpot: {RisingRevenantUtils.BigintToFloat(gamePot[1], 3)}";
        }
    }

    //devWallet
    public async void ChangeInPlayerSpecificData()
    {
        if (DojoEntitiesDataManager.currentDevWallet == null)
        {
            walletAmount.text = "300";
        }
        else
        {
            var devWallet = await RisingRevenantUtils.devWalletInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString() , DojoEntitiesDataManager.currentAccount.Address.Hex());
            walletAmount.text = RisingRevenantUtils.BigintToFloat(devWallet,3).ToString();
        }
    }


    //PlayerContribution
    //GameState
    public async void CalcContrib()
    {
        if (DojoEntitiesDataManager.playerContrib != null && DojoEntitiesDataManager.gameEntCounter != null)
        {
            if (DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal > 0)
            {
                if (contribText.transform.gameObject.activeSelf == false)
                {
                    contribText.transform.gameObject.SetActive(true);
                }

                var playerContrib = await RisingRevenantUtils.playerContributionInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString(), DojoEntitiesDataManager.currentAccount.Address.Hex());
                var totContrib = await RisingRevenantUtils.gameStateInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString());

                var perc = RisingRevenantUtils.GeneralHexToInt(playerContrib) / RisingRevenantUtils.GeneralHexToInt(totContrib) * 100;

                contribText.text = $"Contribution: {perc}%";
            }
            else
            {
                if (contribText.transform.gameObject.activeSelf == true)
                {
                    contribText.transform.gameObject.SetActive(false);
                }

                return;
            }
        }
    }
}
