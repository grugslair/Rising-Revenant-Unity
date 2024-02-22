using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhaseManager : MonoBehaviour
{
    public MenuManager menuManager;
    public GameObject componentsParents;
    public GameObject winningPopUp;
    public GameObject worldEventManager;

    public TopBarUiElement PopUpUiElement;


    private void OnEnable()
    {
        winningPopUp.SetActive(false);
        DojoEntitiesDataManager.gameEntityCounterInstance.OnValueChange += CheckForWin;
        worldEventManager.SetActive(true);

        CheckForWin();
    }

    private void OnDisable()
    {
        DojoEntitiesDataManager.gameEntityCounterInstance.OnValueChange -= CheckForWin;
        worldEventManager.SetActive(false);
    }

    public void CheckForWin()
    {
        if (DojoEntitiesDataManager.gameEntityCounterInstance.outpostExistsCount == 1)
        {
            winningPopUp.SetActive(true);
        }
    }

    public void CheckForMenuPages()
    {
        if (menuManager.currentlyOpened != null)
        {
            componentsParents.SetActive(false);
            CameraController.Instance.active = false;
        }
        else
        {
            CameraController.Instance.active = true;
            componentsParents.SetActive(true);
        }
    }

}
