using Dojo.Starknet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DojoEntitiesDataManager
{

    public static InitializeDojoEntities dojoEntInitializer { get; set; }

    public static Account currentAccount { get; set; }
    public static GameEntityCounter gameEntityCounterInstance { get; set; }
    public static Game gameDataInstance { get; set; }
    public static GameCountTracker gameCounterInstance { get; set; }
    public static PlayerInfo playerSpecificData { get; set; }

 
    public static Dictionary<int, Outpost> outpostDictInstance = new Dictionary<int, Outpost>();
    public static Dictionary<int, Revenant> revDictInstance = new Dictionary<int, Revenant>();

    public static List<int> ownOutpostIndex = new List<int>();

    public static Dictionary<int, WorldEvent> worldEventDictInstance = new Dictionary<int, WorldEvent>();

    public static WorldEvent GetLatestEvent()
    {
        if (gameEntityCounterInstance == null)
        {
            return null;
        }

        if (gameEntityCounterInstance.eventCount == 0)
        {
            return null;
        }

        return worldEventDictInstance[(int)gameEntityCounterInstance.eventCount];
    }


    public delegate void WorldEventAddedHandler(WorldEvent worldEvent);
    public static event WorldEventAddedHandler OnWorldEventAdded;

    public static void AddEvent(int eventId, WorldEvent worldEvent)
    {
        Debug.Log("adding " + eventId);
        worldEventDictInstance.Add(eventId, worldEvent);
        OnWorldEventAdded?.Invoke(worldEvent);
    }
}
