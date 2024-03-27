using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

class ChainStatus : MonoBehaviour
{
    private static readonly HttpClient httpClient = new HttpClient();

    async void Start()
    {
        while (true) // Keeps trying indefinitely. You might want to limit the number of retries.
        {
            bool isBlockchainUp = await IsBlockchainUp("https://starknet-sepolia.public.blastapi.io/rpc/v0_6");
            bool isToriiServiceUp = await IsToriiServiceUp("https://api.cartridge.gg/x/sepoliarr/torii");

            if (isBlockchainUp && isToriiServiceUp)
            {
                Debug.Log("Both services are up, loading Map Scene...");
                //SceneManager.LoadScene("Map Scene");
                break; 
            }
            else
            {
                Debug.Log("One or both services are down. Retrying in 10 seconds...");
                await Task.Delay(10000); 
            }
        }
    }

    static async Task<bool> IsBlockchainUp(string rpcUrl)
    {
        var requestContent = new StringContent("{\"jsonrpc\":\"2.0\",\"method\":\"eth_blockNumber\",\"params\":[],\"id\":1}", Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync(rpcUrl, requestContent);
            if (response.IsSuccessStatusCode)
            {
                Debug.Log("Blockchain is up.");
                return true;
            }
            else
            {
                Debug.LogError("Failed to connect to the blockchain. Status code: " + response.StatusCode);
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("An error occurred while checking the blockchain: " + ex.Message);
            return false;
        }
    }

    static async Task<bool> IsToriiServiceUp(string url)
    {
        try
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent.Contains("\"service\": \"torii\", \"success\": true"))
                {
                    Debug.Log("Torii service is up.");
                    return true;
                }
                else
                {
                    Debug.LogError("Unexpected response from Torii service.");
                    return false;
                }
            }
            else
            {
                Debug.LogError("Failed to connect to Torii service. Status code: " + response.StatusCode);
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("An error occurred while checking the Torii service: " + ex.Message);
            return false;
        }
    }
}

