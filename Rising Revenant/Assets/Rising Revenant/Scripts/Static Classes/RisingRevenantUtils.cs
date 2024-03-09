using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using Dojo.Starknet;
using Vector2 = UnityEngine.Vector2;
using Random = System.Random;
using SimpleGraphQL;
using System.Threading.Tasks;
using System.Drawing;

public static class RisingRevenantUtils
{
    [Serializable]
    public enum EventType
    {
        None,
        Dragon,
        Goblin,
        Earthquake,
    }

    [Serializable]
    public enum ReinforcementType
    {
        None,
        Wall,
        Trench,
        Bunker,
    }

    [Serializable]
    public struct U256
    {
        public BigInteger high;
        public BigInteger low;
    }

    [Serializable]
    public struct Vec2
    {
        public UInt32 x;
        public UInt32 y;
    }

    public static float MAP_WIDHT = 10240;
    public static float MAP_HEIGHT = 5124;
    public static float SCALE = 2;

    public static string[] NamesArray = new string[] {
        "Mireth", "Vexara", "Zorion", "Caelix",
        "Elyndor", "Tharion", "Sylphren", "Aravax",
        "Vexil", "Lyrandar", "Nyxen", "Theralis",
        "Qyra", "Fenrix", "Atheris", "Lorvael",
        "Xyris", "Zephyron", "Calaer", "Drakos",
        "Velixar", "Syrana", "Morvran", "Elithran",
        "Kaelith", "Tyrven", "Ysara", "Vorenth",
        "Alarix", "Ethrios", "Nyrax", "Thrayce",
        "Vynora", "Kerith", "Jorvax", "Lysandor",
        "Eremon", "Xanthe", "Zanther", "Cindris",
        "Baelor", "Lyvar", "Eryth", "Zalvor",
        "Gormath", "Sylvanix", "Quorin", "Taryx",
        "Nyvar", "Oryth", "Valeran", "Myrthil",
        "Zorvath", "Kyrand", "Thalren", "Vexim",
        "Aelar", "Grendar", "Xylar", "Zorael",
        "Calyph", "Vyrak", "Thandor", "Lyrax",
        "Riven", "Drexel", "Yvaris", "Zenthil",
        "Aravorn", "Morthil", "Sylvar", "Quinix",
        "Tharix", "Valthorn", "Nythar", "Lorvax",
        "Exar", "Zilthar", "Cynthis", "Veldor",
        "Arix", "Thyras", "Mordran", "Elyx",
        "Kythor", "Rendal", "Xanor", "Yrthil",
        "Zarvix", "Caelum", "Lythor", "Qyron",
        "Thoran", "Vexor", "Nyxil", "Orith",
        "Valix", "Myrand", "Zorath", "Kaelor"
    };
    public static string[] SurnamesArray = new string[] {
        "Velindor", "Tharaxis", "Sylphara", "Aelvorn",
        "Morvath", "Elynara", "Xyreth", "Zephris",
        "Kaelyth", "Nyraen", "Lorvex", "Quorinax",
        "Dravys", "Aeryth", "Thundris", "Gryfora",
        "Luminaer", "Orythus", "Veximyr", "Zanthyr",
        "Caelarix", "Nythara", "Vaelorix", "Myrendar",
        "Zorvyn", "Ethrios", "Mordraen", "Xanthara",
        "Yrthalis", "Zarvixan", "Calarun", "Vyrakar",
        "Thandoril", "Lyraxin", "Drexis", "Yvarix",
        "Zenithar", "Aravor", "Morthal", "Sylvoran",
        "Quinixar", "Tharixan", "Valthornus", "Nytharion",
        "Lorvax", "Exarion", "Ziltharix", "Cynthara",
        "Veldoran", "Arxian", "Thyras", "Elyxis",
        "Kythoran", "Rendalar", "Xanorath", "Yrthilix",
        "Zarvixar", "Caelumeth", "Lythorix", "Qyronar",
        "Thoranis", "Vexorath", "Nyxilar", "Orithan",
        "Valixor", "Myrandar", "Zorathel", "Kaeloran",
        "Skyrindar", "Nighsearix", "Flamveilar", "Thornvalix",
        "Stormwieldor", "Emberwindar", "Ironwhisparia", "Ravenfrostix",
        "Shadowgleamar", "Frostechoar", "Moonriftar", "Starbinderix",
        "Voidshaperix", "Earthmeldar", "Sunweaverix", "Seablazix",
        "Wraithbloomar", "Windshardix", "Lightchasar", "Darkwhirlar",
        "Thornspiritix", "Stormglowar", "Firegazix", "Nightstreamar",
        "Duskwingar", "Frostrealmar", "Shadowsparkix", "Ironbloomar",
        "Ravenmistar", "Embermarkix", "Gloomveinar", "Moonshroudar"
    };

