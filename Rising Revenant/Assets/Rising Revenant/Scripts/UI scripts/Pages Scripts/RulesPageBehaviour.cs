using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class RulesPageBehaviour : Menu
{
    public RawImage leftArrow;
    public RawImage rightArrow;

    public int selectedMenu = 0;

    public GameObject textPrefab;

  

    private string[] explanationPP = new string[8] {
        "#Summoning the Revenants:",
        "Players begin by summoning Revenants, powerful entities, through a mystical expenditure of $LORDS. Each successful summoning not only brings forth a Revenant but also establishes an Outpost around the game map.",
        "#Building Outposts:",
        "These bastions of power will initially have 1 health. Following a Revenant's summoning, players may fortify these Outposts in the following phase.",
        "#Fortifying Outposts:",
        "Outposts, symbols of your burgeoning empire, can be bolstered up to 20 times in their lifetime. The extent of reinforcements directly influences the Outpost’s defense, manifested in the number of shields it wields:" +
        "\n1-2 reinforcements: Unshielded" +
        "\n3-5 reinforcements: 1 shield" +
        "\n6-9 reinforcements: 2 shields" +
        "\n9-13 reinforcements: 3 shields" +
        "\n14-19 reinforcements: 4 shields" +
        "\n20 reinforcements: 5 shields",
        "#Fortifying Outposts:",
        "Post-preparation, players enter a phase of strategic anticipation. Here, the summoning of new Revenants and bolstering of Outposts continues, setting the stage for the impending Main Phase."
    };

    ////////////////////////////////////////////
    private string[] explanationGP = new string[6] {
           "#Commencing the Main Phase:",
    "Following the initial phase, the game escalates into a whirlwind of action, marked by attacks and disorder.",
    "#Diverse Attacks:",
    "Players must confront challenges ranging from cataclysmic natural disasters to the fiery wrath of dragons and the cunning onslaught of goblins.",
    "#Endurance of Outposts:",
    "The resilience of an Outpost is key, with its survival odds escalating with every reinforcement. The ultimate ambition? To stand as the last Rising Revenant."

    };

    ////////////////////////////////////////////
    private string[] explanationC = new string[8] {
            "#Final Rewards:",
    "The Ultimate Prize: The games transactions feed into a colossal final jackpot, destined for the sole Revenant who outlasts all others.",
    "#Economic Dynamics of \"Rising Revenant\":",
    "#Preparation Phase:",
    "75% of $LORDS channeled into the final jackpot\n10% allocated to transaction confirmation\n15% as a creator tribute",
    "#Main Phase:",
    "90% of $LORDS flows to the trader\n5% augments the final jackpot\n5% reserved as a lasting reward for the enduring players",
    "These rules are your compass in the world of \"Rising Revenant,\" guiding you through a labyrinth of summoning, defense, and cunning trade to claim the crown of the ultimate survivor."

    };

    ////////////////////////////////////////////
    private string[] explanationFR = new string[2] {
           "#Contribution:",
            "In our game, \"contribution\" refers to a player's active engagement in verifying in-game events on the blockchain. Contributors, who validate at least one event, become eligible for a share of the \"contribution jackpot\", which is separate from the main prize upon the game's conclusion."
    };

    ////////////////////////////////////////////

    private string[][] explanation = new string[4][];

    private void Awake()
    {
        explanation[0] = explanationPP;
        explanation[1] = explanationGP;
        explanation[2] = explanationC;
        explanation[3] = explanationFR;
    }

    public GameObject rulesParent;
    private void Start()
    {
        selectedMenu = -1;

        ChangeSelectedMenuButton(0);
    }

    public void ChangeSelectedMenuArrow(int index)
    {
        if (selectedMenu + index < 0)
        {
            return;
        }
        else if (selectedMenu + index > 3)
        {
            return;
        }

        selectedMenu += index;

        if (selectedMenu == 0)
        {
            leftArrow.transform.gameObject.SetActive(false);
        }
        else
        {
            leftArrow.transform.gameObject.SetActive(true);
        }

        if (selectedMenu == 3)
        {
            rightArrow.transform.gameObject.SetActive(false);
        }
        else
        {
            rightArrow.transform.gameObject.SetActive(true);
        }

        WriteRules();
    }

    public void ChangeSelectedMenuButton(int index)
    {
        if (index == selectedMenu) { return;  }

        selectedMenu = index;

        if (selectedMenu == 0)
        {
            leftArrow.transform.gameObject.SetActive(false);
        }
        else
        {
            leftArrow.transform.gameObject.SetActive(true);
        }

        if (selectedMenu == 3)
        {
            rightArrow.transform.gameObject.SetActive(false);
        }
        else
        {
            rightArrow.transform.gameObject.SetActive(true);
        }

        WriteRules();
    }


    private void WriteRules()
    {
        

        foreach (Transform child in rulesParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var text in explanation[selectedMenu])
        {
            GameObject instance = Instantiate(textPrefab, rulesParent.transform);
            TMPro.TextMeshProUGUI tmpTextComponent = instance.GetComponentInChildren<TMPro.TextMeshProUGUI>(); 

            if (text.StartsWith("#"))
            {
                tmpTextComponent.text = text.Substring(1);

                tmpTextComponent.fontSize = 36;
                tmpTextComponent.fontStyle = TMPro.FontStyles.Underline;
            }
            else
            {
                // Set the text as is for regular lines
                tmpTextComponent.text = text;
            }
        }
    }
}
