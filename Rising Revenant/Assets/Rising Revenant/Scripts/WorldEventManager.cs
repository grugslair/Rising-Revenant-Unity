using bottlenoselabs.C2CS.Runtime;
using Dojo;
using dojo_bindings;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldEventManager : MonoBehaviour
{
    public MenuManager menuManager;
    public Menu eventMenu;
    public WorldManager worldManager;

    private bool loaded = false;

    // world ebent manager shoudl dod that amange
    // on new event check whicih outpost have been hit, call an update by sending the event to the rev journal and minimap

    // this should be called all only when the game is game phase prob just disable and then actiove

    private static readonly int SizeOfCircle = Shader.PropertyToID("_SizeOfCircle");
    private static readonly int FullnessOfColor = Shader.PropertyToID("_FullnessOfColor");
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private Material shaderMaterial;

    private void Awake()
    {
        UiEntitiesReferenceManager.worldEventManager = this;
    }

    private void Start()
    {
        shaderMaterial = GetComponent<Renderer>().material;
        StartCoroutine(AnimateShaderPropertiesCoroutine());
    }

    private void OnEnable()
    {
        if (DojoEntitiesDataManager.currentWorldEvent == null) {
            this.transform.gameObject.SetActive(false);
            if (UiEntitiesReferenceManager.warningSystemUiComponent != null)
            {
                UiEntitiesReferenceManager.warningSystemUiComponent.transform.gameObject.SetActive(false);
            }
        }
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
            float normalizedTime = time / duration; 

            float sizeOfCircle = Mathf.Lerp(sizeStart, sizeEnd, normalizedTime);
            float fullnessOfColor = Mathf.Lerp(fullnessStart, fullnessEnd, normalizedTime);

            shaderMaterial.SetFloat(SizeOfCircle, sizeOfCircle);
            shaderMaterial.SetFloat(FullnessOfColor, fullnessOfColor);

            // Optionally change the base color gradually
            //if (increasing)
            //{
            //    shaderMaterial.SetColor(BaseColor, Color.Lerp(Color.blue, Color.red, normalizedTime));
            //}
            //else
            //{
            //    shaderMaterial.SetColor(BaseColor, Color.Lerp(Color.red, Color.blue, normalizedTime));
            //}

            yield return null; // Wait until the next frame to continue
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() && UiEntitiesReferenceManager.gamePhaseManager.objectsAreVisible)
        {
            return;
        }

        if (menuManager.currentlyOpened == null)
        {
            UiEntitiesReferenceManager.gamePhaseManager.CheckForMenuPages();
            menuManager.OpenMenu(eventMenu);
        }
    }

    public void LoadLastWorldEventData(CurrentWorldEvent lastWorldEvent) 
    {
        if (UiEntitiesReferenceManager.warningSystemUiComponent != null)
        {
            if (UiEntitiesReferenceManager.warningSystemUiComponent.transform.gameObject.activeSelf == false)
            {
                UiEntitiesReferenceManager.warningSystemUiComponent.transform.gameObject.SetActive(true);
            }
        }

        //UnsubAllOutposts();

        gameObject.transform.position = new Vector3(lastWorldEvent.position.x, 0.01f, lastWorldEvent.position.y);
        //what??
        float scaleFactor = MathF.Sqrt(lastWorldEvent.radius*2) *2;

        gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        
        foreach (var outpost in DojoEntitiesDataManager.outpostDictInstance.Values)
        {
            if (RisingRevenantUtils.IsPointInsideCircle(new Vector2(lastWorldEvent.position.x,lastWorldEvent.position.y), lastWorldEvent.radius, new Vector2(outpost.position.x, outpost.position.y)))
            {
                outpost.SetAttackState(true);
                Debug.Log($"I have attacked one outpost, the outpost is at {outpost.position.x} {outpost.position.y}");

                //var outpostModel = new dojo.KeysClause[]
                //{ new() { model_ = CString.FromString("Outpost"), keys = new[] { DojoEntitiesDataManager.currentGameId.ToString(),outpost.position.x.ToString("x").ToLower() , outpost.position.y.ToString("x").ToLower() } } };

                //outpost.isSubbed = true;

                //worldManager.toriiClient.AddModelsToSync(outpostModel);
            }
            else
            {
                outpost.SetAttackState(false);
                outpost.SetOutpostTexture();
            }
        }

        //call minimap
        if (UiEntitiesReferenceManager.minimapComp != null)
        {
            UiEntitiesReferenceManager.minimapComp.SpawnEventOnMinimap(lastWorldEvent);
        }

        //call revj
        if (UiEntitiesReferenceManager.revJournalCompBehaviour != null)
        {
            UiEntitiesReferenceManager.revJournalCompBehaviour.HandleWorldEventAdded(lastWorldEvent);
        }
    }
    private void UnsubAllOutposts()
    {
        foreach (var outpost in DojoEntitiesDataManager.outpostDictInstance.Values)
        {
            if (outpost.isSubbed)
            {
                var outpostModel = new dojo.KeysClause[]
                { new() { model_ = CString.FromString("Outpost"), keys = new[] { DojoEntitiesDataManager.currentGameId.ToString(), outpost.position.x.ToString("x").ToLower(), outpost.position.y.ToString("x").ToLower() } } };

                outpost.isSubbed = false;
                worldManager.toriiClient.RemoveModelsToSync(outpostModel);
            }
        }
    }
}
