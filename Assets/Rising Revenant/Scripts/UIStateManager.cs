using Dojo.Starknet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class UIStateManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] UIScreenObjs = new GameObject[4];

    private void Awake()
    {
        UiEntitiesReferenceManager.UIStateManager = this;
    }

    public enum UIState
    {
        LOGIN = 0,
        LOADING,
        VIDEO,
        PREP,
        GAME
    }

    public UIState currentUiState = UIState.LOGIN;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Y))
        {
            SetUiState(0);
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
            SetUiState(1);
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            SetUiState(2);
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            SetUiState(3);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            SetUiState(4);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            Debug.Log("\n\nDebug has been pressed");
            Debug.Log("Current UI State: " + currentUiState);
            Debug.Log($"\n\nAll current game data");
            Debug.Log("Current Game ID: " + DojoEntitiesDataManager.currentGameId.Hex());

            if (DojoEntitiesDataManager.outpostMarketData != null)
            {
                Debug.Log($"Outpost Market Data: \nGame id: {DojoEntitiesDataManager.outpostMarketData.gameId.Hex()}" +
                    $"\nprice per outpost:  {DojoEntitiesDataManager.outpostMarketData.pricePerOutpost.low}" +
                    $"\navailable outpost: {DojoEntitiesDataManager.outpostMarketData.maxAmountOfOutposts}" );
            }
            else
            {
                Debug.Log("Outpost Market Data: null");
            }

            if (DojoEntitiesDataManager.gamePot != null)
            {
                Debug.Log($"Game State Data: \nGame id: {DojoEntitiesDataManager.gamePot.gameId.Hex()}" +
                    $"\nwinner pot:  {DojoEntitiesDataManager.gamePot.winnersPot.low}" +
                    $"\ntotal pot: {DojoEntitiesDataManager.gamePot.totalPot.low}" +
                    $"\nconfirmation pot: {DojoEntitiesDataManager.gamePot.confirmationPot.low}" +
                    $"\ndev pot: {DojoEntitiesDataManager.gamePot.confirmationPot.low}");
            }
            else
            {
                Debug.Log("gamePot Data: null");
            }

            if (DojoEntitiesDataManager.currentWorldEvent != null)
            {
                Debug.Log($"Current World Event Data:" +
                          $"\nGame id: {DojoEntitiesDataManager.currentWorldEvent.gameId}" +
                          $"\nEvent id: {DojoEntitiesDataManager.currentWorldEvent.eventId}" +
                          $"\nPosition: {DojoEntitiesDataManager.currentWorldEvent.position}" +
                          $"\nRadius: {DojoEntitiesDataManager.currentWorldEvent.radius}" +
                          $"\nEvent type: {DojoEntitiesDataManager.currentWorldEvent.eventType}" +
                          $"\nNumber: {DojoEntitiesDataManager.currentWorldEvent.number}" +
                          $"\nBlock number: {DojoEntitiesDataManager.currentWorldEvent.blockNumber}" +
                          $"\nPrevious event: {DojoEntitiesDataManager.currentWorldEvent.previousEvent}");
            }
            else
            {
                Debug.Log("Current World Event Data: null");
            }

            if (DojoEntitiesDataManager.gameEntCounter != null)
            {
                Debug.Log($"Game State Data:" +
                          $"\nGame id: {DojoEntitiesDataManager.gameEntCounter.gameId}" +
                          $"\nOutpost created count: {DojoEntitiesDataManager.gameEntCounter.outpostCreatedCount}" +
                          $"\nOutpost remaining count: {DojoEntitiesDataManager.gameEntCounter.outpostRemainingCount}" +
                          $"\nRemain life count: {DojoEntitiesDataManager.gameEntCounter.remainLifeCount}" +
                          $"\nReinforcement count: {DojoEntitiesDataManager.gameEntCounter.reinforcementCount}" +
                          $"\nContribution score total: {DojoEntitiesDataManager.gameEntCounter.contributionScoreTotal}");
            }
            else
            {
                Debug.Log("Game State Data: null");
            }


            Debug.Log("\n\n");

            Debug.Log($"This is the number of outposts {DojoEntitiesDataManager.outpostDictInstance.Count}");
        }

        //if (Input.GetKeyUp(KeyCode.H))
        //{
        //    UiEntitiesReferenceManager.notificationManager.CreateNotification("Test", null, 5);
        //}
    }

    /// <summary>
    /// need to take in an int otherwise buttons dont recognise it, so just cast the enum when called
    /// </summary>
    /// <param name="stateNum"></param>
    public void SetUiState(int stateNum)
    {

        if (stateNum == (int)currentUiState)
        {
            return;
        }

        for (int i = 0; i < UIScreenObjs.Length; i++)
        {
            if (i == (int)stateNum)
            {
                UIScreenObjs[i].SetActive(true);
            }
            else
            {
                UIScreenObjs[i].SetActive(false);
            }
        }
    }

    public void OnChainTransactionCallbackFunction(string response)
    {
        JsonResponse jsonResponse = JsonUtility.FromJson<JsonResponse>(response);
        Debug.Log(jsonResponse.result);
    }
}
