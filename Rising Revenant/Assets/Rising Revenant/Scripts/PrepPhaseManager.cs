using bottlenoselabs.C2CS.Runtime;
using Dojo;
using dojo_bindings;
using Unity.VisualScripting;
using UnityEngine;

public class PrepPhaseManager : MonoBehaviour
{
    public WorldManager worldManager;

    private void Awake()
    {
        UiEntitiesReferenceManager.prepPhaseUIManager = this;
    }

    private void OnEnable()
    {
        //var models = new dojo.KeysClause[]
        //{ new() { model_ = CString.FromString("GameState"), keys = new[] { "0" } } };

        //worldManager.toriiClient.AddModelsToSync(models);

        //var subscribedModels = worldManager.toriiClient.SubscribedModels();

        //for (var i = 0; i < subscribedModels.Length; i++)
        //{
        //    Debug.Log(subscribedModels[i].model);
        //    Debug.Log(subscribedModels[i].keys[0]);
        //}
    }




    private void Update()
    {
        if (DojoEntitiesDataManager.currentWorldEvent != null)
        {
            //UiEntitiesReferenceManager.uistat.setu
        }
    }
}
