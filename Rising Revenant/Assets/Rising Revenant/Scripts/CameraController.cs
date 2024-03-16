using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public float panSpeed = 20f;
    public float zoomSpeed = 2f;
    public float minY = 10f;
    public float maxY = 80f;

    public float minX = 0;
    public float maxX = 0f;
    public float minZ = 10240f;
    public float maxZ = 5124f;

    public float currentBoundsMinX = 0;
    public float currentBoundsMaxX = 0f;
    public float currentBoundsMinZ = 0f;
    public float currentBoundsMaxZ = 0f;

    public bool active = false;

    private Vector3 dragOrigin;

    public float shakingIntensity = 10f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); 
        }
    }

    void Update()
    {
        if (!active)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ShakeCamera(shakingIntensity, 2f);
        }

        GetBoundsOfCam();
        MoveCamera();
        ZoomCamera();
        DragCamera();
        InstantTeleport();
        SendDataToTooltip();
        UpdateGameObjectScalesRelativeToZoom();
    }

    // need to chekc on the bounds now that they are set

    private void MoveCamera()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * panSpeed * Time.deltaTime;
        Vector3 targetPosition = transform.position + movement;

        // Applying boundary constraints

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);  //
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        transform.position = targetPosition;
    }

    private void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * zoomSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    private void DragCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 posDelta = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(posDelta.x * panSpeed, 0, posDelta.y * panSpeed);

            Vector3 targetPosition = transform.position - move;

            // Applying boundary constraints
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

            transform.position = targetPosition;
            dragOrigin = Input.mousePosition;
        }
    }

    private void InstantTeleport()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 newPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                // Applying boundary constraints
                newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

                transform.position = newPosition;
            }
        }
    }

    private void SendDataToTooltip()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            int layerMask = 1 << LayerMask.NameToLayer("Outpost");

            hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

            if (hits.Length > 0)
            {
                List<RisingRevenantUtils.Vec2> hitIds = new List<RisingRevenantUtils.Vec2>();

                Debug.Log(hits.Length);

                foreach (RaycastHit hit in hits)
                {
                    Outpost component = hit.collider.gameObject.GetComponent<Outpost>();
                    if (component != null)
                    {
                        hitIds.Add(component.position); 
                    }

                }

                UiEntitiesReferenceManager.tooltipCompBehaviour.HandleRaycastHits(hitIds);
            }
        }
    }

    void UpdateGameObjectScalesRelativeToZoom()
    {
        float cameraHeight = Camera.main.transform.position.y;
        float scale = Mathf.Clamp((cameraHeight - minY) / (maxY - minY), 0.5f, 1.5f);

        foreach (Outpost outpostTransform in DojoEntitiesDataManager.outpostDictInstance.Values)
        {
            outpostTransform.gameObject.transform.localScale = new Vector3(10,10,10) * scale;
        }
    }


    void GetBoundsOfCam()
    {
        float distance = this.transform.position.y; 

        float height = 2.0f * distance * Mathf.Tan(this.GetComponent<Camera>().fieldOfView * 0.5f * Mathf.Deg2Rad);
        float width = height * this.GetComponent<Camera>().aspect;

        // Calculate the corners
        Vector3 bottomLeft = this.transform.position + this.transform.forward * distance - this.transform.right * width / 2 - this.transform.up * height / 2;
        Vector3 bottomRight = this.transform.position + this.transform.forward * distance + this.transform.right * width / 2 - this.transform.up * height / 2;
        Vector3 topLeft = this.transform.position + this.transform.forward * distance - this.transform.right * width / 2 + this.transform.up * height / 2;
        Vector3 topRight = this.transform.position + this.transform.forward * distance + this.transform.right * width / 2 + this.transform.up * height / 2;

        bottomLeft.y = 0;
        bottomRight.y = 0;
        topLeft.y = 0;
        topRight.y = 0;

        currentBoundsMinX = bottomLeft.x;
        currentBoundsMaxX = topRight.x;
        currentBoundsMinZ = bottomRight.z;
        currentBoundsMaxZ = topLeft.z;

        var boundsWidht = currentBoundsMaxX - currentBoundsMinX;
        var boundsHeight = currentBoundsMaxZ - currentBoundsMinZ;

        if (currentBoundsMaxX > maxX)
        {  
            transform.position = new Vector3(maxX - boundsWidht/2, transform.position.y, transform.position.z);
        }
        if (currentBoundsMaxZ > maxZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxZ - boundsHeight / 2);
        }
        if (currentBoundsMinX < minX)
        {
            transform.position = new Vector3(minX + boundsWidht/2, transform.position.y, transform.position.z);
        }
        if (currentBoundsMinZ < minZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, minZ + boundsHeight/2);
        }

        Debug.DrawLine(this.transform.position, bottomLeft, Color.red);
        Debug.DrawLine(this.transform.position, bottomRight, Color.green);
        Debug.DrawLine(this.transform.position, topLeft, Color.blue);
        Debug.DrawLine(this.transform.position, topRight, Color.yellow);
    }

    // animations such as shaking and stuff like that needs ot be put in their own class with references to this script in case
    // possible issue i can oversee is that based on how the movemsent works for the shaking it is going to be clamped
    // on the ssidde, os maybe make the camera  achild of a gameobject, chekc on the gameobj position but move the child camera for freedom

    #region Camera animations region
    public void MoveCameraTo(Vector3 targetPosition, float duration)
    {
        StartCoroutine(LerpPosition(new Vector3(targetPosition.x, transform.position.y, targetPosition.z), duration));
    }

    private IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z); // Ensure the final position is set
    }

    public void ShakeCamera(float intensity, float duration)
    {
        StartCoroutine(Shake(intensity, duration));
    }

    private IEnumerator Shake(float intensity, float duration)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float remainingTime = duration - elapsed;
            float currentIntensity = Mathf.Lerp(0f, intensity, remainingTime / duration);

            float x = UnityEngine.Random.Range(-1f, 1f) * currentIntensity;
            float z = UnityEngine.Random.Range(-1f, 1f) * currentIntensity;

            transform.position = new Vector3(originalPosition.x + x, originalPosition.y, originalPosition.z + z);

            elapsed += Time.deltaTime;

            yield return null; // Wait until next frame
        }

        transform.position = originalPosition; // Reset position after shaking
    }
    #endregion
}
