using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class BlockchainInteraction : MonoBehaviour
{
    // Import the JavaScript function
    [DllImport("__Internal")]
    private static extern void FetchCurrentBlockNumber();

    // Method to call the JavaScript function
    public void Start()
    {
        FetchCurrentBlockNumber();
    }

    // Callback method to receive the block number from JavaScript
    public void OnBlockNumberReceived(string blockNumber)
    {
        Debug.Log("Received Block Number: " + blockNumber);
    }
}
