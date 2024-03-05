public static class UiEntitiesReferenceManager
{


    // PREP PHASE --------------------------
    public static PrepPhaseManager prepPhaseUIManager { get; set; }
    public static BuyReinforcementsPageBehaviour buyReinforcementsPageBehaviour { get; set; }
    public static BuyRevenantPageBehaviour buyRevenantPageBehaviour { get; set; }

    //waiting for ending of the pahse

    //guest wait phase





    // GAME PHASE ----------------------------

    public static GamePhaseManager gamePhaseManager { get; set; }


    //objs
    public static WorldEventManager worldEventManager { get; set; }

    //pages
    public static RevenantJurnalPageBehaviour revJournalPageBehaviour { get; set; }
    public static TradePageBehaviour tradePageBehaviour { get; set; }
    public static EventPageDataContainerBehaviour eventPageDataContainerBehaviour { get; set; }
    public static GameStatsPageBehaviour gameStatsPageBehaviour { get; set; }
    public static WinnerPageBehaviour winnerPageBehaviour { get; set; }

    //comps
    public static MinimapComponentBehaviour minimapComp { get; set; }
    public static RevenantJurnalComponentBehaviour revJournalCompBehaviour { get; set; }
    public static TooltipComponentBehaviour tooltipCompBehaviour { get; set; }
    public static WarningSystemUiComponent warningSystemUiComponent { get; set; }
   





    // BOTH -----------------------------------

    //comps
    public static TopBarUiElement topBarUiElement { get; set; }
    public static TooltipManager tooltipManager { get; set; }
    public static NotificationManager notificationManager { get; set; }
    public static PlayerReinforcementBalanceUiComponents reinforcementCounterElement { get; set; }

    //pages
    public static RulesPageBehaviour rulesPageBehaviour { get; set; }
    public static ProfilePageBehaviour profilePageBehaviour { get; set; }

    //public static settingsManager




}
