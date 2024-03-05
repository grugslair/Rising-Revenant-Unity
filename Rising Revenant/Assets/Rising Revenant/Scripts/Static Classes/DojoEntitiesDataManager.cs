using Dojo;
using Dojo.Starknet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DojoEntitiesDataManager
{
    public static InitializeDojoEntities dojoEntInitializer { get; set; }
    public static Account currentAccount { get; set; }

    #region Overall game data
    public static FieldElement currentGameId { get; set; }
    public static GamePot gamePot { get; set; }
    public static GameState gameEntCounter { get; set; }
    public static OutpostMarket outpostMarketData { get; set; }
    #endregion


    #region Specific Player Data
    public static PlayerInfo playerInfo { get; set; }
    public static DevWallet currentDevWallet { get; set; }
    public static PlayerContribution playerContrib { get; set; }
    #endregion


    public static Dictionary<RisingRevenantUtils.Vec2, Outpost> outpostDictInstance = new Dictionary<RisingRevenantUtils.Vec2, Outpost>();
    public static List<RisingRevenantUtils.Vec2> ownOutpostIndex = new List<RisingRevenantUtils.Vec2>();


    #region event data region
    public static Dictionary<UInt32, WorldEvent> worldEventDictInstance = new Dictionary<UInt32, WorldEvent>();
    public static CurrentWorldEvent currentWorldEvent { get; set; }
    #endregion
}
