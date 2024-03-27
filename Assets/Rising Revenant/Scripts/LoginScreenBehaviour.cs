using Dojo;
using Dojo.Starknet;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* this script should

check if there is a burner
create a burner
 if there is login

if there isnt ask to make one

if they enter as a guest this should connect to the singleton
 */


public class LoginScreenBehaviour : Menu
{

    public InitializeDojoEntities initializeDojoEntitiesScript;
    public UIStateManager uiStateManager;
    [SerializeField] WalletConnect walletConnect;
    [SerializeField] WorldManager worldManager;

    //public TextMeshProUGUI loginButtonText;
    //public Button loginButton;

    [SerializeField] private List<GameObject> loginButtonObjs;

    private void OnEnable()
    {
        switch (worldManager.chainConfig.environmentType)
        {
            case EnvironmentType.LOCAL:
                SetCorrectLoginBox(0);
                WalletConnect.SuccessfulConnection += OnSuccesfullConnection;
                break;

            case EnvironmentType.TORII:
                SetCorrectLoginBox(0);
                break;

            case EnvironmentType.TESTNET:
                SetCorrectLoginBox(1);
                break;
        }
    }

    private void OnSuccesfullConnection()
    {
        SetCorrectLoginBox(2);

        loginButtonObjs[2].GetComponentInChildren<TextMeshProUGUI>().text = $"Connected with {DojoEntitiesDataManager.currentAccount.Address.Hex().Substring(0,6)}";

        WalletConnect.SuccessfulConnection -= OnSuccesfullConnection;
    }

    public void SetCorrectLoginBox(int boxNum)
    {
        for (int i = 0; i < loginButtonObjs.Count; i++)
        {
            if (i == boxNum)
            {
                loginButtonObjs[i].SetActive(true);
            }
            else
            {
                loginButtonObjs[i].SetActive(false);
            }
        }
    }

    public void JoinAsGuest()
    {
        DojoEntitiesDataManager.currentAccount = new Account(null, null, null);
        //set something to null
    }

    public void CreateBurner()
    {
        if (initializeDojoEntitiesScript.burnerManager.CurrentBurner != null)
        {
            SetCorrectLoginBox(2);
            loginButtonObjs[2].GetComponentInChildren<TextMeshProUGUI>().text = $"Connected with {DojoEntitiesDataManager.currentAccount.Address.Hex().Substring(0, 6)}";
        }
    }

    public void GoForwardNewUiState()
    {
        if (initializeDojoEntitiesScript.burnerManager.CurrentBurner != null)
        {
            uiStateManager.SetUiState(1);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartGame();
        }
    }

    public async void StartGame()
    {
        var createGameProps = new DojoCallsManager.CreateGameStruct
        {
            startBlock = 209,
            preparationBlock = 10
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "create",
            addressOfSystem = DojoEntitiesDataManager.worldManager.chainConfig.gameActionsAddress,
            account = DojoEntitiesDataManager.currentAccount,
        };

        var transaction = await DojoCallsManager.CreateGameDojoCall(createGameProps, endpoint);
    }
}
