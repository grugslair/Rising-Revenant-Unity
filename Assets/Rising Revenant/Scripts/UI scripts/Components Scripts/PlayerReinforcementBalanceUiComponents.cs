using TMPro;
using UnityEngine;

public class PlayerReinforcementBalanceUiComponents : MonoBehaviour
{

    /*
     This will only get the most up to date value of the users amount of reinforcement it currently has

     PlayerInfo
     */

    [SerializeField]
    private TMP_Text reinforcementText;

    public int reinforcementAmount;

    private void OnEnable()
    {
        if (DojoEntitiesDataManager.playerInfo != null)
        {
           
            SetTextValue();  
        }
        else
        {
            reinforcementText.text = "0";
            reinforcementAmount = -1;
        }

        UiEntitiesReferenceManager.reinforcementCounterElement = this;
    }
    private void OnDisable()
    {

    }

    public void SetTextValue(PlayerInfo playerInfo = null)
    {
        if (playerInfo == null)
        {
            reinforcementText.text = DojoEntitiesDataManager.playerInfo.reinforcementsAvailableCount.ToString();
            reinforcementAmount = (int)DojoEntitiesDataManager.playerInfo.reinforcementsAvailableCount;
        }
        else
        {
            reinforcementText.text = playerInfo.reinforcementsAvailableCount.ToString();
            reinforcementAmount = (int)playerInfo.reinforcementsAvailableCount;
        }
    }
}
