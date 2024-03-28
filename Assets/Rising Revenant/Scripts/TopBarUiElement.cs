using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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

    public TooltipAsker contribTooltip;

    /*
        GameState 
        outpostMarketData
        gamepot
        devwallet
        playerocntub
     */

    void OnEnable()
    {
        if (DojoEntitiesDataManager.currentAccount.Address.Hex() != "")
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
            //var gamePot = await RisingRevenantUtils.gamePotInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString());
            //jackpotText.text = $"Jackpot: {RisingRevenantUtils.BigintToFloat(gamePot[1], 3)}";

            jackpotText.text = $"Jackpot: { RisingRevenantUtils.BigIntToFloat(DojoEntitiesDataManager.gamePot.winnersPot.low,5)}";
        }
    }

    //devWallet
    public void ChangeInPlayerSpecificData()
    {
        if (DojoEntitiesDataManager.currentDevWallet == null)
        {
            walletAmount.text = "300";
        }
        else
        {
            //var devWallet = await RisingRevenantUtils.devWalletInfo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString() , DojoEntitiesDataManager.currentAccount.Address.Hex());
            //walletAmount.text = RisingRevenantUtils.BigintToFloat(devWallet,3).ToString();


            jackpotText.text = $"Jackpot: {RisingRevenantUtils.BigIntToFloat(DojoEntitiesDataManager.currentDevWallet.balance.low, 5)}";
        }
    }


    //PlayerContribution
    //GameState
    public void CalcContrib()
    {
        if (DojoEntitiesDataManager.playerContrib != null && DojoEntitiesDataManager.gameEntCounter != null)
        {
            if (DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal.low > 0)
            {
                if (contribText.transform.gameObject.activeSelf == false)
                {
                    contribText.transform.gameObject.SetActive(true);
                }

                //check if any of the two are 0 in case return

                if (DojoEntitiesDataManager.playerContrib.score.low == 0 || DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal.low == 0)
                {
                    return;
                }

                var perc = RisingRevenantUtils.BigIntToFloat(DojoEntitiesDataManager.playerContrib.score.low, 5) / RisingRevenantUtils.BigIntToFloat(DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal.low, 5) * 100;

                contribText.text = $"Contribution: {perc}%";
                contribTooltip.message = $"Total game contribution: {RisingRevenantUtils.BigIntToFloat(DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal.low, 5)}\nYour contribution: {RisingRevenantUtils.BigIntToFloat(DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal.low, 5)}";
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
