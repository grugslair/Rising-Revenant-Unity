using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    private WorldEvent latestEvent;

    public MenuManager menuManager;
    public Menu eventMenu;







    private static readonly int SizeOfCircle = Shader.PropertyToID("_SizeOfCircle");
    private static readonly int FullnessOfColor = Shader.PropertyToID("_FullnessOfColor");
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private Material shaderMaterial;

    private void Start()
    {
        // Get the material from the Renderer component
        shaderMaterial = GetComponent<Renderer>().material;

        // Start the coroutine to animate shader properties
        StartCoroutine(AnimateShaderPropertiesCoroutine());
    }

    private IEnumerator AnimateShaderPropertiesCoroutine()
    {
        while (true) // Infinite loop to continuously animate
        {
            // Animate from start to end values
            yield return StartCoroutine(ChangeShaderProperties(0.3f, 0.7f, -0.2f, 0.3f, true));

            // Animate from end back to start values
            yield return StartCoroutine(ChangeShaderProperties(0.7f, 0.3f, 0.3f, -0.2f, false));
        }
    }

    private IEnumerator ChangeShaderProperties(float sizeStart, float sizeEnd, float fullnessStart, float fullnessEnd, bool increasing)
    {
        float duration = 2.0f; // Duration of the animation in seconds
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / duration; // 0 to 1 over the duration

            // Lerp the properties based on normalizedTime
            float sizeOfCircle = Mathf.Lerp(sizeStart, sizeEnd, normalizedTime);
            float fullnessOfColor = Mathf.Lerp(fullnessStart, fullnessEnd, normalizedTime);

            // Apply the lerped values to the shader
            shaderMaterial.SetFloat(SizeOfCircle, sizeOfCircle);
            shaderMaterial.SetFloat(FullnessOfColor, fullnessOfColor);

            // Optionally change the base color gradually
            if (increasing)
            {
                shaderMaterial.SetColor(BaseColor, Color.Lerp(Color.blue, Color.red, normalizedTime));
            }
            else
            {
                shaderMaterial.SetColor(BaseColor, Color.Lerp(Color.red, Color.blue, normalizedTime));
            }

            yield return null; // Wait until the next frame to continue
        }
    }













    private void OnEnable()
    {
        DojoEntitiesDataManager.OnWorldEventAdded += HandleWorldEventAdded;

        var lastWorldEnt = DojoEntitiesDataManager.GetLatestEvent();
        if (lastWorldEnt != null)
        {
            LoadLastWorldEventData(lastWorldEnt);
        }
        //sub to when a world event gets added
    }

    private void OnDisable()
    {

        DojoEntitiesDataManager.OnWorldEventAdded -= HandleWorldEventAdded;
    }


    private void HandleWorldEventAdded(WorldEvent worldEvent)
    {
        if (DojoEntitiesDataManager.gameEntityCounterInstance != null)
        {
            if (DojoEntitiesDataManager.gameEntityCounterInstance.eventCount == RisingRevenantUtils.FieldElementToInt(worldEvent.entityId))
            {
                LoadLastWorldEventData(worldEvent);
            }
        }
    }

    private void OnMouseDown()
    {
        if (menuManager.currentlyOpened == null)
        {
            Debug.Log("this got clicked on");
            menuManager.OpenMenu(eventMenu);
        }
    }

    private void LoadLastWorldEventData(WorldEvent lastWorldEvent) {

        Debug.Log("I am loading in new data");

        gameObject.transform.position = new Vector3(lastWorldEvent.xPosition, 0.01f, lastWorldEvent.yPosition);
        float scaleFactor = MathF.Sqrt(lastWorldEvent.radius);

        gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        foreach (var item in DojoEntitiesDataManager.outpostDictInstance)
        {

            var outpost = item.Value;

            if (RisingRevenantUtils.IsPointInsideCircle(new Vector2(lastWorldEvent.xPosition,lastWorldEvent.yPosition), lastWorldEvent.radius, new Vector2(outpost.xPosition, outpost.yPosition)))
            {
                outpost.SetAttackState(true);
                Debug.Log("I am calling for the hit reg");
            }
            else
            {
                outpost.SetAttackState(false);
                outpost.SetOutpostTexture();
            }

        }

    }
}
