using Dojo.Starknet;
using dojo_bindings;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

public static class DojoCallsManager
{
    public static string gameActionsAddress = "0x28a0935b0850adb88afe3b3a073a733aa6bc048857946afa7ed9225a291e5c1";
    public static string eventActionsAddress = "0x5b9434baf833e688d3788c808e7870a01b63f0f0b86a6c9b08a4f081b4d44a9";
    public static string revenantActionsAddress = "0x64782561cd66079fc678808c161eae1c5f5332ebd60f601cdcb6fde039ad9db";
    public static string tradeReinfActionsAddress = "0x264f17ee6331a847e465c08d51996b8a50f2ecf0449c0cb94f79ee57c417e79";
    public static string tradeRevsActionsAddress = "0x2d95ca6f9da2ea0497ba0cf084881d24c8bb8d3e21fc7a9c9f24974128d6ffe";

    #region structs structure for calls

    public struct EndpointDojoCallStruct
    {
        public Account account;
        public string functionName;
        public string addressOfSystem;
    }

    public struct CreateGameStruct
    {
        public UInt64 preparationPhaseInterval;
        public UInt64 eventInterval;
        public FieldElement ercAddress;
        public FieldElement rewardPoolAddress;
        public FieldElement revenantInitPrice;
        public UInt32 maxAmountOfRevenants;
        public UInt32 transactionFeePercentage;
        public UInt32 championPrizePercentage;
    }
    public struct ClaimEndgameRewardsStruct
    {
        public UInt32 gameId;
    }
    public struct ClaimContribEngameRewardsStruct
    {
        public UInt32 gameId;
    }

    public struct ReinforceOutpostStruct
    {
        public UInt32 gameId;
        public UInt32 count;
        public FieldElement outpostId;
    }
    public struct CreateRevenantsStruct
    {
        public UInt32 gameId;
        public UInt32 count;
    }
    public struct PurchaseReinforcementsStruct
    {
        public UInt32 gameId;
        public UInt32 count;
    }

    public struct CreateTradeRevenantStruct
    {
        public UInt32 gameId;
        public FieldElement revenantId;
        public FieldElement price;
    }
    public struct ModifyTradeRevenantStruct
    {
        public UInt32 gameId;
        public UInt32 tradeId;
        public FieldElement price;
    }
    public struct RevokeTradeRevenantStruct
    {
        public UInt32 gameId;
        public UInt32 tradeId;
    }
    public struct PurchaseTradeRevenantStruct
    {
        public UInt32 gameId;
        public UInt32 tradeId;
    }

    public struct CreateTradeReinforcementStruct
    {
        public UInt32 gameId;
        public UInt32 count;
        public FieldElement price;
    }
    public struct ModifyTradeReinforcementStruct
    {
        public UInt32 gameId;
        public UInt32 tradeId;
        public FieldElement price;
    }
    public struct RevokeTradeReinforcementStruct
    {
        public UInt32 gameId;
        public UInt32 tradeId;
    }
    public struct PurchaseTradeReinforcementStruct
    {
        public UInt32 gameId;
        public UInt32 tradeId;
    }

    public struct CreateEventStruct
    {
        public UInt32 gameId;
    }
    public struct DamageOutpostStruct
    {
        public UInt32 gameId;
        public FieldElement eventId;
        public FieldElement outpostId;
    }

    #endregion

    #region dojo calls
    public static async Task<FieldElement> CreateGameDojoCall(CreateGameStruct dataStruct, EndpointDojoCallStruct endpointData)
    {

        List<dojo.Call> calls = new List<dojo.Call>();

        for (int i = 0; i < 3; i++)
        {
            calls.Add(new dojo.Call
            {
                calldata = new dojo.FieldElement[] {
                    new FieldElement(dataStruct.preparationPhaseInterval).Inner(),
                    new FieldElement(dataStruct.eventInterval).Inner(),
                    dataStruct.ercAddress.Inner(),
                    dataStruct.rewardPoolAddress.Inner(),
                    dataStruct.revenantInitPrice.Inner(),
                    new FieldElement(dataStruct.maxAmountOfRevenants).Inner(),
                    new FieldElement(dataStruct.transactionFeePercentage).Inner(),
                    new FieldElement(dataStruct.championPrizePercentage).Inner(),

                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            });
        }

        return await endpointData.account.ExecuteRaw(calls.ToArray());
    }

    public static async Task<FieldElement> ClaimEndgameRewardsDojoCall(ClaimEndgameRewardsStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> ClaimContribEndgameRewardsDojoCall(ClaimContribEngameRewardsStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> ReinforceOutpostDojoCall(ReinforceOutpostStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.count).Inner(),
                dataStruct.outpostId.Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> CreateRevenantsDojoCall(CreateRevenantsStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.count).Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }


    public static async Task<FieldElement> CreateRevenantsDojoMultiCall(CreateRevenantsStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        List<dojo.Call> calls = new List<dojo.Call>();

        for (int i = 0; i < dataStruct.count; i++)
        {
            calls.Add(new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.count).Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
            });
        }

        return await endpointData.account.ExecuteRaw(calls.ToArray());
    }

    public static async Task<FieldElement> PurchaseReinforcementsDojoCall(PurchaseReinforcementsStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.count).Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> CreateTradeRevenantDojoCall(CreateTradeRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                dataStruct.revenantId.Inner(),
                dataStruct.price.Inner(),
            },
              selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> ModifyTradeRevenantDojoCall(ModifyTradeRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.tradeId).Inner(),
                dataStruct.price.Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> RevokeTradeRevenantDojoCall(RevokeTradeRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.tradeId).Inner(),
            },
            selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> PurchaseTradeRevenantDojoCall(PurchaseTradeRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.tradeId).Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> CreateTradeReinforcementDojoCall(CreateTradeReinforcementStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.count).Inner(),
                dataStruct.price.Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> ModifyTradeReinforcementDojoCall(ModifyTradeReinforcementStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.tradeId).Inner(),
                dataStruct.price.Inner(),
            },
              selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> RevokeTradeReinforcementDojoCall(RevokeTradeReinforcementStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.tradeId).Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> PurchaseTradeReinforcementDojoCall(PurchaseTradeReinforcementStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                new FieldElement(dataStruct.tradeId).Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> CreateEventDojoCall(CreateEventStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    public static async Task<FieldElement> DamageOutpostDojoCall(DamageOutpostStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
            calldata = new dojo.FieldElement[]
            {
                new FieldElement(dataStruct.gameId).Inner(),
                dataStruct.eventId.Inner(),
                dataStruct.outpostId.Inner(),
            },
               selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }

    #endregion

   
}