    public enum TradesStatus
    {
        NOT_CREATED = 0,
        SELLING,
        SOLD,
        REVOKED
    }

    public static int GetConsistentRandomNumber(int input, int seed, int min, int max)
    {
        int combinedSeed = seed + input;
        Random random = new Random(combinedSeed);
        return random.Next(min, max + 1);
    }

    public static int FieldElementToInt(FieldElement value) 
    {
        string hexString = value.Hex(); 
        hexString = hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? hexString[2..] : hexString;
        BigInteger number = BigInteger.Parse(hexString, System.Globalization.NumberStyles.HexNumber);

        return (int)number;
    }

    public static bool IsPointInsideCircle(Vector2 circleCenter, float radius, Vector2 point)
    {
        float distance = Vector2.Distance(circleCenter, point);
        return distance <= radius;
    }

    public static string GetFullRevenantName(RisingRevenantUtils.Vec2 id) {
        var randomNum = DojoEntitiesDataManager.outpostDictInstance[id].position.x * DojoEntitiesDataManager.outpostDictInstance[id].position.y;

        return $"{NamesArray[GetConsistentRandomNumber((int)randomNum, (int)DojoEntitiesDataManager.outpostDictInstance[id].position.x, 0,49)] } {SurnamesArray[GetConsistentRandomNumber((int)randomNum, (int)DojoEntitiesDataManager.outpostDictInstance[id].position.y, 0, 49)]}";
    }

    public static int GeneralHexToInt(string hexString, int decimalPlaces = -1)
    {
        if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            hexString = hexString.Substring(2);
        }

        int intValue = Convert.ToInt32(hexString, 16);

        if (decimalPlaces >= 0)
        {
            double adjustedValue = intValue / Math.Pow(10, decimalPlaces);
            intValue = (int)Math.Round(adjustedValue);
        }

