using System.Collections.Generic;
using Dojo;
using Dojo.Starknet;
using UnityEngine;
using Random = UnityEngine.Random;

public class InitializeDojoEntities : MonoBehaviour
{
    public string masterPrivateKey;
    public string masterAddress;

    public WorldManager worldManager;

    public string gameActionsAddress;
    public string eventActionsAddress;
    public string revenantActionsAddress;
    public string tradeReinfActionsAddress;
    public string tradeRevsActionsAddress;

    public BurnerManager burnerManager;
    private Dictionary<FieldElement, string> spawnedBurners = new();

    void Awake()
    {
        var provider = new JsonRpcClient(worldManager.rpcUrl);
        var signer = new SigningKey(masterPrivateKey);
        var account = new Account(provider, signer, new FieldElement(masterAddress));

        burnerManager = new BurnerManager(provider, account);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        DojoEntitiesDataManager.dojoEntInitializer = this;
        
        // this is a subs to eveyr new entity you call that func
        //worldManager.synchronizationMaster.OnEntitySpawned.AddListener(InitEntity);

        //// all the entities loaded do this
        //foreach (var entity in worldManager.Entities())
        //{
        //    InitEntity(entity);
        //}
    }

    public async void SpawnBurner()
    {
        var burner = await burnerManager.DeployBurner();
        spawnedBurners[burner.Address] = null;
        DojoEntitiesDataManager.currentAccount = burner;
    }

    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            var burner = await burnerManager.DeployBurner();
            spawnedBurners[burner.Address] = null;
            DojoEntitiesDataManager.currentAccount = burner;
        }
    }

    //private void InitEntity(GameObject entity)
    //{
    //    var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
    //    // change color of capsule to a random color
    //    capsule.GetComponent<Renderer>().material.color = Random.ColorHSV();
    //    capsule.transform.parent = entity.transform;

    //    foreach (var burner in spawnedBurners)
    //    {
    //        if (burner.Value == null)
    //        {
    //            spawnedBurners[burner.Key] = entity.name;
    //            break;
    //        }
    //    }
    //}
}
