using System;
using System.Collections.Generic;
using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using UnityEngine;
using static UnityEditor.Progress;


public class Outpost : ModelInstance
{

    public event Action OnValueChange;

    [ModelField("game_id")]
    public UInt32 gameId;
    [ModelField("entity_id")]
    public FieldElement entityId;
    [ModelField("owner")]
    public FieldElement ownerAddress;
    [ModelField("name_outpost")]
    public FieldElement nameOutpost;
    [ModelField("x")]
    public UInt32 xPosition;
    [ModelField("y")]
    public UInt32 yPosition;
    [ModelField("lifes")]
    public UInt32 lifes;
    [ModelField("shield")]
    public byte shield; 
    [ModelField("reinforcement_count")]
    public UInt32 reinforcementCount;
    [ModelField("status")]
    public UInt32 status;
    [ModelField("last_affect_event_id")]
    public FieldElement lastAffectEventId;

    public bool selected = false;
    public bool isAttacked = false;

    private void Start()
    {
        DojoEntitiesDataManager.outpostDictInstance.Add(RisingRevenantUtils.FieldElementToInt(entityId), this);

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        meshFilter.mesh = CreatePlaneMesh();

        gameObject.layer = 6;

        gameObject.transform.position = new Vector3(xPosition, 0.1f, yPosition);

        SetOutpostTexture();

        gameObject.AddComponent<BoxCollider>();
    }

    Mesh CreatePlaneMesh()
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Mesh mesh = plane.GetComponent<MeshFilter>().mesh;

        plane.transform.localScale = new Vector3(4,4,4);

        Destroy(plane);

        return mesh;
    }

    public void SetOutpostTexture()
    {
        var renderer = gameObject.GetComponent<Renderer>();
        if (renderer == null) { return; }

        Texture outpostTexture;
        Material outpostMat;

        //check if selected
        if (selected)
        {
            outpostTexture = Resources.Load<Texture>(AssetsManager.ToTextureOutputString(AssetsManager.OutpostColorOption.SELECTED_OUTPOST));
            renderer.material.mainTexture = outpostTexture;

            outpostMat = Resources.Load<Material>(AssetsManager.ToMaterialOutputString(AssetsManager.OutpostColorOption.SELECTED_OUTPOST));
            renderer.material = outpostMat;
            return;
        }

        //check if dead
        if (lifes == 0)
        {
            outpostTexture = Resources.Load<Texture>(AssetsManager.ToTextureOutputString(AssetsManager.OutpostColorOption.DEAD_OUTPOST));
            renderer.material.mainTexture = outpostTexture;

            outpostMat = Resources.Load<Material>(AssetsManager.ToMaterialOutputString(AssetsManager.OutpostColorOption.DEAD_OUTPOST));
            renderer.material = outpostMat;
            return;
        }


        //get event
        //if (DojoEntitiesDataManager.gameEntityCounterInstance.eventCount != 0)
        //{
        //    var currentEvent = DojoEntitiesDataManager.worldEventDictInstance[(int)DojoEntitiesDataManager.gameEntityCounterInstance.eventCount];

        //    if (RisingRevenantUtils.IsPointInsideCircle(new Vector2(currentEvent.xPosition, currentEvent.yPosition), currentEvent.radius, new Vector2(xPosition, yPosition)))
        //    {
        //        outpostTexture = Resources.Load<Texture>(AssetsManager.ToTextureOutputString(AssetsManager.OutpostColorOption.ATTACKED_OUTPOST));
        //        renderer.material.mainTexture = outpostTexture;

        //        outpostMat = Resources.Load<Material>(AssetsManager.ToMaterialOutputString(AssetsManager.OutpostColorOption.ATTACKED_OUTPOST));
        //        renderer.material = outpostMat;
        //        return;
        //    }
        //}

        //check if mine
        //check if your



        if (DojoEntitiesDataManager.currentAccount.Address.Hex() == ownerAddress.Hex())
        {
            outpostTexture = Resources.Load<Texture>(AssetsManager.ToTextureOutputString(AssetsManager.OutpostColorOption.OWN_OUTPOST));
            renderer.material.mainTexture = outpostTexture;

            outpostMat = Resources.Load<Material>(AssetsManager.ToMaterialOutputString(AssetsManager.OutpostColorOption.OWN_OUTPOST));
            renderer.material = outpostMat;
            return;
        }
        else
        {
            outpostTexture = Resources.Load<Texture>(AssetsManager.ToTextureOutputString(AssetsManager.OutpostColorOption.ENEMY_OUTPOST));
            renderer.material.mainTexture = outpostTexture;

            outpostMat = Resources.Load<Material>(AssetsManager.ToMaterialOutputString(AssetsManager.OutpostColorOption.ENEMY_OUTPOST));
            renderer.material = outpostMat;

            return;
        }
    }


    

    public void SetAttackState(bool attacked)
    {
        isAttacked = attacked;

        if (attacked == true && lifes > 0)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer == null) { return; }

            Material outpostMat;

            renderer.material.mainTexture = Resources.Load<Texture>(AssetsManager.ToTextureOutputString(AssetsManager.OutpostColorOption.ATTACKED_OUTPOST)); ;

            renderer.material = Resources.Load<Material>(AssetsManager.ToMaterialOutputString(AssetsManager.OutpostColorOption.ATTACKED_OUTPOST));
        }
        else
        {
            SetOutpostTexture();
        }


        OnValueChange?.Invoke();
    }

    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);
        OnValueChange?.Invoke();
    }
}
