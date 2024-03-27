using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerPageBehaviour : Menu
{

    public TMP_Text addressName;
    public TMP_Text winningOutcomeText;
    public GameObject jackpotClaim;

    private Outpost lastOutpost;

    private void Awake()
    {
        UiEntitiesReferenceManager.winnerPageBehaviour = this;
    }

    private void OnEnable()
    {
        if (DojoEntitiesDataManager.gameEntCounter.outpostRemainingCount == 1)
        {
            foreach (var outpost in DojoEntitiesDataManager.outpostDictInstance.Values)
            {
                if (outpost.life > 0)
                {
                    lastOutpost = outpost;
                    break;
                }
            }
            
            if (lastOutpost.ownerAddress.Hex() == DojoEntitiesDataManager.currentAccount.Address.Hex())
            {
                addressName.text =  RisingRevenantUtils.ShortenAddress(DojoEntitiesDataManager.currentAccount.Address.Hex());
                winningOutcomeText.text = "YOU HAVE WON THE GAME, YOU ARE THE RISING REVENANT";
                jackpotClaim.SetActive(true);
            }
            else
            {
                addressName.text = RisingRevenantUtils.ShortenAddress(lastOutpost.ownerAddress.Hex());
                winningOutcomeText.text = "YOU HAVE LOST THE GAME, SOMEONE ELSE IS THE RISING REVENANT";
            }
        }
        else
        {
            addressName.text = RisingRevenantUtils.ShortenAddress( DojoEntitiesDataManager.currentAccount.Address.Hex());
            winningOutcomeText.text = $"how did you get here, there are still {DojoEntitiesDataManager.gameEntCounter.outpostRemainingCount} Outposts left";
        }
    }

    public async void ClaimContributionDojoCall()
    {
        var callStruct = new DojoCallsManager.ClaimContribEngameRewardsStruct
        {
            gameId = DojoEntitiesDataManager.currentGameId
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            account = DojoEntitiesDataManager.currentAccount,
            addressOfSystem = DojoEntitiesDataManager.worldManager.chainConfig.paymentActionsAddress,
            functionName = "claim_confirmation_contribution",
        };

        var transaction = await DojoCallsManager.ClaimContribEndgameRewardsDojoCall(callStruct, endpoint);
    }

    public async void ClaimJackpotDojoCall()
    {
        var callStruct = new DojoCallsManager.ClaimEndgameRewardsStruct
        {
            gameId = DojoEntitiesDataManager.currentGameId
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            account = DojoEntitiesDataManager.currentAccount,
            addressOfSystem = DojoEntitiesDataManager.worldManager.chainConfig.paymentActionsAddress,
            functionName = "claim_jackpot",
        };

        var transaction = await DojoCallsManager.ClaimEndgameRewardsDojoCall(callStruct, endpoint);
    }
}
