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

    public TextMeshProUGUI loginButtonText;
    public Button loginButton;

    private void Start()
    {
        if (initializeDojoEntitiesScript.burnerManager.CurrentBurner == null)
        {
            loginButton.onClick.RemoveAllListeners();
            loginButton.onClick.AddListener(CreateBurner);

            loginButtonText.text = "Create Wallet";
        }
        else
        {
            loginButton.onClick.RemoveAllListeners();
            loginButton.onClick.AddListener(GoForward);

            loginButtonText.text = "Login as: " + initializeDojoEntitiesScript.burnerManager.CurrentBurner.Address.Hex().Substring(0, 8);
        }
    }

    public void CreateBurner()
    {
        //var burner = await initializeDojoEntitiesScript.burnerManager.DeployBurner();
        //loginButtonText.text = "Login as: " + burner.Address.Hex().Substring(0, 8);

        //loginButton.onClick.RemoveListener(CreateBurner);

        //loginButton.onClick.AddListener(GoForward);



        //initializeDojoEntitiesScript.SpawnBurner();

        var burner = initializeDojoEntitiesScript.burnerManager.CurrentBurner;
        loginButtonText.text = "Login as: " + burner.Address.Hex().Substring(0, 8);

        loginButton.onClick.RemoveListener(CreateBurner);

        loginButton.onClick.AddListener(GoForward);
    }

    public void JoinAsGuest()
    {

    }

    public void GoForward()
    {
        uiStateManager.SetUiState(1);
    }

    public async void StartGame()
    {

        var createGameProps = new DojoCallsManager.CreateGameStruct
        {
            startBlock = 209,
            preparationBlock = 10
        };

        Debug.Log("wdiojwdiijodwijiwodjawd");

        var acc = initializeDojoEntitiesScript.GenerateAccount();

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "create",
            addressOfSystem = DojoCallsManager.gameActionsAddress,
            account = acc,
        };

        var transaction = await DojoCallsManager.CreateGameDojoCall(createGameProps, endpoint);
    }
}
