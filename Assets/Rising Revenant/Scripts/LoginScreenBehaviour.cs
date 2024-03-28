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
    [SerializeField] WorldManager worldManager;

    [SerializeField] TMP_Text enviText;

    //public TextMeshProUGUI loginButtonText;
    //public Button loginButton;

    [SerializeField] private List<GameObject> loginButtonObjs;

    private void OnEnable()
    {
        switch (worldManager.chainConfig.environmentType)
        {
            case EnvironmentType.LOCAL:
                enviText.text = "Environment is Local";
                SetCorrectLoginBox(0);
                break;

            case EnvironmentType.TORII:
                enviText.text = "Environment is Torii";
                SetCorrectLoginBox(0);
                break;

            case EnvironmentType.TESTNET:
                enviText.text = "Environment is Testnet";
                SetCorrectLoginBox(1);
                break;
        }
    }

    public void OnSuccesfullConnection()
    {
        Debug.Log("Connected to wallet from login");
        SetCorrectLoginBox(2);
        loginButtonObjs[2].GetComponentInChildren<TextMeshProUGUI>().text = $"Continue with {DojoEntitiesDataManager.currentAccount.Address.Hex().Substring(0,8)}";
        DojoCallsManager.selectedWalletType = DojoCallsManager.WalletType.BRAAVOS;
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
        DojoEntitiesDataManager.currentAccount = new Account(new JsonRpcClient(DojoEntitiesDataManager.worldManager.chainConfig.rpcUrl), new SigningKey(""), new FieldElement(""));
        uiStateManager.SetUiState(1);
    }

    public void CreateBurner()
    {
        if (initializeDojoEntitiesScript.burnerManager.CurrentBurner != null)
        {
            SetCorrectLoginBox(2);
            loginButtonObjs[2].GetComponentInChildren<TextMeshProUGUI>().text = $"Continue with {DojoEntitiesDataManager.currentAccount.Address.Hex().Substring(0, 6)}";

            DojoCallsManager.selectedWalletType = DojoCallsManager.WalletType.BURNER;
        }
    }

    public void GoForwardNewUiState()
    {
        if (DojoEntitiesDataManager.currentAccount != null)
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
            objectName = "Main_Canvas",
            callbackFunctionName = "OnChainTransactionCallbackFunction",
        };

        var transaction = await DojoCallsManager.CreateGameDojoCall(createGameProps, endpoint);
    }
}
