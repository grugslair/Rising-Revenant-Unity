using Dojo.Starknet;
using dojo_bindings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class DojoCallsManager
{
    public readonly static string graphlQLEndpoint = "http://localhost:8080/graphql";
    public readonly static string katanaEndpoint = "http://localhost:5050";

    public readonly static string worldAddress = "0x7a4adc3dc01142811d2db99848998828b30e8a3c3d2a3875751f427ff11ad35";

    public readonly static string gameActionsAddress = "0x54017a9219f6483af430c0aa45dd5d6d1fa129d768b01d427636a5121b5f285";
    public readonly static string eventActionsAddress = "0x1f69f004d6bfd4e09b328cff8e84ad9a73d6b2b034741b69ec200d30fc7df97";
    public readonly static string outpostActionsAddress = "0x71977a0f344a88cd338279dc01795428ce9872062f353eea3cc2d5ade58cf40";
    public readonly static string reinforcementActionsAddress = "0x64d6e5d52b28941226a0a4148a6a26f8fd4271a5150b2cfe49ab6f64788bff2";
    public readonly static string paymentActionsAddress = "0x1cbf7df5c43cefe7d497b5e8985e566c0ef910f600a1ea0ebb2efcd34577d99";
    public readonly static string tradeOutpostActionsAddress = "0x76a4dc474308a5ea36e3b089940269d90a6b4c6dcb98acb6f6018a177e1a827";
    public readonly static string tradeReinforcementActionsAddress = "0x696cde8aa85d7e3191eab7396640f11a002c68dc11fe6cb6dc94471bd35ff90";

    public readonly static string masterAddress = "0xb3ff441a68610b30fd5e2abbf3a1548eb6ba6f3559f2862bf2dc757e5828ca";
    public readonly static string masterPrivateKey = "0x2bbf4f9fd0bbb2e60b0316c1fe0b76cf7a4d0198bd493ced9b8df2a3a24d68a";

    //public static string graphlQLEndpoint = "https://api.cartridge.gg/x/rr/torii/graphql";
    //public static string katanaEndpoint = "https://api.cartridge.gg/x/rr/katana";

    //public static string worldAddress = "0x739075d3eab7b1463ef8e99ad59afc470dbee7a4d5682fecde6c84c0798e1e7";

    //public static string gameActionsAddress = "0x2bbd7c9d7822223e43460fe0c0c8089cab0f4e5c6b960ae7ec38a9ef8e560b2";
    //public static string eventActionsAddress = "0x223e3880bb801613e3ad10efc0e3b54f83e9d9a64100c62148dad450178e272";
    //public static string outpostActionsAddress = "0x1ce589283a7353cad2c51f9c7ef2407962ab356cfb32f909ae0dfba315f342b";
    //public static string reinforcementActionsAddress = "0x7c68de0e3b99ffc19673104c582653b786d16eb18bd721716dcf2bfe5785d2f";
    //public static string paymentActionsAddress = "0x7cfca1aa08b1ef734b532c8b16d4e267c41451b9a4b7ba9c5e557400324a102";
    //public static string tradeOutpostActionsAddress = "0x4b595653ed5a8e53fafb6c45904eea086ce57eed8082970572a9b5163a4f9a5";
    //public static string tradeReinforcementActionsAddress = "0x677359fd0f14a9d4af8cac6e29354a1a0380bde387241deabd27560bf475d95";

    //public static string masterAddress = "0x3ebe00c0bce66b6d4bb20726812bff83fbb527226babcaf3d4dac46915cedb";
    //public static string masterPrivateKey = "0x1d2ce4b504f4dcf9061d4db9e10e9d5d14f37b4ec595a648d6cd6e005ef937e";


    //here should go an enum for all the functions to call


    #region structs structure for calls

    public struct EndpointDojoCallStruct
    {
        public Account account;
        public string functionName;
        public string addressOfSystem;
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
