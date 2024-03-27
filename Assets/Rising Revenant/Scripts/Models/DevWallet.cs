using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using SimpleGraphQL;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class DevWallet : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("owner")]
    public FieldElement ownerAddress;

    [ModelField("balance")]
    public RisingRevenantUtils.U256 balance;

    [ModelField("init")]
    public bool init;

    public string balanceString;

    // Start is called before the first frame update
    void Start()
    {
        if (DojoEntitiesDataManager.currentAccount != null)
        {
            var currentAddress = DojoEntitiesDataManager.currentAccount.Address.Hex();

            if (ownerAddress.Hex() == currentAddress)
            {
                DojoEntitiesDataManager.currentDevWallet = this;

                UiEntitiesReferenceManager.topBarUiElement.ChangeInPlayerSpecificData();
            }
            else
            {
                Destroy(this);
            }
        }
    }

    public override void OnUpdate(Model model)
    {
        Debug.Log("\nDevWallet");
        Debug.Log("low: " + balance.low.ToString() + "|| high: " + balance.high.ToString());

        base.OnUpdate(model);
        OnValueChange?.Invoke();

        UiEntitiesReferenceManager.topBarUiElement.ChangeInPlayerSpecificData();
    }

    private async Task<string> devWalletInfo(string gameId, string ownerWallet)
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

        var client = new GraphQLClient(DojoEntitiesDataManager.worldManager.chainConfig.toriiUrl);
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
}
