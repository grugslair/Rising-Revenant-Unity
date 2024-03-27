using System.Collections.Generic;
using Dojo;
using Dojo.Starknet;
using UnityEngine;

public class InitializeDojoEntities : MonoBehaviour
{
    public WorldManager worldManager;

    public BurnerManager burnerManager;
    private Dictionary<FieldElement, string> spawnedBurners = new();

    public JsonRpcClient provider;

    void Awake()
    {
        provider = new JsonRpcClient(worldManager.chainConfig.rpcUrl);
        var signer = new SigningKey(worldManager.chainConfig.masterPrivateKey);
        var account = new Account(provider, signer, new FieldElement(worldManager.chainConfig.masterAddress));

        burnerManager = new BurnerManager(provider, account);

        DojoEntitiesDataManager.worldManager = worldManager;
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

    async void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            var burner = await burnerManager.DeployBurner();
            spawnedBurners[burner.Address] = null;
            DojoEntitiesDataManager.currentAccount = burner;
        }
    }


    public async void CallCreateEvent()
    {
        if (DojoEntitiesDataManager.currentAccount == null) { return; }

        var createRevenantsProps = new DojoCallsManager.CreateEventStruct
        {
            gameId = DojoEntitiesDataManager.currentGameId
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            functionName = "random",
            addressOfSystem = DojoEntitiesDataManager.worldManager.chainConfig.eventActionsAddress,
            account = DojoEntitiesDataManager.currentAccount,
        };

        var transaction = await DojoCallsManager.CreateEventDojoCall(createRevenantsProps, endpoint);
    }
}
