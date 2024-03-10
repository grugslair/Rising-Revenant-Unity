using UnityEngine;

public class GamePhaseManager : MonoBehaviour
{
    public MenuManager menuManager;
    public CanvasGroup componentsParents;
    public GameObject winningPopUp;
    public GameObject eventObj;
    public GameObject worldEventManager;

    public TopBarUiElement PopUpUiElement;

    public bool objectsAreVisible = true;

    private void Start()
    {
        UiEntitiesReferenceManager.gamePhaseManager = this;
    }

    private void OnEnable()
    {
        winningPopUp.SetActive(false);
        CameraController.Instance.active = true;

        if (DojoEntitiesDataManager.currentWorldEvent == null)
        {
            worldEventManager.SetActive(false);
        }
        else
        {
            worldEventManager.GetComponent<WorldEventManager>().LoadLastWorldEventData(DojoEntitiesDataManager.currentWorldEvent);
            worldEventManager.SetActive(true);
        }

        CheckForWin();
    }


    void Update()
    {
        // Check if the H key was pressed
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Toggle visibility
            objectsAreVisible = !objectsAreVisible;
            componentsParents.alpha = componentsParents.alpha == 0.2f ? 1f : 0.2f ;
        }
    }

    private void OnDisable()
    {
        worldEventManager.SetActive(false);
        CameraController.Instance.active = false;
    }

    public void CheckForWin()
    {
        if (DojoEntitiesDataManager.gameEntCounter.outpostRemainingCount <= 1)
        {
            winningPopUp.SetActive(true);
            winningPopUp.SetActive(false);
        }
        else
        {
            winningPopUp.SetActive(false);
            eventObj.SetActive(true);
        }
    }

    public void CheckForMenuPages()
    {
        if (menuManager.currentlyOpened != null)
        {
            componentsParents.alpha = 0f;
            CameraController.Instance.active = false;
            objectsAreVisible = false;
        }
        else
        {
            CameraController.Instance.active = true;
            componentsParents.alpha = 1f;
            objectsAreVisible = true;
        }
    }
}
