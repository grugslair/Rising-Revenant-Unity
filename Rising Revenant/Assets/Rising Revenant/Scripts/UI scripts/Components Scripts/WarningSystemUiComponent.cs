using UnityEngine;
using UnityEngine.UI;

public class WarningSystemUiComponent : MonoBehaviour
{
    public RawImage WSBackground;
    public Camera mainCamera;
    public GameObject targetObject;

    public string lastsavedDir = "";

    private void Awake()
    {
        UiEntitiesReferenceManager.warningSystemUiComponent = this;
    }

    private void Update()
    { 
        if (DojoEntitiesDataManager.currentWorldEvent == null)
        {
            WSBackground.color = new Color(0, 0, 0, 0);
            return;
        }
        

        if (!IsTargetInView(targetObject))
        {
            var dir = GetDirectionRelativeToCamera(mainCamera);

            if (dir != lastsavedDir)
            {
                lastsavedDir = dir;
                Texture2D image = Resources.Load<Texture2D>("WarningSystem/" + lastsavedDir);
                WSBackground.texture = image;
                WSBackground.color = new Color(1, 1, 1, 1);
            }
        }
        else
        {
            lastsavedDir = "";
            WSBackground.color = new Color(0, 0, 0, 0);
        }
    }

    bool IsTargetInView(GameObject obj)
    {
        if (obj.activeSelf == false)
        {
            return false;
        }

        var corners = GetWorldCorners(obj);
        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(corners[i]);
            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                viewportPoint.z > 0)
            {
                return true;
            }
        }
        return false;
    }

    Vector3[] GetWorldCorners(GameObject obj)
    {
        Vector3[] worldCorners = new Vector3[4];
        if (obj.GetComponent<Renderer>() != null)
        {
            Bounds bounds = obj.GetComponent<Renderer>().bounds;
            worldCorners[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
            worldCorners[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            worldCorners[2] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            worldCorners[3] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        }
        return worldCorners;
    }

    string GetDirectionRelativeToCamera(Camera camera)
    {
        if (targetObject.activeSelf == false)
        {
            return "";
        }

        var corners = GetWorldCorners(targetObject);

        var topLeftCorner = corners[0];
        var bottomRightCorner = corners[2];

        Vector3 topLeftNorm = topLeftCorner - camera.transform.position;
        topLeftNorm = topLeftNorm.normalized;

        Vector3 botRightNorm = bottomRightCorner - camera.transform.position;
        botRightNorm = botRightNorm.normalized;

        if (CheckPositive(botRightNorm.x) && CheckPositive(botRightNorm.z) && CheckPositive(topLeftNorm.x) && !CheckPositive(topLeftNorm.z))
        {
            return "E";
        }
        if (CheckPositive(botRightNorm.x) && CheckPositive(botRightNorm.z) && !CheckPositive(topLeftNorm.x) && CheckPositive(topLeftNorm.z))
        {
            return "N";
        }
        if (CheckPositive(botRightNorm.x) && !CheckPositive(botRightNorm.z) && !CheckPositive(topLeftNorm.x) && !CheckPositive(topLeftNorm.z))
        {
            return "S";
        }
        if (!CheckPositive(botRightNorm.x) && CheckPositive(botRightNorm.z) && !CheckPositive(topLeftNorm.x) && !CheckPositive(topLeftNorm.z))
        {
            return "W";
        }
        if (CheckPositive(botRightNorm.x) && CheckPositive(botRightNorm.z) && CheckPositive(topLeftNorm.x) && CheckPositive(topLeftNorm.z))
        {
            return "NE";
        }
        if (CheckPositive(botRightNorm.x) && !CheckPositive(botRightNorm.z) && CheckPositive(topLeftNorm.x) && !CheckPositive(topLeftNorm.z))
        {
            return "SE";
        }
        if (!CheckPositive(botRightNorm.x) && !CheckPositive(botRightNorm.z) && !CheckPositive(topLeftNorm.x) && !CheckPositive(topLeftNorm.z))
        {
            return "SW";
        }
        if (!CheckPositive(botRightNorm.x) && CheckPositive(botRightNorm.z) && !CheckPositive(topLeftNorm.x) && CheckPositive(topLeftNorm.z))
        {
            return "NW";
        }

        return "";
    }

    bool CheckPositive(float num)
    {
        return num >= 0;
    }
}
