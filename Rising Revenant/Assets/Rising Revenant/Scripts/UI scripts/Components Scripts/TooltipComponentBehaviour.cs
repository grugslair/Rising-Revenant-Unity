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

    private List<int> savedIds = new List<int>();
    private int currentIndex = 0;

    public GameObject parentObject;

    private void Start()
    {
        CloseTooltip();
    }

    void OnEnable()
    {
        CameraController.Instance.OnRaycastHits += HandleRaycastHits;
    }

    void OnDisable()
    {
        CameraController.Instance.OnRaycastHits -= HandleRaycastHits;
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
        Revenant revData = DojoEntitiesDataManager.revDictInstance[savedIds[currentIndex]];
        Outpost outpostData = DojoEntitiesDataManager.outpostDictInstance[savedIds[currentIndex]];
       
        outpostDataText.text = $"{RisingRevenantUtils.FieldElementToInt(revData.entityId)} ID - X:{outpostData.xPosition} || Y:{outpostData.yPosition}\nReinforcements: {outpostData.lifes}\nState: ";

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
            if (i >= outpostData.shield)
            {
                RawImage image = child.GetComponent<RawImage>();
                image.color = new Color(1, 1, 1, 0);
            }

            i++;
        }

        revenantDataText.text = $"Owner: {revData.ownerAddress.Hex().Substring(0,6)}\nName {RisingRevenantUtils.GetFullRevenantName(RisingRevenantUtils.FieldElementToInt(revData.entityId))}";
    }

    int CalcState( Outpost outpost)
    {
        var latestEvent = DojoEntitiesDataManager.GetLatestEvent();

        if (outpost.lifes <= 0)
        {
            stateText.text = "Dead";
            stateText.color = Color.grey;
            return 0;
        }

        if (latestEvent != null)
        {
            if (RisingRevenantUtils.IsPointInsideCircle(new Vector2(latestEvent.xPosition, latestEvent.yPosition), latestEvent.radius, new Vector2(outpost.xPosition, outpost.yPosition)))
            {
                stateText.text = "Hit";
                stateText.color = Color.blue;
                return 1;
            }
        }

        stateText.text = "Healthy";
        stateText.color = Color.green;
        return 2;
    }

    void HandleRaycastHits(List<int> hitIds)
    {
        if (hitIds.Count > 0)
        {
            parentObject.SetActive(true);
            savedIds.Clear();
            currentIndex = 0;

            DojoEntitiesDataManager.outpostDictInstance[hitIds[currentIndex]].OnValueChange += LoadData;
            savedIds = hitIds;

            outpostSelectedCount.text = $"1/{hitIds.Count}";

            LoadData();
        }
    }
}
