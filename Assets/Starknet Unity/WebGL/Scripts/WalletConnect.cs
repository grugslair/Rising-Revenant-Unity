using Dojo.Starknet;
using System;
using System.Collections;
using UnityEngine;

public class WalletConnect : MonoBehaviour
{
    public static string playerAddress;
    public static event Action SuccessfulConnection; 

    private IEnumerator ConnectWalletAsync(Action connectWalletFunction)
    {
        // Call the JavaScript method to connect the wallet
        connectWalletFunction();

        // Wait for the connection to be established
        yield return new WaitUntil(() => JSInteropManager.IsConnected());

        playerAddress = JSInteropManager.GetAccount();
        PlayerPrefs.SetString("playerAddress", playerAddress);

        DojoEntitiesDataManager.currentAccount = new Account(null, null, new FieldElement(playerAddress));
        Debug.Log("Connected to wallet: " + playerAddress);

        // Invoke the SuccessfulConnection event
        SuccessfulConnection?.Invoke();
    }

    public void OnButtonConnectWalletArgentX()
    {
        StartCoroutine(ConnectWalletAsync(JSInteropManager.ConnectWalletArgentX));
    }

    public void OnButtonConnectWalletBraavos()
    {
        StartCoroutine(ConnectWalletAsync(JSInteropManager.ConnectWalletBraavos));
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("playerAddress"))
        {
            playerAddress = PlayerPrefs.GetString("playerAddress");
            Debug.Log("Connected to wallet: " + playerAddress);
        }
        bool available = JSInteropManager.IsWalletAvailable();
        if (!available)
        {
            JSInteropManager.AskToInstallWallet();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
