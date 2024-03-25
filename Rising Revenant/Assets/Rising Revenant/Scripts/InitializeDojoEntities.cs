using System.Collections.Generic;
using Dojo;
using Dojo.Starknet;
using UnityEngine;

public class InitializeDojoEntities : MonoBehaviour
{
    public WorldManager worldManager;

    public BurnerManager burnerManager;
    private Dictionary<FieldElement, string> spawnedBurners = new();

    //public string testPubK = "0x4c339f18b9d1b95b64a6d378abd1480b2e0d5d5bd33cd0828cbce4d65c27284";
    //public string testPrivK = "0x1c9053c053edf324aec366a34c6901b1095b07af69495bffec7d7fe21effb1b";
    //public string testAddress = "0x6b86e40118f29ebe393a75469b4d926c7a44c2e2681b6d319520b7c1156d114";

    public JsonRpcClient provider;

    void Awake()
    {
        provider = new JsonRpcClient(DojoCallsManager.katanaEndpoint);
        var signer = new SigningKey(DojoCallsManager.masterPrivateKey);
        var account = new Account(provider, signer, new FieldElement(DojoCallsManager.masterAddress));

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

    //public Account GenerateAccount() 
    //{
    //    var signer = new SigningKey(testPrivK);
    //    return new Account(provider, signer, new FieldElement(testAddress));
    //}

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
                addressOfSystem = DojoCallsManager.eventActionsAddress,
                account = DojoEntitiesDataManager.currentAccount,
            };

            var transaction = await DojoCallsManager.CreateEventDojoCall(createRevenantsProps, endpoint);
    }
}
