
using System;
using TMPro;
using UnityEngine;

public class TradePageBehaviour : Menu
{
  
    public TradeOutpostPageBehaviour tradeOutpostPageBehaviour { get; set; }
    public TradeReinforcementPageBehaviour reinforcementPageBehaviour { get; set; }

    public SellOutpostPageBehaviour sellOutpostPageBehaviour { get; set; }
    public SellReinforcementPageBehaviour sellReinforcementPageBehaviour { get; set; }

    [SerializeField]
    private GameObject tradesUiParent;
    [SerializeField]
    private GameObject tradesPrefab;


    private void Awake()
    {
        UiEntitiesReferenceManager.tradePageBehaviour = this;
    }
}
