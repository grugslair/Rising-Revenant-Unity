using Dojo.Starknet;
using dojo_bindings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class DojoCallsManager
{
    public readonly static string graphlQLEndpoint = "http://localhost:8080/graphql";
    public readonly static string katanaEndpoint = "http://localhost:5050";

    public readonly static string worldAddress = "0x64391fb1874628896a0ab666b40f6b5efd96ab01fb5a823279255e9c17a2e43";

    public readonly static string gameActionsAddress = "0x550d0856811a0eecf61847771c1293344a751dfe0a0422a34dd9147154e71b6";
    public readonly static string eventActionsAddress = "0x29d7c04fba830a9af1ce47b8fac1cb1790c7f3cfc201378adaf3aed60cc45d0";
    public readonly static string outpostActionsAddress = "0x3bdb0f8918378fc3da0b8437a3c043c30c2cabfab0c4a77746ae260d1a8c8bc";
    public readonly static string reinforcementActionsAddress = "0x457287e08259fdbf7e3b3f2e8918b82d2feb522842ab76523975e3ae25a206c";
    public readonly static string paymentActionsAddress = "0x55aee8fcc216662fb6599af9380afeadef8ec16443b40b0a3eb78cb358f4f32";
    public readonly static string tradeOutpostActionsAddress = "0x1316bc6a2b1c0402066e80d524ae3464ee460c6affdab48f5c1373a2cd12387";
    public readonly static string tradeReinforcementActionsAddress = "0x21493ba896ac16eea4150a5ca1e23b1a7ab2f76655ba794429841ba3be8be33";

    public readonly static string masterAddress = "0x6162896d1d7ab204c7ccac6dd5f8e9e7c25ecd5ae4fcb4ad32e57786bb46e03";
    public readonly static string masterPrivateKey = "0x1800000000300000180000000000030000000000003006001800006600";


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

    //public static string masterAddress = "0x923e0b8bdebfcc065807d934013b8c721cf24e6995c9971d577dfea27d4c47";
    //public static string masterPrivateKey = "0x7faebb5bca5519544081123abb38f376063c6f0f8399773e7809927bb84cced";

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
        public FieldElement price;
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
        public FieldElement price;
    }
    public struct RevokeTradeRevenantStruct
    {
        public FieldElement gameId;
        public FieldElement tradeId;
    }


    public struct CreateTradeReinforcementStruct
    {
        public FieldElement gameId;
        public FieldElement price;
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
        public FieldElement price;
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
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
        {
        new dojo.Call
        {
        calldata = new dojo.FieldElement[] {
                    new FieldElement(dataStruct.startBlock.ToString("X")).Inner(),
                    new FieldElement(dataStruct.preparationBlock.ToString("X")).Inner(),
                },
                selector = endpointData.functionName,
                to = endpointData.addressOfSystem,
        }
        });
    }


    public static async Task<FieldElement> ClaimEndgameRewardsDojoCall(ClaimEndgameRewardsStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
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
    }

    public static async Task<FieldElement> ClaimContribEndgameRewardsDojoCall(ClaimContribEngameRewardsStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
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
    }

    public static async Task<FieldElement> ReinforceOutpostDojoCall(ReinforceOutpostStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        return await endpointData.account.ExecuteRaw(new dojo.Call[]
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
    }

    public static async Task<FieldElement> SummonRevenantsDojoCall(SummonRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
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

        return await endpointData.account.ExecuteRaw(calls.ToArray());
    }

    public static async Task<FieldElement> CreateRevenantsDojoMultiCall(SummonRevenantStruct dataStruct, EndpointDojoCallStruct endpointData)
    {
        List<dojo.Call> calls = new List<dojo.Call>();

        for (int i = 0; i < dataStruct.count; i++)
        {
            calls.Add(new dojo.Call
            {
                calldata = new dojo.FieldElement[]
                {
                dataStruct.gameId.Inner(),
                new FieldElement(dataStruct.count.ToString("X")).Inner(),
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
                dataStruct.gameId.Inner(),
                new FieldElement(dataStruct.count.ToString("X")).Inner(),
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
                dataStruct.gameId.Inner(),
                dataStruct.price.Inner(),
                new FieldElement(dataStruct.revenantId.x.ToString("X")).Inner(),
                new FieldElement(dataStruct.revenantId.y.ToString("X")).Inner(),
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
                dataStruct.gameId.Inner(),
                dataStruct.tradeId.Inner(),
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
                dataStruct.gameId.Inner(),
                dataStruct.tradeId.Inner(),
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
                dataStruct.gameId.Inner(),
                dataStruct.tradeId.Inner(),
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
                dataStruct.gameId.Inner(),
                dataStruct.price.Inner(),
                new FieldElement(dataStruct.count.ToString("X")).Inner(),
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
                dataStruct.gameId.Inner(),
                dataStruct.tradeId.Inner(),
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
                dataStruct.gameId.Inner(),
                dataStruct.tradeId.Inner(),
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
                dataStruct.gameId.Inner(),
                dataStruct.tradeId.Inner(),
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
                dataStruct.gameId.Inner(),
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
                dataStruct.gameId.Inner(),
                new FieldElement(dataStruct.outpostId.x.ToString("X")).Inner(),
                new FieldElement(dataStruct.outpostId.y.ToString("X")).Inner(),
            },
            selector = endpointData.functionName,
            to = endpointData.addressOfSystem,
        }
        });
    }

    #endregion
}