        return intValue;
    }

    public static string ConvertLargeNumberToString(BigInteger largeNumber, int roundTo = -1)
    {
        decimal scaleFactor = 1e18m; 
        decimal result = (decimal)largeNumber / scaleFactor;

        if (roundTo >= 0)
        {
            result = decimal.Round(result, roundTo, MidpointRounding.AwayFromZero);
        }

        string resultString = result.ToString("G29");

        return resultString;
    }

    /// <summary>
    /// do this when the string is a literal otherwise replace doesnt work
    /// </summary>
    /// <param name="input"></param>
    /// <param name="replacements"></param>
    /// <returns></returns>
    public static string ReplaceWords(string input, Dictionary<string, string> replacements)
    {
        foreach (var pair in replacements)
        {
            input = input.Replace(pair.Key, pair.Value);
        }
        return input;
    }

    public static int CalculateShields(uint lifes)
    {
        if (lifes >= 1 && lifes <= 2) return 0; 
        if (lifes >= 3 && lifes <= 5) return 1;
        if (lifes >= 6 && lifes <= 9) return 2; 
        if (lifes >= 10 && lifes <= 13) return 3; 
        if (lifes >= 14 && lifes <= 19) return 4; 
        if (lifes >= 20) return 5; 
        return 0;
    }

    public static long CantonPair(int x, int y)
    {
        return ((long)x + y) * ((long)x + y + 1) / 2 + y;
    }

    public static (int, int) CantonUnpair(long z)
    {
        long w = (long)Math.Floor((Math.Sqrt(8 * z + 1) - 1) / 2);
        long t = (w * w + w) / 2;
        long y = z - t;
        long x = w - y;
        return ((int)x, (int)y);
    }

    public async static Task<bool> IsOutpostOnSale(Vec2 outpostId, int gameId)
    {

        return false;

        var dict = new Dictionary<string, string>
        {
            { "GAME_ID", gameId.ToString() }
        };

        string query = @"
        query {
            outpostTradeModels(where: { game_id: ""0""}) {
              edges {
                node {
                  entity {
                    keys
                    models {
                      __typename
                      ... on OutpostTrade {
                        game_id
                        offer {
                          x
                          y
                        }
                        status
                        price
                      }
                    }
                  }
                }
              }
            }
          }";

        //var query = RisingRevenantUtils.ReplaceWords(lastSavedGraphqlQueryStructure, dict);
        //var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);

        var client = new GraphQLClient("https://api.cartridge.gg/x/rr/torii/graphql");
        var tradesRequest = new Request
        {
            Query = query,
        };

        var responseType = new
        {
            outpostTradeModels = new
            {
                edges = new[]
                {
                   new
                   {
                        node = new
                        {
                            entity = new
                            {
                                keys = new[]
                                {
                                   ""
                                },
                                models = new[]
                                {
                                    new
                                    {
                                        __typename = "",
                                        game_id = "",
                                        trade_id = "",
                                        status = "",
                                        offer = new
                                        {
                                            x= "",
                                            y = "",
                                        },
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var response = await client.Send(() => responseType, tradesRequest);

        for (int i = 0; i < response.Data.outpostTradeModels.edges.Length; i++)
        {
            var edge = response.Data.outpostTradeModels.edges[i];

            for (int x = 0; x < edge.node.entity.models.Length; x++)
            {
                if (edge.node.entity.models[x].__typename == "OutpostTrade")
                {
                    //need to loop throuhg them to find if there is one that status is selling
                    if (edge.node.entity.models[x].status == "1")
                    {
                        Debug.Log("there is something selling");
                        return true;
                    }
                }
            }
        }

        Debug.Log("nothing is selling");
        return false;
    }

    public async static Task<bool> IsOutpostEventConfirmed(Vec2 outpostId, int eventId, int gameId)
    {
        return false;

        var dict = new Dictionary<string, string>
        {
            { "GAME_ID", gameId.ToString() },
        };

        string query = @"
        query {
            outpostVerifiedModels(where: { game_id: ""0"", event_id: ""2""}) {
              edges {
                node {
                  entity {
                    keys
                    models {
                      __typename
                      ... on OutpostVerified {
                        game_id
                        verified
                        outpost_id{
                          x
                          y
                        }
                        event_id
                      }
                    }
                  }
                }
              }
            }
          }";

        //var query = RisingRevenantUtils.ReplaceWords(lastSavedGraphqlQueryStructure, dict);

        //var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var client = new GraphQLClient("https://api.cartridge.gg/x/rr/torii/graphql");
        var tradesRequest = new Request
        {
            Query = query,
        };

        var responseType = new
        {
            outpostTradeModels = new
            {
                edges = new[]
                {
                   new
                   {
                        node = new
                        {
                            entity = new
                            {
                                keys = new[]
                                {
                                   ""
                                },
                                models = new[]
                                {
                                    new
                                    {
                                        __typename = "",
                                        game_id = "",
                                        verified = "",
                                        event_id = "",
                                        outpost_id = new
                                        {
                                            x= "",
                                            y = "",
                                        },
                                     }
                                }
                            }
                        }
                    }
                }
            }
        };

        var response = await client.Send(() => responseType, tradesRequest);

        for (int i = 0; i < response.Data.outpostTradeModels.edges.Length; i++)
        {
            var edge = response.Data.outpostTradeModels.edges[i];

            for (int x = 0; x < edge.node.entity.models.Length; x++)
            {
                if (edge.node.entity.models[x].__typename == "OutpostVerified")
                {
                    //if there is one that should mean that it is verified
                    Debug.Log("given id is verified");
                    return true;
                }
            }
        }

        Debug.Log("given id is not verified");
        return false;
    }

    public static double HexToFloat(string hexString, int decimalPlaces = -1)
    {
        if (string.IsNullOrWhiteSpace(hexString))
            throw new ArgumentException("Input cannot be null or empty.", nameof(hexString));

        hexString = hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? hexString[2..] : hexString;

        BigInteger totalValue = BigInteger.Parse("0" + hexString, System.Globalization.NumberStyles.AllowHexSpecifier);
        
        double result = (double)totalValue / Math.Pow(10, 18); 

        return Math.Round(result, 7);
    }






    public static async Task<string> playerContributionInfo(string gameId, string playerId)
    {
        string queryForPlayerContribution = $@"
        query {{
            playerContributionModels(where: {{ game_id: ""{gameId}"", player_id: ""{playerId}""}}) {{
                edges {{
                    node {{
                        entity {{
                            keys
                            models {{
                                __typename
                                ... on PlayerContribution {{
                                    player_id
                                    game_id
                                    score
                                }}
                            }}
                        }}
                    }}
                }}
            }}
        }}";

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var request = new Request
        {
            Query = queryForPlayerContribution,
        };

        var responseType = new
        {
            playerContributionModels = new
            {
                edges = new[]
                {
                new
                {
                    node = new
                    {
                        entity = new
                        {
                            keys = new string[] {},
                            models = new[]
                            {
                                new
                                {
                                    __typename = "",
                                    player_id = "",
                                    game_id = "",
                                    score = ""
                                }
                            }
                        }
                    }
                }
            }
            }
        };

        try
        {
            var response = await client.Send(() => responseType, request);

            if (response.Data != null && response.Data.playerContributionModels != null)
            {
                foreach (var edge in response.Data.playerContributionModels.edges)
                {
                    foreach (var model in edge.node.entity.models)
                    {
                        if (model.__typename == "PlayerContribution")
                        {
                            return model.score;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed for PlayerContribution: {ex.Message}");
        }

        return "0";
    }

    public static async Task<string> outpostMarketInfo(string gameId)
    {
        string queryForOutpostMarket = $@"
        query {{
            outpostMarketModels(where: {{ game_id: ""{gameId}"" }}) {{
                edges {{
                    node {{
                        entity {{
                            keys
                            models {{
                                __typename
                                ... on OutpostMarket {{
                                    game_id
                                    price
                                }}
                            }}
                        }}
                    }}
                }}
            }}
        }}";

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var request = new Request
        {
            Query = queryForOutpostMarket,
        };

        var responseType = new
        {
            outpostMarketModels = new
            {
                edges = new[]
                {
                new
                {
                    node = new
                    {
                        entity = new
                        {
                            keys = new string[] {},
                            models = new[]
                            {
                                new
                                {
                                    __typename = "",
                                    game_id = "",
                                    price = ""
                                }
                            }
                        }
                    }
                }
            }
            }
        };

        try
        {
            var response = await client.Send(() => responseType, request);

            if (response.Data != null && response.Data.outpostMarketModels != null)
            {
                foreach (var edge in response.Data.outpostMarketModels.edges)
                {
                    foreach (var model in edge.node.entity.models)
                    {
                        if (model.__typename == "OutpostMarket")
                        {
                            return model.price;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed for OutpostMarket: {ex.Message}");
        }

        return "0";
    }

    public static async Task<string> gameStateInfo(string gameId)
    {
        string queryForGameState = $@"
        query {{
            gameStateModels(where: {{ game_id: ""{gameId}"" }}) {{
                edges {{
                    node {{
                        entity {{
                            keys
                            models {{
                                __typename
                                ... on GameState {{
                                    game_id
                                    contribution_score_total
                                }}
                            }}
                        }}
                    }}
                }}
            }}
        }}";

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var request = new Request
        {
            Query = queryForGameState,
        };

        var responseType = new
        {
            gameStateModels = new
            {
                edges = new[]
                {
                new
                {
                    node = new
                    {
                        entity = new
                        {
                            keys = new string[] {},
                            models = new[]
                            {
                                new
                                {
                                    __typename = "",
                                    game_id = "",
                                    contribution_score_total = ""
                                }
                            }
                        }
                    }
                }
            }
            }
        };

        try
        {
            var response = await client.Send(() => responseType, request);

            if (response.Data != null && response.Data.gameStateModels != null)
            {
                foreach (var edge in response.Data.gameStateModels.edges)
                {
                    foreach (var model in edge.node.entity.models)
                    {
                        if (model.__typename == "GameState")
                        {
                            return model.contribution_score_total;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed for GameState: {ex.Message}");
        }

        return "0";
    }

    public static async Task<string> devWalletInfo(string gameId, string ownerWallet)
    {
        string queryForDevWallet = $@"
        query {{
            devWalletModels(where: {{ game_id: ""{gameId}"", owner: ""{ownerWallet}""}}) {{
                edges {{
                    node {{
                        entity {{
                            keys
                            models {{
                                __typename
                                ... on DevWallet {{
                                    owner
                                    game_id
                                    balance
                                }}
                            }}
                        }}
                    }}
                }}
            }}
        }}";

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var request = new Request
        {
            Query = queryForDevWallet,
        };

        var responseType = new
        {
            devWalletModels = new
            {
                edges = new[]
                {
                new
                {
                    node = new
                    {
                        entity = new
                        {
                            keys = new string[] {},
                            models = new[]
                            {
                                new
                                {
                                    __typename = "",
                                    owner = "",
                                    game_id = "",
                                    balance = ""
                                }
                            }
                        }
                    }
                }
            }
            }
        };

        try
        {
            var response = await client.Send(() => responseType, request);

            if (response.Data.devWalletModels.edges.Length == 0)
            {
                Debug.Log("No matching dev wallet found");
                return null;
            }

            if (response.Data != null && response.Data.devWalletModels != null)
            {
                foreach (var edge in response.Data.devWalletModels.edges)
                {
                    foreach (var model in edge.node.entity.models)
                    {
                        if (model.__typename == "DevWallet")
                        {
                            return model.balance;
                        }
                    }
                }
            }
            Debug.LogError("Failed to parse data for DevWallet");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed for DevWallet: {ex.Message}");
        }

        return "";
    }


    /// <summary>
    /// total pot -- winners pot -- ltr pot -- dev pot
    /// </summary>
    /// <param name="gameId"></param>
    /// <returns></returns>
    public static async Task<string[]> gamePotInfo(string gameId)
    {
        string queryForGamePot = $@"
        query {{
            gamePotModels(where: {{ game_id: ""{gameId}"" }}) {{
                edges {{
                    node {{
                        entity {{
                            keys
                            models {{
                                __typename
                                ... on GamePot {{
                                    game_id
                                    total_pot
                                    winners_pot
                                    ltr_pot
                                    dev_pot
                                }}
                            }}
                        }}
                    }}
                }}
            }}
        }}";

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var request = new Request
        {
            Query = queryForGamePot,
        };

        var responseType = new
        {
            gamePotModels = new
            {
                edges = new[]
                {
                new
                {
                    node = new
                    {
                        entity = new
                        {
                            keys = new string[] {},
                            models = new[]
                            {
                                new
                                {
                                    __typename = "",
                                    game_id = "",
                                    total_pot = "",
                                    winners_pot = "",
                                    ltr_pot = "",
                                    dev_pot = ""
                                }
                            }
                        }
                    }
                }
            }
            }
        };

        try
        {
            var response = await client.Send(() => responseType, request);

            if (response.Data != null && response.Data.gamePotModels != null)
            {
                foreach (var edge in response.Data.gamePotModels.edges)
                {
                    foreach (var model in edge.node.entity.models)
                    {
                        if (model.__typename == "GamePot")
                        {
                            return new string[] { 
                                model.total_pot, 
                                model.winners_pot, 
                                model.ltr_pot, 
                                model.dev_pot };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed for GamePot: {ex.Message}");
        }

        return new string[] { "", "", "", "" };
    }


    public static async Task<HashSet<Vec2>> GetOutpostVerifiedInfo(string gameId, int number)
    {
        string query = $@"
        query {{
            outpostVerifiedModels(where: {{ game_id: ""{gameId}"", event_id: ""{number}"" }}) {{
                edges {{
                    node {{
                        entity {{
                            keys
                            models {{
                                __typename
                                ... on OutpostVerified {{
                                    game_id
                                    verified
                                    event_id
                                    outpost_id {{
                                        x
                                        y
                                    }}
                                }}
                            }}
                        }}
                    }}
                }}
            }}
        }}";

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var request = new Request
        {
            Query = query,
        };

        var responseType = new
        {
            outpostVerifiedModels = new
            {
                edges = new[]
                {
                new
                {
                    node = new
                    {
                        entity = new
                        {
                            keys = new string[] {},
                            models = new[]
                            {
                                new
                                {
                                    __typename = "",
                                    game_id = "",
                                    verified = "",
                                    event_id = "",
                                    outpost_id = new { x = "", y = "" }
                                }
                            }
                        }
                    }
                }
            }
            }
        };

        var listOfVerifiedOutpost = new HashSet<Vec2>();

        try
        {
            var response = await client.Send(() => responseType, request);

            if (response.Data.outpostVerifiedModels.edges.Length == 0)
            {
                Debug.Log("No verification outpost shits found");
                return null;
            }

            if (response.Data != null && response.Data.outpostVerifiedModels != null)
            {
                foreach (var edge in response.Data.outpostVerifiedModels.edges)
                {
                    foreach (var model in edge.node.entity.models)
                    {
                        if (model.__typename == "OutpostVerified ")
                        {
                            //wtf
                            listOfVerifiedOutpost.Add(new Vec2 { x = (uint)int.Parse(model.outpost_id.x), y = (uint)int.Parse(model.outpost_id.y) });
                        }
                    }
                }
            }
            Debug.LogError("Failed to parse data for OutpostVerified ");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed for OutpostVerified : {ex.Message}");
        }

        return listOfVerifiedOutpost;
    }
}
