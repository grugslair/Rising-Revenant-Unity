using TMPro;
using UnityEngine;

public class PlayerReinforcementBalanceUiComponents : MonoBehaviour
{
    
    /*
     * This will only get the most up to date value of the users amount of reinforcement it currently has
     */

    [SerializeField]
    private TMP_Text reinforcementText;

    public int reinforcementAmount;

    private void OnEnable()
    {
        if (DojoEntitiesDataManager.playerSpecificData != null)
        {
            DojoEntitiesDataManager.playerSpecificData.OnValueChange += SetTextValue;  //subscribe to the event that listens to the change of the entity
            SetTextValue();  
        }
        else
        {
            reinforcementText.text = "0";
            reinforcementAmount = -1;
        }
    }

    private void SetTextValue()
    {
        reinforcementText.text = DojoEntitiesDataManager.playerSpecificData.reinforcementAvailableCount.ToString();
        reinforcementAmount = (int)DojoEntitiesDataManager.playerSpecificData.reinforcementAvailableCount;
    }

    private void OnDisable()
    {
        if (DojoEntitiesDataManager.playerSpecificData != null)
        {
            DojoEntitiesDataManager.playerSpecificData.OnValueChange -= SetTextValue;
        }
    }
}
