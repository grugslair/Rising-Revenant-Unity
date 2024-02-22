using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public event Action<List<int>> OnRaycastHits;
    public static CameraController Instance { get; private set; }

    public float panSpeed = 20f;
    public float zoomSpeed = 2f;
    public float minY = 10f;
    public float maxY = 80f;

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

        MoveCamera();
        ZoomCamera();
        DragCamera();
        InstantTeleport();
        SendRaycast();
    }

    private void MoveCamera()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * panSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
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

            transform.Translate(move, Space.World);
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
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
        }
    }

    private void SendRaycast()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            int layerMask = 1 << LayerMask.NameToLayer("Outpost");

            hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

            // Check if any hits were recorded
            if (hits.Length > 0)
            {
                List<int> hitIds = new List<int>();

                foreach (RaycastHit hit in hits)
                {
                    Outpost component = hit.collider.gameObject.GetComponent<Outpost>();
                    if (component != null)
                    {
                        hitIds.Add(RisingRevenantUtils.FieldElementToInt(component.entityId)); 
                    }

                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                }

                // Invoke the event if there are listeners and at least one ID was collected
                if (OnRaycastHits != null && hitIds.Count > 0)
                {
                    OnRaycastHits(hitIds);
                }
            }
        }
    }



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
}
