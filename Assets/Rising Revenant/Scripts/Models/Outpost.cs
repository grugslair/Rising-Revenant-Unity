using System;
using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using UnityEngine;

public class Outpost : ModelInstance
{
    public event Action OnValueChange;

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("position")]
    public RisingRevenantUtils.Vec2 position;

    [ModelField("owner")]
    public FieldElement ownerAddress;

    [ModelField("life")]
    public UInt32 life;

    [ModelField("reinforces_remaining")]
    public UInt32 reinforcesRemaining;

    [ModelField("reinforcement_type")]
    public RisingRevenantUtils.ReinforcementType reinforcementType;

    [ModelField("status")]
    public byte status;

    public bool selected = false;
    public bool isAttacked = false;
    public bool isSubbed = false;
    private FieldElement initalOwner;

    private void Start()
    {
        DojoEntitiesDataManager.outpostDictInstance.Add(position, this);
        initalOwner = ownerAddress;

        if (DojoEntitiesDataManager.currentAccount != null)
        {
            if (DojoEntitiesDataManager.currentAccount.Address.Hex() == ownerAddress.Hex())
            {
                DojoEntitiesDataManager.ownOutpostIndex.Add(position);
            }
        }

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
        gameObject.transform.position = new Vector3(position.x, 0.1f, position.y);

        gameObject.AddComponent<BoxCollider>();
        gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        SetOutpostTexture();
    }

    Mesh CreatePlaneMesh()
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Mesh mesh = plane.GetComponent<MeshFilter>().mesh;

        plane.transform.localScale = new Vector3(4, 4, 4);

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
        if (life == 0)
        {
            outpostTexture = Resources.Load<Texture>(AssetsManager.ToTextureOutputString(AssetsManager.OutpostColorOption.DEAD_OUTPOST));
            renderer.material.mainTexture = outpostTexture;

            outpostMat = Resources.Load<Material>(AssetsManager.ToMaterialOutputString(AssetsManager.OutpostColorOption.DEAD_OUTPOST));
            renderer.material = outpostMat;
            return;
        }

        //check if mine
        //check if your

        if (DojoEntitiesDataManager.currentAccount == null)
        {
            outpostTexture = Resources.Load<Texture>(AssetsManager.ToTextureOutputString(AssetsManager.OutpostColorOption.ENEMY_OUTPOST));
            renderer.material.mainTexture = outpostTexture;

            outpostMat = Resources.Load<Material>(AssetsManager.ToMaterialOutputString(AssetsManager.OutpostColorOption.ENEMY_OUTPOST));
            renderer.material = outpostMat;
            return;
        }


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

        if (attacked == true && life > 0)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer == null) { return; }

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

        if (initalOwner != null)
        {
            if (initalOwner.Hex() != ownerAddress.Hex())
            {
                initalOwner = ownerAddress;
                Debug.Log("something chaged");

                if (DojoEntitiesDataManager.ownOutpostIndex.Contains(position))
                {
                    DojoEntitiesDataManager.ownOutpostIndex.Remove(position);
                }
                else
                {
                    DojoEntitiesDataManager.ownOutpostIndex.Add(position);
                }

                SetOutpostTexture();
            }
        }
       

        OnValueChange?.Invoke();
    }
}
