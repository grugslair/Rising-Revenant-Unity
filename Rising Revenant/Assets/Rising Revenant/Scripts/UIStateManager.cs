using Dojo.Starknet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] UIScreenObjs = new GameObject[4];

    public enum UIState
    {
        LOGIN = 0,
        LOADING,
        VIDEO,
        PREP,
        GAME
    }

    public UIState currentUiState = UIState.LOGIN;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Y))
        {
            SetUiState(0);
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
            SetUiState(1);
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            SetUiState(2);
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            SetUiState(3);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            SetUiState(4);
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            UiEntitiesReferenceManager.notificationManager.CreateNotification("Test", null, 5);
        }
    }

    /// <summary>
    /// need to take in an int otherwise buttons dont recognise it, so just cast the enum when called
    /// </summary>
    /// <param name="stateNum"></param>
    public void SetUiState(int stateNum)
    {
        for (int i = 0; i < UIScreenObjs.Length; i++)
        {
            if (i == (int)stateNum)
            {
                UIScreenObjs[i].SetActive(true);
            }
            else
            {
                UIScreenObjs[i].SetActive(false);
            }
        }
    }
}
