using System.Collections.Generic;
using System.Linq;
using Dojo;
using Dojo.Starknet;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] WorldManager worldManager;
    [SerializeField] ChatManager chatManager;

    [SerializeField] WorldManagerData dojoConfig;
    [SerializeField] GameManagerData gameManagerData; 

    private BurnerManager burnerManager;
    private Dictionary<FieldElement, string> spawnedBurners = new();

    
    void Start()
    {
        var provider = new JsonRpcClient(dojoConfig.rpcUrl);
        var signer = new SigningKey(gameManagerData.masterPrivateKey);
        var account = new Account(provider, signer, new FieldElement(gameManagerData.masterAddress));

        Debug.Log(account.Address);

        burnerManager = new BurnerManager(provider, account);

        worldManager.synchronizationMaster.OnEntitySpawned.AddListener(InitEntity);
        foreach (var entity in worldManager.Entities())
        {
            InitEntity(entity);
        }
    }

    async void Update()
    {
        // dont register inputs if our chat is open
        if (chatManager.chatOpen) return;

    }

    private void InitEntity(GameObject entity)
    {
        var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        // change color of capsule to a random color
        capsule.GetComponent<Renderer>().material.color = Random.ColorHSV();
        capsule.transform.parent = entity.transform;

        foreach (var burner in spawnedBurners)
        {
            if (burner.Value == null)
            {
                spawnedBurners[burner.Key] = entity.name;
                break;
            }
        }
    }
}