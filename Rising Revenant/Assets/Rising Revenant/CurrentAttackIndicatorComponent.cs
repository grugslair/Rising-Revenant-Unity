
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentAttackIndicatorComponent : MonoBehaviour
{

    [SerializeField]
    private struct PopUpDataText
    {
        public string attackName;
        public string attackDescription;
        public string reinforcementName;
    }

    [SerializeField]
    private List<RawImage> eventIcons = new List<RawImage>();

    [SerializeField]
    private TMP_Text attackName;

    [SerializeField]
    private List<Texture2D> eventBackgroundImages = new List<Texture2D>();
    [SerializeField]
    private List<Texture2D> eventReinforcementsImages = new List<Texture2D>();


    [SerializeField]
    private GameObject tooltipPrefab;

    private void Awake()
    {
        UiEntitiesReferenceManager.currentAttackIndicatorComponent = this;
    }

    [SerializeField]
    private List<PopUpDataText> popUpData = new List<PopUpDataText>() { 
    
        new PopUpDataText
        {
            attackName = "EARTHQUAKE",
            attackDescription = "Earthquakes threaten outposts, but obsidian, robut and resiliest, guards against seismic shocks. Its unmathced strength ensures your stronghold remains unscathed, preserving stability amidst turmoil",
            reinforcementName = "OBSIDIAN",
        },
        
        new PopUpDataText
        {
            attackName = "GOBLIN ATTACK",
            attackDescription = "Goblin hordes aim for outpost, seeking chaos, but startegic trenches stand as your safeguard. These defenses are key to protecting your domain from their disruptive incursion, ensuring your outpost's security",
            reinforcementName = "TRENCHES",
        },
    
        new PopUpDataText
        {
            attackName = "DRAGON ATTACK",
            attackDescription = "Dragons descend, targeting outposts with fiery might. Stone walls stand as the ultimate defense. These barriers repel the dragon's fire, safeguarding your domain from destruction",
            reinforcementName = "STONE WALL",
        }
    };

    void Start()
    {
        for (int i = 0; i < eventIcons.Count; i++)
        {
            var comp = eventIcons[i].GetComponent<TooltipAsker>();
            comp.tooltipPrefab = tooltipPrefab;
        }
    }

    
    public void SetEventType(RisingRevenantUtils.EventType eventType)
    {
        SetIndicator(eventType);
    }

    private void SetIndicator(RisingRevenantUtils.EventType eventType)
    {
        for (int i = 0; i < eventIcons.Count; i++)
        {
            var comp = eventIcons[i].GetComponent<TooltipAsker>();

            if (i == (int)eventType)
            {
                eventIcons[i].color = new Color(0.9f, 0, 0, 1);
                var prefabComp = comp.tooltipPrefab.GetComponent<AttackPopUpElement>();

                prefabComp.SetData(popUpData[i].attackName, popUpData[i].attackDescription, popUpData[i].reinforcementName, eventBackgroundImages[i], eventReinforcementsImages[i]);
                comp.show = true;
                this.attackName.text = $"Current Attack: {popUpData[i].attackName}";
            }
            else
            {   
                comp.show = false;
                eventIcons[i].color = new Color(1,1,1, 0.5f);
            }
        }
    }
}
