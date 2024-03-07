using Dojo.Starknet;
using SimpleGraphQL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

//HERE this should be started with no data as right now its pre populated

public class RevenantJurnalPageBehaviour : Menu
{
    public TMP_Text eventNumText;
    public TMP_Text eventDataTypeText;
    public TMP_Text eventDataCoordText;
    public TMP_Text eventDataRadiusText;

    public GameObject parentList;
    public GameObject parentListPrefab;

    private WorldEventStruct currentWorldEvent;

    private int currentEventNumber;
    private UInt32 maxNumOfEvents;

    [SerializeField]
    private AudioClip openingPageSFX;

    public struct WorldEventStruct
    {
        public FieldElement gameId;
        public UInt32 radius;
        public Vector2 position;
        public UInt32 number;
    }

    string worldEventQueryStructure = @"
     query {
        worldEventModels (
          where : {number : EVENT_NUMBER, game_id: ""GAME_ID""}
        ) {
          edges {
            node {
              entity {
                keys
                models {
                  __typename
                  ... on WorldEvent {
                    game_id
                    radius
                    position{
                      x
                      y
                    }
                    number
                  }
                }
              }
            }
          }
        }
      }";

    private void Awake()
    {
        UiEntitiesReferenceManager.revJournalPageBehaviour = this;
    }

    void OnEnable()
    {
        SoundEffectManager.Instance.PlaySoundEffect(openingPageSFX,true);

        if (DojoEntitiesDataManager.currentWorldEvent == null) { return; }

        var currentWorldEvent = new WorldEventStruct
        {
            radius = DojoEntitiesDataManager.currentWorldEvent.radius,
            position = new Vector2(DojoEntitiesDataManager.currentWorldEvent.position.x, DojoEntitiesDataManager.currentWorldEvent.position.y),
            number = DojoEntitiesDataManager.currentWorldEvent.number
        };

        maxNumOfEvents = DojoEntitiesDataManager.currentWorldEvent.number;
        currentEventNumber = (int)maxNumOfEvents;
        LoadEventData(currentWorldEvent);
    }

    void OnDisable()
    {

    }

    public async void ChangeEvent(int direction)
    {
        if (DojoEntitiesDataManager.currentWorldEvent == null) { return; }

        int newEventNumber = currentEventNumber + direction;

        if (newEventNumber < 1 || newEventNumber > maxNumOfEvents) { return; }

        currentEventNumber = newEventNumber;

        if (newEventNumber == maxNumOfEvents) {

            var currentWorldEvent = new WorldEventStruct
            {
                radius = DojoEntitiesDataManager.currentWorldEvent.radius,
                position = new Vector2(DojoEntitiesDataManager.currentWorldEvent.position.x, DojoEntitiesDataManager.currentWorldEvent.position.y),
                number = DojoEntitiesDataManager.currentWorldEvent.number
            };
            LoadEventData(currentWorldEvent);
            return;
        }

        await GetEventDataFromDojo(RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId).ToString(), (newEventNumber).ToString());
        LoadEventData(currentWorldEvent);
    }

    public async Task<int> GetEventDataFromDojo(string gameId, string number)
    {
        var dict = new Dictionary<string, string>();

        dict.Add("GAME_ID", gameId);
        dict.Add("EVENT_NUMBER", number);

        var query = RisingRevenantUtils.ReplaceWords(worldEventQueryStructure, dict);

        var client = new GraphQLClient(DojoCallsManager.graphlQLEndpoint);
        var tradesRequest = new Request
        {
            Query = query,
        };

        var responseType = new
        {
            worldEventModels = new
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
                                        game_id = "",
                                        position = new {
                                          x= "",
                                          y= "",
                                        },
                                        __typename= "",
                                        radius= "",
                                        number = "",
                                    }
                                }
                            }
                        }
                    }
                },
            }
        };

        try
        {
            var response = await client.Send(() => responseType, tradesRequest);

            if (response.Data.worldEventModels.edges.Length == 0)
            {
                return 0;
            }

            if (response.Data != null && response.Data.worldEventModels != null)
            {
                currentWorldEvent = new WorldEventStruct
                {
                    radius = UInt32.Parse(response.Data.worldEventModels.edges[0].node.entity.models[0].radius),
                    position = new Vector2(float.Parse(response.Data.worldEventModels.edges[0].node.entity.models[0].position.x), float.Parse(response.Data.worldEventModels.edges[0].node.entity.models[0].position.y)),
                    number = UInt32.Parse(response.Data.worldEventModels.edges[0].node.entity.models[0].number)
                };

                return 1;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Query failed for status: {ex.Message}");
        }

        return 0;
    }

    public void LoadEventData(WorldEventStruct worldEvent)
    {
        currentWorldEvent = worldEvent;

        eventNumText.text = $"Event {currentEventNumber}/{maxNumOfEvents}";

        eventDataRadiusText.text = $"Radius\n{currentWorldEvent.radius}km";
        eventDataCoordText.text = $"Position\nX:{currentWorldEvent.position.x} || Y:{currentWorldEvent.position.y}";
        eventDataTypeText.text = $"Type\nNull";

        //kill all children
        foreach (Transform child in parentList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in DojoEntitiesDataManager.outpostDictInstance)
        {
            var outpost = item.Value;

            if (RisingRevenantUtils.IsPointInsideCircle(new Vector2(currentWorldEvent.position.x, currentWorldEvent.position.y), currentWorldEvent.radius, new Vector2(outpost.position.x, outpost.position.y)))
            {
                GameObject newItem = Instantiate(parentListPrefab);
                newItem.transform.SetParent(parentList.transform, false);
                var comp = newItem.GetComponent<RevenantJurnalPageListElement>();

                comp.OutpostCoordsText.text = $"X:{outpost.position.x}\nY:{outpost.position.y}";
                comp.OutpostNameText.text = $"Owner: {RisingRevenantUtils.GetFullRevenantName(outpost.position)}";
                comp.OutpostIdText.text = $"Outpost Id: {-1}";
            }
        }
    }
}
