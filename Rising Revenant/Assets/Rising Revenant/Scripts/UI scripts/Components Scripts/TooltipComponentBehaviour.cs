using Dojo;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipComponentBehaviour : MonoBehaviour
{

    public TMP_Text revenantDataText;

    public TMP_Text outpostDataText;  // this is a bit weird
    public TMP_Text stateText;

    public GameObject buttonValidate;

    public TMP_Text outpostSelectedCount;

    public GameObject shieldsParent;

    private List<RisingRevenantUtils.Vec2> savedIds = new List<RisingRevenantUtils.Vec2>();
    private int currentIndex = 0;

    public GameObject parentObject;

    private void Awake()
    {
        UiEntitiesReferenceManager.tooltipCompBehaviour = this;
    }

    private void Start()
    {
        CloseTooltip();
    }

    public void CloseTooltip()
    {
        currentIndex = 0;
       
        parentObject.SetActive(false);

        if (savedIds.Count > 0)
        {
            DojoEntitiesDataManager.outpostDictInstance[savedIds[currentIndex]].OnValueChange -= LoadData;
        }
       
        savedIds.Clear();
    }

    public void ChangeSelected(int id)
    {
        if (currentIndex + id >= savedIds.Count)
        {
            return;
        }
        if (currentIndex + id < 1)
        {
            return;
        }

        DojoEntitiesDataManager.outpostDictInstance[savedIds[currentIndex]].OnValueChange -= LoadData;

        currentIndex += id;

        DojoEntitiesDataManager.outpostDictInstance[savedIds[currentIndex]].OnValueChange += LoadData;

        outpostSelectedCount.text = $"{currentIndex}/{savedIds.Count}";

        LoadData();
    }

    void LoadData()
    {
        Outpost outpostData = DojoEntitiesDataManager.outpostDictInstance[savedIds[currentIndex]];
       
        outpostDataText.text = $"{-1} ID - X:{outpostData.position.x} || Y:{outpostData.position.y}\nReinforcements: {outpostData.life}\nState: ";

        var state = CalcState( outpostData);

        if (state != 1)
        {
            buttonValidate.SetActive(false);
        }
        else
        {
            buttonValidate.SetActive(true);
        }

        int i = 0;
        foreach (Transform child in shieldsParent.transform)
        {
            if (i >= RisingRevenantUtils.CalculateShields(outpostData.life))
            {
                RawImage image = child.GetComponent<RawImage>();
                image.color = new Color(1, 1, 1, 0);
            }

            i++;
        }

        revenantDataText.text = $"Owner: {outpostData.ownerAddress.Hex().Substring(0,6)}\nName {RisingRevenantUtils.GetFullRevenantName(outpostData.position)}";
    }

    int CalcState( Outpost outpost)
    {
        if (outpost.life <= 0)
        {
            stateText.text = "Dead";
            stateText.color = Color.grey;
            return 0;
        }

        if (outpost.isAttacked)
        {
            stateText.text = "Hit";
            stateText.color = Color.blue;
            return 1;
        }

        stateText.text = "Healthy";
        stateText.color = Color.green;
        return 2;
    }

    public void HandleRaycastHits(List<RisingRevenantUtils.Vec2> hitIds)
    {
        if (hitIds.Count > 0)
        {
            Debug.Log("Waht is gionuibefuibe");

            parentObject.SetActive(true);
            savedIds.Clear();
            currentIndex = 0;

            DojoEntitiesDataManager.outpostDictInstance[hitIds[currentIndex]].OnValueChange += LoadData;
            savedIds = hitIds;

            outpostSelectedCount.text = $"1/{hitIds.Count}";

            LoadData();
        }
    }

    public void ConfirmOutpost()
    {
        var structCall = new DojoCallsManager.DamageOutpostStruct
        {
            gameId = DojoEntitiesDataManager.currentGameId,
            outpostId = DojoEntitiesDataManager.outpostDictInstance[savedIds[currentIndex]].position
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            account = DojoEntitiesDataManager.currentAccount,
            addressOfSystem = DojoCallsManager.outpostActionsAddress,
            functionName = "verify"
        };

        var transaction = DojoCallsManager.DamageOutpostDojoCall(structCall, endpoint);

    }
}
