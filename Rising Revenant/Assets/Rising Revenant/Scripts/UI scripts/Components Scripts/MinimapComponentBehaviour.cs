
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapComponentBehaviour : MonoBehaviour
{
    public Vector2 scale = Vector2.zero;

    public RectTransform spot;
    public float SpotSize = 10f;

    public RawImage mapImage;

    public RectTransform parentMinimapComp;
    public RectTransform cameraView;

    private void Awake()
    {
        UiEntitiesReferenceManager.minimapComp = this;
    }

    private void OnEnable()
    {
        if (DojoEntitiesDataManager.currentWorldEvent != null)
        {
            SpawnEventOnMinimap(new Vector2(DojoEntitiesDataManager.currentWorldEvent.position.x, DojoEntitiesDataManager.currentWorldEvent.position.y));
        }
    }

    private void Start()
    {
        spot.gameObject.SetActive(false);
    }

    public void SpawnEventOnMinimap(Vector2 worldPosition)
    {
        spot.gameObject.SetActive(true);
        var compHeight = parentMinimapComp.rect.height;
        var compWidth = parentMinimapComp.rect.width;

        float scaledX =compWidth - (worldPosition.x / RisingRevenantUtils.MAP_WIDHT) * compWidth;
        float scaledY = (worldPosition.y / RisingRevenantUtils.MAP_HEIGHT) * compHeight;

        spot.anchoredPosition = new Vector2(scaledX, scaledY);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIObject(mapImage.GetComponent<RectTransform>()))
            {
                Vector2 localCursor;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mapImage.rectTransform, Input.mousePosition, null, out localCursor))
                {
                    var realLocation = new Vector2(localCursor.x + scale.x / 2, localCursor.y + scale.y / 2);
                    var evenRealerLocation = new Vector3(RisingRevenantUtils.MAP_WIDHT * (realLocation.x / scale.x), 0f, RisingRevenantUtils.MAP_HEIGHT * (realLocation.y / scale.y));
                    //lol what

                    CameraController.Instance.transform.position = evenRealerLocation;
                    //CameraController.Instance.MoveCameraTo( new Vector3(  RisingRevenantUtils.MAP_WIDHT * (realLocation.x / scale.x)    ,0f, RisingRevenantUtils.MAP_HEIGHT * (realLocation.y / scale.y))  , 2f);
                }
            }
        }

        MinimapCameraViewSet();
    }

    private bool IsPointerOverUIObject(RectTransform rectTransform)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<RectTransform>() == rectTransform)
                return true;
        }
        return false;
    }

    void MinimapCameraViewSet()
    {
        var cam = CameraController.Instance;

        var compHeight = parentMinimapComp.rect.height;
        var compWidth = parentMinimapComp.rect.width;

        var leftDistance = Mathf.Clamp((cam.currentBoundsMinX / RisingRevenantUtils.MAP_WIDHT) * compWidth, 0, compWidth);
        var rightDistance = Mathf.Clamp(compWidth - (cam.currentBoundsMaxX / RisingRevenantUtils.MAP_WIDHT) * compWidth, 0, compWidth);
        var topDistance = Mathf.Clamp(compHeight - (cam.currentBoundsMaxZ / RisingRevenantUtils.MAP_HEIGHT) * compHeight, 0, compHeight);
        var botDistance = Mathf.Clamp((cam.currentBoundsMinZ / RisingRevenantUtils.MAP_HEIGHT) * compHeight, 0, compHeight);

        cameraView.offsetMin = new Vector2(leftDistance, botDistance);
        cameraView.offsetMax = new Vector2(-rightDistance, -topDistance);
    }
}
