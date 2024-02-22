using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WarningSystemUiComponent : MonoBehaviour
{
    //get the position of the camera
    //get the position of the last event
    //see if the last event is in the screen space
    // if now work out direction and add the texture

    public RawImage WSBackground;
    public Camera mainCamera; // Assign the main camera in the inspector
    public GameObject targetObject; // Assign the target object in the inspector

    private string lastsavedDir = "";

    private void Update()
    {



       var dir =  GetDirectionRelativeToCamera(mainCamera);


        if (dir != lastsavedDir)
        {
            if (!IsTargetInView(targetObject))
            {
                // Target is not in view, determine direction
                string direction = GetDirectionRelativeToCamera(mainCamera);

                if (direction != lastsavedDir)
                {

                    WSBackground.gameObject.SetActive(true);
                    lastsavedDir = direction;
                    Texture2D image = Resources.Load<Texture2D>("WarningSystem/" + lastsavedDir);
                    WSBackground.texture = image;
                }
            }
            else
            {
                if (lastsavedDir != "")
                {
                    lastsavedDir = "";
                    WSBackground.gameObject.SetActive(false);
                }
            }
        }

       
    }

    bool IsTargetInView(GameObject obj)
    {
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

    // Determine the direction of the target relative to the camera and return it as a string
    string GetDirectionRelativeToCamera(Camera camera)
    {
        var corners = GetWorldCorners(targetObject);

        var topLeftCorner = corners[0];
        var bottomRightCorner = corners[2];

        Vector3 topLeftNorm = topLeftCorner - camera.transform.position;
        topLeftNorm = topLeftNorm.normalized;

        Vector3 botRightNorm = bottomRightCorner - camera.transform.position;
        botRightNorm = botRightNorm.normalized;

        if (CheckPositive(botRightNorm.x) && CheckPositive(botRightNorm.z) && CheckPositive(topLeftNorm.x) && !CheckPositive(topLeftNorm.z))
        {
            return "W";
        }

        if (CheckPositive(botRightNorm.x) && CheckPositive(botRightNorm.z) && !CheckPositive(topLeftNorm.x) && CheckPositive(topLeftNorm.z))
        {
            return "S";
        }
        if (CheckPositive(botRightNorm.x) && !CheckPositive(botRightNorm.z) && !CheckPositive(topLeftNorm.x) && !CheckPositive(topLeftNorm.z))
        {
            return "N";
        }
        if (!CheckPositive(botRightNorm.x) && CheckPositive(botRightNorm.z) && !CheckPositive(topLeftNorm.x) && !CheckPositive(topLeftNorm.z))
        {
            return "E";
        }
        if (CheckPositive(botRightNorm.x) && CheckPositive(botRightNorm.z) && CheckPositive(topLeftNorm.x) && CheckPositive(topLeftNorm.z))
        {
            return "SW";
        }
        if (CheckPositive(botRightNorm.x) && !CheckPositive(botRightNorm.z) && CheckPositive(topLeftNorm.x) && !CheckPositive(topLeftNorm.z))
        {
            return "NW";
        }
        if (!CheckPositive(botRightNorm.x) && !CheckPositive(botRightNorm.z) && !CheckPositive(topLeftNorm.x) && !CheckPositive(topLeftNorm.z))
        {
            return "NE";
        }
        if (!CheckPositive(botRightNorm.x) && CheckPositive(botRightNorm.z) && !CheckPositive(topLeftNorm.x) && CheckPositive(topLeftNorm.z))
        {
            return "SE";
        }

        return "";
    }

    bool CheckPositive(float num)
    {
        return num >= 0;
    }

}
