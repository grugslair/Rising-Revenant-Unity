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
        
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SetUiState(0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SetUiState(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SetUiState(2);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SetUiState(3);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            SetUiState(4);
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
