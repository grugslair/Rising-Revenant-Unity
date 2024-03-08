using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using Dojo.Starknet;
using Vector2 = UnityEngine.Vector2;
using Random = System.Random;
using SimpleGraphQL;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public static class RisingRevenantUtils
{

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

    public static int GeneralHexToInt(string hexString)
    {
        if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            hexString = hexString.Substring(2);
        }

        int intValue = Convert.ToInt32(hexString, 16);

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
}
