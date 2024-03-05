
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapComponentBehaviour : MonoBehaviour
{

    // this doesnt have to be a sub we can just get a func called fomr the world event manager
    // swithc to using trig
    //issue on the moving of up and down of the minimap thing
    public Vector2 worldEventPos;
    public float radius;

    public RawImage spot;

    [Header("all to clear out")]
    public Vector2 topLeftCorner = Vector2.zero;
    public Vector2 botRightCorner = Vector2.zero;
    public Vector2 scale = Vector2.zero;

    public float scaleOfMinimapRelativeToRealSize;
    public float totWidth;
    public float totHeight;

    public float camAreaWidhtStandard = 2022;   // this is a pain in the ass
    public float camAreaHeightStandard = 1140;
    public float camHeightValueStandard = 1000;

    private Texture2D redCircleTexture;

    public RawImage mapImage;


    private void Awake()
    {
        UiEntitiesReferenceManager.minimapComp = this;
    }

    void Start()
    {
        var centerPos = new Vector2(gameObject.GetComponent<RectTransform>().anchoredPosition.x, gameObject.GetComponent<RectTransform>().anchoredPosition.y);
        scale = new Vector2(gameObject.GetComponent<RectTransform>().rect.width, gameObject.GetComponent<RectTransform>().rect.height);

        topLeftCorner = new Vector2(centerPos.x - scale.x / 2,Mathf.Abs( centerPos.y) - scale.y / 2);
        botRightCorner = new Vector2(centerPos.x + scale.x / 2, Mathf.Abs(centerPos.y) + scale.y / 2);

        scaleOfMinimapRelativeToRealSize = (botRightCorner.x - topLeftCorner.x) / 10240;   
    }

    private void OnEnable()
    {
        if (DojoEntitiesDataManager.currentWorldEvent != null)
        {
            SpawnEventOnMinimap(DojoEntitiesDataManager.currentWorldEvent);
        }
    }

    public void SpawnEventOnMinimap(CurrentWorldEvent worldEvent)
    {
        this.radius = 150;
        this.worldEventPos = new Vector2(worldEvent.position.x, worldEvent.position.y);
        redCircleTexture = CreateCircleTexture(150, Color.red);
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


                    CameraController.Instance.transform.position = evenRealerLocation;

                    //CameraController.Instance.MoveCameraTo( new Vector3(  RisingRevenantUtils.MAP_WIDHT * (realLocation.x / scale.x)    ,0f, RisingRevenantUtils.MAP_HEIGHT * (realLocation.y / scale.y))  , 2f);
                }
            }
        }
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

    Texture2D CreateCircleTexture(float diameter, Color color)
    {
        int diameterInt = Mathf.RoundToInt(diameter);

        Texture2D texture = new Texture2D(diameterInt, diameterInt, TextureFormat.ARGB32, false);
        float rSquared = (diameterInt / 2) * (diameterInt / 2);

        for (int u = 0; u < diameterInt; u++)
        {
            for (int v = 0; v < diameterInt; v++)
            {
                int x = u - diameterInt / 2;
                int y = v - diameterInt / 2;
                if (x * x + y * y <= rSquared) texture.SetPixel(u, v, color);
                else texture.SetPixel(u, v, Color.clear);
            }
        }

        texture.Apply();
        return texture;
    }

    private void OnGUI()
    {
        if (redCircleTexture != null)
        {
            GUI.DrawTexture(new Rect(
                topLeftCorner.x + ( (worldEventPos.x / 10240) * scale.x) - radius * scaleOfMinimapRelativeToRealSize, 
                botRightCorner.y - ((worldEventPos.y / 5124) * scale.y) - radius * scaleOfMinimapRelativeToRealSize, 
                radius * scaleOfMinimapRelativeToRealSize, 
                radius * scaleOfMinimapRelativeToRealSize), 
                redCircleTexture);
        }

        var currentCamHeight = CameraController.Instance.transform.position;

        var magnification = (currentCamHeight.y / camHeightValueStandard);

        var something = (currentCamHeight.z + (camAreaHeightStandard * magnification) * 0.75f) *scaleOfMinimapRelativeToRealSize;

        var boundsWidth = CameraController.Instance.currentBoundsMaxX - CameraController.Instance.currentBoundsMinX;
        var boundsHeight = CameraController.Instance.currentBoundsMaxZ - CameraController.Instance.currentBoundsMinZ;

        DrawRectangleOutline(new Rect(
            (currentCamHeight.x - (camAreaWidhtStandard * magnification) / 2) * scaleOfMinimapRelativeToRealSize   +  topLeftCorner.x,
            botRightCorner.y - something ,
            boundsWidth * scaleOfMinimapRelativeToRealSize,
            boundsHeight * scaleOfMinimapRelativeToRealSize
            ), 4, new Color(100, 100, 100));
    }

    void DrawRectangleOutline(Rect rect, float thickness, Color color)
    {
        // Top
        GUI.color = color;
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, thickness), Texture2D.whiteTexture);
        // Left
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, thickness, rect.height), Texture2D.whiteTexture);
        // Right
        GUI.DrawTexture(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), Texture2D.whiteTexture);
        // Bottom
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), Texture2D.whiteTexture);
    }
}
