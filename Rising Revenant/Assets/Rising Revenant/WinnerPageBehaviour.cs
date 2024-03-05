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
                addressName.text = DojoEntitiesDataManager.currentAccount.Address.Hex();
                winningOutcomeText.text = "You have won the game, you are the Rising Revenant";
                jackpotClaim.SetActive(true);
            }
            else
            {
                addressName.text = lastOutpost.ownerAddress.Hex();
                winningOutcomeText.text = "You have lost the game, the last outpost standing is owned by someone else";
            }
        }
        else
        {
            addressName.text = DojoEntitiesDataManager.currentAccount.Address.Hex();
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
            addressOfSystem = DojoCallsManager.paymentActionsAddress,
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
            addressOfSystem = DojoCallsManager.paymentActionsAddress,
            functionName = "claim_jackpot",
        };

        var transaction = await DojoCallsManager.ClaimEndgameRewardsDojoCall(callStruct, endpoint);
    }
}
