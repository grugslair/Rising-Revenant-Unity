using Dojo;
using Dojo.Starknet;
using dojo_bindings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public static class DojoCallsManager
{
    #region structs structure for calls
    public enum WalletType
    {
        BURNER,
        ARGENT_X,
        BRAAVOS
    }

    public static WalletType selectedWalletType { get; set; }

    public struct EndpointDojoCallStruct
    {
        public Account account;
        public string functionName;
        public string addressOfSystem;
        public EnvironmentType environmentType;
        public WalletType walletType;
    }

    public struct CreateGameStruct
    {
        public UInt64 startBlock;
        public UInt64 preparationBlock;
    }


    public struct ClaimEndgameRewardsStruct
    {
        public FieldElement gameId;
    }
    public struct ClaimContribEngameRewardsStruct
    {
        public FieldElement gameId;
    }

    public struct SummonRevenantStruct
    {
        public FieldElement gameId;
        public UInt32 count;
    }
    public struct GetOutpostPrice
    {
        public FieldElement gameId;
    }
    public struct ReinforceOutpostStruct
    {
        public FieldElement gameId;
        public RisingRevenantUtils.Vec2 outpostId;
        public UInt32 count;
    }
    public struct DamageOutpostStruct
    {
        public FieldElement gameId;
        public RisingRevenantUtils.Vec2 outpostId;
    }
    public struct SetReinforcementType
    {
        public FieldElement gameId;
        public RisingRevenantUtils.Vec2 outpostId;
        public RisingRevenantUtils.ReinforcementType type;
    }


    public struct GetReinforcementPrice
    {
        public FieldElement gameId;
        public UInt32 count;
    }
    public struct PurchaseReinforcementsStruct
    {
        public FieldElement gameId;
        public UInt32 count;
    }


    public struct CreateTradeRevenantStruct
    {
        public FieldElement gameId;
        public FieldElement priceRevenant;
        public RisingRevenantUtils.Vec2 revenantId;
    }
    public struct PurchaseTradeRevenantStruct
    {
        public FieldElement gameId;
        public FieldElement tradeId;
    }
    public struct ModifyTradeRevenantStruct
    {
        public FieldElement gameId;
        public FieldElement tradeId;
        public FieldElement priceRevenant;
    }
    public struct RevokeTradeRevenantStruct
    {
        public FieldElement gameId;
        public FieldElement tradeId;
    }


    public struct CreateTradeReinforcementStruct
    {
        public FieldElement gameId;
        public FieldElement priceReinforcement;
        public UInt32 count;
    }
    public struct PurchaseTradeReinforcementStruct
    {
        public FieldElement gameId;
        public FieldElement tradeId;
    }
    public struct ModifyTradeReinforcementStruct
    {
        public FieldElement gameId;
        public FieldElement tradeId;
        public FieldElement priceReinforcemnt;
    }
    public struct RevokeTradeReinforcementStruct
    {
        public FieldElement gameId;
        public FieldElement tradeId;
    }


    public struct CreateEventStruct
    {
        public FieldElement gameId;
    }


    #endregion

    #region dojo calls
    public static async Task<FieldElement> CreateGameDojoCall(CreateGameStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var transaction =  await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
                new dojo.Call
                {
                    calldata = new dojo.FieldElement[]
                    {
                        new FieldElement(dataStruct.startBlock.ToString("X")).Inner(),
                        new FieldElement(dataStruct.preparationBlock.ToString("X")).Inner(),
                    },
                    selector = endpointData.functionName,
                    to = endpointData.addressOfSystem,
                }
            });

            UiEntitiesReferenceManager.notificationManager.CreateNotification("Created a game", null, 5f);

            return transaction;

        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Couldn't create a game", null, 5f);
            Debug.Log("issue with the call to the chain " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> ClaimEndgameRewardsDojoCall(ClaimEndgameRewardsStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Endgame rewards claimed successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to claim endgame rewards", null, 5f);
            Debug.Log("Error in ClaimEndgameRewardsDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> ClaimContribEndgameRewardsDojoCall(ClaimContribEngameRewardsStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Contributor endgame rewards claimed successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to claim contributor endgame rewards", null, 5f);
            Debug.Log("Error in ClaimContribEndgameRewardsDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> ReinforceOutpostDojoCall(ReinforceOutpostStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    new FieldElement(dataStruct.outpostId.x.ToString("X")).Inner(),
                    new FieldElement(dataStruct.outpostId.y.ToString("X")).Inner(),
                    new FieldElement(dataStruct.count.ToString("X")).Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Outpost reinforced successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to reinforce outpost", null, 5f);
            Debug.Log("Error in ReinforceOutpostDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> SetReinforcementTypeCall(SetReinforcementType dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    new FieldElement(dataStruct.outpostId.x.ToString("X")).Inner(),
                    new FieldElement(dataStruct.outpostId.y.ToString("X")).Inner(),
                    new FieldElement(dataStruct.type).Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Reinforcement type set successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to set reinforcement type", null, 5f);
            Debug.Log("Error in SetReinforcementTypeCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> SummonRevenantsDojoCall(SummonRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            List<dojo.Call> calls = new List<dojo.Call>();

            for (int i = 0; i < dataStruct.count; i++)
            {
                calls.Add(new dojo.Call
                {
                    calldata = new dojo.FieldElement[]
                    {
                    dataStruct.gameId.Inner(),
                    },
                    selector = endpointData.functionName,
                    to = endpointData.addressOfSystem,
                });
            }

            UiEntitiesReferenceManager.notificationManager.CreateNotification($"Succesfully summoned {dataStruct.count} Revenants", null, 5f);
            return await endpointData.account.ExecuteRaw(calls.ToArray());
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to purchase revenants", null, 5f);
            Debug.Log("Error in PurchaseRevenantssDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> PurchaseReinforcementsDojoCall(PurchaseReinforcementsStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    new FieldElement(dataStruct.count.ToString("X")).Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Reinforcements purchased successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to purchase reinforcements", null, 5f);
            Debug.Log("Error in PurchaseReinforcementsDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> CreateTradeRevenantDojoCall(CreateTradeRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    dataStruct.priceRevenant.Inner(),
                    new FieldElement(dataStruct.revenantId.x.ToString("X")).Inner(),
                    new FieldElement(dataStruct.revenantId.y.ToString("X")).Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Trade revenant created successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to create trade revenant", null, 5f);
            Debug.Log("Error in CreateTradeRevenantDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> ModifyTradeRevenantDojoCall(ModifyTradeRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    dataStruct.tradeId.Inner(),
                    dataStruct.priceRevenant.Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Trade revenant modified successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to modify trade revenant", null, 5f);
            Debug.Log("Error in ModifyTradeRevenantDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> RevokeTradeRevenantDojoCall(RevokeTradeRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    dataStruct.tradeId.Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Trade revenant revoked successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to revoke trade revenant", null, 5f);
            Debug.Log("Error in RevokeTradeRevenantDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> PurchaseTradeRevenantDojoCall(PurchaseTradeRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    dataStruct.tradeId.Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Trade revenant purchased successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to purchase trade revenant", null, 5f);
            Debug.Log("Error in PurchaseTradeRevenantDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> CreateTradeReinforcementDojoCall(CreateTradeReinforcementStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    dataStruct.priceReinforcement.Inner(),
                    new FieldElement(dataStruct.count.ToString("X")).Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
        });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Trade reinforcement created successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to create trade reinforcement", null, 5f);
            Debug.Log("Error in CreateTradeReinforcementDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> ModifyTradeReinforcementDojoCall(ModifyTradeReinforcementStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    dataStruct.tradeId.Inner(),
                    dataStruct.priceReinforcemnt.Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Trade reinforcement modified successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to modify trade reinforcement", null, 5f);
            Debug.Log("Error in ModifyTradeReinforcementDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> RevokeTradeReinforcementDojoCall(RevokeTradeReinforcementStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    dataStruct.tradeId.Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Trade reinforcement revoked successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to revoke trade reinforcement", null, 5f);
            Debug.Log("Error in RevokeTradeReinforcementDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> PurchaseTradeReinforcementDojoCall(PurchaseTradeReinforcementStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    dataStruct.tradeId.Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Trade reinforcement purchased successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to purchase trade reinforcement", null, 5f);
            Debug.Log("Error in PurchaseTradeReinforcementDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> CreateEventDojoCall(CreateEventStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Event created successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to create event", null, 5f);
            Debug.Log("Error in CreateEventDojoCall: " + ex.Message);
            return null;
        }
    }

    public static async Task<FieldElement> DamageOutpostDojoCall(DamageOutpostStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        try
        {
            var result = await endpointData.account.ExecuteRaw(new dojo.Call[]
            {
            new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                    dataStruct.gameId.Inner(),
                    new FieldElement(dataStruct.outpostId.x.ToString("X")).Inner(),
                    new FieldElement(dataStruct.outpostId.y.ToString("X")).Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            }
            });
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Outpost damaged successfully", null, 5f);
            return result;
        }
        catch (Exception ex)
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Failed to damage outpost", null, 5f);
            Debug.Log("Error in DamageOutpostDojoCall: " + ex.Message);
            return null;
        }
    }


    #endregion
}
