using System.Linq;
using UnityEngine;
using Types;
using Utils;
using StarkSharp.Rpc.Utils;

public class GamePlay : MonoBehaviour
{
    private string gameObjectName = "UnityMainThreadDispatcher";
    private string worldAddress = "0x04718f5a0fc34cc1af16a1cdee98ffb20c31f5cd61d6ab07201858f4287c938d";

    public void Entity(string component, Query query, int offset = 0, int length = 0)
    {
        string[] calldata = new string[]{
            StarknetOps.CalculateFunctionSelector(component),
            query.AddressDomain,
            string.Join(",", query.Keys.Select(k => k.ToString())),
            offset.ToString(),
            length.ToString()
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.CallContract(worldAddress, WorldEntryPoints.Get, calldataString, gameObjectName, "Callback");
    }

    public void Entities(string component, int length)
    {
        string[] calldata = new string[]{
            component,
            StarknetOps.CalculateFunctionSelector(length.ToString())
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.CallContract(worldAddress, WorldEntryPoints.Entities, calldataString, gameObjectName, "Callback");
    }

    public void Component(string name)
    {
        string[] calldata = new string[]{
            StarknetOps.CalculateFunctionSelector(name)
        };
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = calldata });
        JSInteropManager.CallContract(worldAddress, WorldEntryPoints.Component, calldataString, gameObjectName, "Callback");
    }

    public void Execute(string[] callData)
    {
        string calldataString = JsonUtility.ToJson(new ArrayWrapper { array = callData });
        JSInteropManager.SendTransactionBraavos(worldAddress, "transfer", calldataString, gameObjectName, "Callback");
    }

    public void Callback(string response)
    {
        JsonResponse jsonResponse = JsonUtility.FromJson<JsonResponse>(response);
        Debug.Log(jsonResponse.result);
    }
}
