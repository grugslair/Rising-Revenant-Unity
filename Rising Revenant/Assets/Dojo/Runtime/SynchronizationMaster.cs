using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dojo.Starknet;
using Dojo.Torii;
using dojo_bindings;
using UnityEngine;
using UnityEngine.Events;

namespace Dojo
{
    public class SynchronizationMaster : MonoBehaviour
    {
        public WorldManager worldManager;

        // Maximum number of entities to synchronize
        public uint limit = 100;

        // Handle entities that get synchronized
        private ModelInstance[] _models;
        // Returns all of the model definitions
        private ModelInstance[] models => _models ??= GetComponents<ModelInstance>();

        public UnityEvent<List<GameObject>> OnSynchronized;
        public UnityEvent<GameObject> OnEntitySpawned;


        private HashSet<string> modelsNotToLoadSet = new HashSet<string>(new[] { "OutpostPosition", "ReinforcementBalance", "TradeReinforcement", "TradeRevenant", "WorldEventTracker" });

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            // We don't want our model definitions to be active.
            // Only used as templates for the actual entities to use.
            foreach (var model in models)
            {
                model.enabled = false;
            }
        }

        // Fetch all entities from the dojo world and spawn them.
#if UNITY_WEBGL && !UNITY_EDITOR
        public async Task<int> SynchronizeEntities()
#else
        public int SynchronizeEntities()
#endif
        {

            //this creates the query for everything
//           var query = new dojo.Query
//           {
//               clause = new dojo.COptionClause
//               {
//                   tag = dojo.COptionClause_Tag.NoneClause,
//               },
//               limit = limit,
//           };


//            //this actually does the query
//#if UNITY_WEBGL && !UNITY_EDITOR
//            var entities = await worldManager.wasmClient.Entities((int)limit, 0);
//#else
//           var entities = worldManager.toriiClient.Entities(query);
//#endif

            //Debug.Log(query.ToString());
            //Debug.Log(entities.Count());



            //var testQuery = new dojo.Query
            //{
            //    clause = new dojo.COptionClause
            //    {
            //        tag = dojo.COptionClause_Tag.SomeClause,
            //        some = new dojo.Clause
            //        {
            //            tag = dojo.Clause_Tag.CMember,
            //            c_member = new dojo.MemberClause
            //            {
            //                member = "game_id",
            //                model = "Outpost",
            //                operator_ = dojo.ComparisonOperator.Eq,
            //                value = new dojo.Value
            //                {
            //                    primitive_type = new dojo.Primitive
            //                    {
            //                        tag = dojo.Primitive_Tag.U32,
            //                        u32 = 1,
            //                    },
            //                    value_type = new dojo.ValueType
            //                    {
            //                        tag = dojo.ValueType_Tag.UInt,
            //                        u_int = 1,
            //                    }
            //                }
            //            }
            //        }
            //    },
            //};
            //var testEntities = worldManager.toriiClient.Entities(testQuery);

            //Debug.Log(testEntities.Count());
            //Debug.Log(testQuery.ToString());

            //var entityGameObjects = new List<GameObject>();
            ////for every entity add a game object and such
            //foreach (var entity in entities)
            //{
            //    entityGameObjects.Add(SpawnEntity(entity.HashedKeys, entity.Models.Values.ToArray()));
            //}

            ////and then do something
            //OnSynchronized?.Invoke(entityGameObjects);
            return 1;
        }

        //HERE make the gameconfig on the cairo end not 1 and another high impossible number
        public bool CallFromLoadingPage(Account account)
        {
            var entityGameObjects = new List<GameObject>();

            //the first query crashes the unity engine
            /*
            byte[] valueToSet = new byte[16];
            valueToSet[15] = 1;
            // this can be 0 depending on endian

            var queryForGameCounter = new dojo.Query
            {
                clause = new dojo.COptionClause
                {
                    tag = dojo.COptionClause_Tag.SomeClause,
                    some = new dojo.Clause
                    {
                        tag = dojo.Clause_Tag.CMember,
                        c_member = new dojo.MemberClause
                        {
                            member = "entity_id",
                            model = "GameCountTracker",
                            operator_ = dojo.ComparisonOperator.Eq,
                            value = new dojo.Value
                            {
                                primitive_type = new dojo.Primitive
                                {
                                    tag = dojo.Primitive_Tag.U128,
                                    u128 = valueToSet,
                                },
                                value_type = new dojo.ValueType
                                {
                                    tag = dojo.ValueType_Tag.Bytes,
                                    bytes = valueToSet,
                                }
                            }
                        }
                    }
                },
            };

            var entities = worldManager.toriiClient.Entities(queryForGameCounter);
            foreach (var entity in entities)
            {
                entityGameObjects.Add(SpawnEntity(entity.HashedKeys, entity.Models.Values.ToArray()));
            }
            */

            UInt32 gameID = 1;  // this is for test right now

            var queryForGameData = new dojo.Query
            {
                clause = new dojo.COptionClause
                {
                    tag = dojo.COptionClause_Tag.SomeClause,
                    some = new dojo.Clause
                    {
                        tag = dojo.Clause_Tag.CMember,
                        c_member = new dojo.MemberClause
                        {
                            member = "game_id",
                            model = "Game",
                            operator_ = dojo.ComparisonOperator.Eq,
                            value = new dojo.Value
                            {
                                primitive_type = new dojo.Primitive
                                {
                                    tag = dojo.Primitive_Tag.U32,
                                    u32 = gameID,
                                },
                                value_type = new dojo.ValueType
                                {
                                    tag = dojo.ValueType_Tag.UInt,
                                    u_int = gameID,
                                }
                            }
                        }
                    }
                },
            };
            var entities = worldManager.toriiClient.Entities(queryForGameData);
            foreach (var entity in entities)
            {
                entityGameObjects.Add(SpawnEntity(entity.HashedKeys, entity.Models.Values.ToArray()));
            }

            //var queryForWorldEventEntities = new dojo.Query
            //{
            //    clause = new dojo.COptionClause
            //    {
            //        tag = dojo.COptionClause_Tag.SomeClause,
            //        some = new dojo.Clause
            //        {
            //            tag = dojo.Clause_Tag.CMember,
            //            c_member = new dojo.MemberClause
            //            {
            //                member = "game_id",
            //                model = "WorldEvent",
            //                operator_ = dojo.ComparisonOperator.Eq,
            //                value = new dojo.Value
            //                {
            //                    primitive_type = new dojo.Primitive
            //                    {
            //                        tag = dojo.Primitive_Tag.U32,
            //                        u32 = gameID,
            //                    },
            //                    value_type = new dojo.ValueType
            //                    {
            //                        tag = dojo.ValueType_Tag.UInt,
            //                        u_int = gameID,
            //                    }
            //                }
            //            }
            //        }
            //    },
            //};
            //entities = worldManager.toriiClient.Entities(queryForWorldEventEntities);
            //foreach (var entity in entities)
            //{
            //    entityGameObjects.Add(SpawnEntity(entity.HashedKeys, entity.Models.Values.ToArray()));
            //}

            Debug.Log($"i am loading in {entities.Count} of events");

            // this theoretically does events too
            var queryForGameWorldEntities = new dojo.Query
            {
                clause = new dojo.COptionClause
                {
                    tag = dojo.COptionClause_Tag.SomeClause,
                    some = new dojo.Clause
                    {
                        tag = dojo.Clause_Tag.CMember,
                        c_member = new dojo.MemberClause
                        {
                            member = "game_id",
                            model = "Outpost",
                            operator_ = dojo.ComparisonOperator.Eq,
                            value = new dojo.Value
                            {
                                primitive_type = new dojo.Primitive
                                {
                                    tag = dojo.Primitive_Tag.U32,
                                    u32 = gameID,
                                },
                                value_type = new dojo.ValueType
                                {
                                    tag = dojo.ValueType_Tag.UInt,
                                    u_int = gameID,
                                }
                            }
                        }
                    }
                },
            };
            entities = worldManager.toriiClient.Entities(queryForGameWorldEntities);
            foreach (var entity in entities)
            {
                entityGameObjects.Add(SpawnEntity(entity.HashedKeys, entity.Models.Values.ToArray()));
            }

            Debug.Log($"i am loading in {entities.Count} of entities");





            //var queryForPlayerInfo = new dojo.Query
            //{
            //    clause = new dojo.COptionClause
            //    {
            //        tag = dojo.COptionClause_Tag.SomeClause,
            //        some = new dojo.Clause
            //        {
            //            tag = dojo.Clause_Tag.CMember,
            //            c_member = new dojo.MemberClause
            //            {
            //                member = "owner",
            //                model = "PlayerInfo",
            //                operator_ = dojo.ComparisonOperator.Eq,
            //                value = new dojo.Value
            //                {
            //                    primitive_type = new dojo.Primitive
            //                    {
            //                        tag = dojo.Primitive_Tag.ContractAddress,
            //                        contract_address = account.Address.Inner(),
            //                    },
            //                    value_type = new dojo.ValueType
            //                    {
            //                        tag = dojo.ValueType_Tag.String,

            //                    }
            //                }
            //            }
            //        }
            //    },
            //};
            //entities = worldManager.toriiClient.Entities(queryForPlayerInfo);
            //foreach (var entity in entities)
            //{
            //    entityGameObjects.Add(SpawnEntity(entity.HashedKeys, entity.Models.Values.ToArray()));
            //}




            OnSynchronized?.Invoke(entityGameObjects);

            return true;
        }



        // Spawn an Entity game object from a dojo.Entity
        private GameObject SpawnEntity(FieldElement hashedKeys, Model[] entityModels)
        {
            // Add the entity to the world.
            // HERE  we dont want an empty gameobject if there are no other things inside as they have been discarde
            // fun fact, default is better when assinging vars that are yet to be used
            GameObject entityGameObject = default;
            bool entityAdded = false;
            // also surely this can be parallized

            // Initialize each one of the entity models


            //HERE
            foreach (var entityModel in entityModels)
            {
                // Check if we have a model definition for this entity model
                var model = models.FirstOrDefault(m => m.GetType().Name == entityModel.Name);
                if (model == null )
                {
                    Debug.LogError($"Model {entityModel.Name} not found");
                    continue;
                }
                else if (modelsNotToLoadSet.Contains(entityModel.Name))
                {
                    //Debug.LogError($"Model {entityModel.Name} has been discarded");
                    continue;
                }
                // this is how we check for the model name
                if (!entityAdded)
                {
                   var ent = worldManager.Entity(hashedKeys.Hex());

                   if (ent == null)
                   {
                       entityGameObject = worldManager.AddEntity(hashedKeys.Hex());
                   }
                    else
                    {
                        //here we need to check if the model we are adding is already there if it si skip
                        Component[] components = ent.GetComponents<Component>();

                        foreach (Component things in components)
                        {
                            if (things.GetType().Name == entityModel.Name) {
                                continue;
                            }
                        }
                    }

                   entityAdded = true;
                }
                // Add the model component to the entity

                if(entityGameObject != null)
                {
                    var component = (ModelInstance)entityGameObject.AddComponent(model.GetType());
                    component.Initialize(entityModel);
                }
                
            }

            OnEntitySpawned?.Invoke(entityGameObject);
            return entityGameObject;
        }

        // Handles spawning / updating entities as they are updated from the dojo world
        private void HandleEntityUpdate(FieldElement hashedKeys, Model[] entityModels)
        {
            // Get the entity game object
            var entity = GameObject.Find(hashedKeys.Hex());
            if (entity == null)
            {
                // should we fetch the entity here?
                entity = SpawnEntity(hashedKeys, entityModels);
            }

            // Update each one of the entity models
            foreach (var entityModel in entityModels)
            {
                var component = entity.GetComponent(entityModel.Name);
                if (component == null)
                {
                    // TODO: decouple?
                    var model = models.FirstOrDefault(m => m.GetType().Name == entityModel.Name);
                    if (model == null)
                    {
                        Debug.LogError($"Model {entityModel.Name} not found");
                        continue;
                    }
                    else if (modelsNotToLoadSet.Contains(entityModel.Name))
                    {
                        //Debug.LogError($"Model {entityModel.Name} has been discarded");
                        continue;
                    }

                    // we dont need to initialize the component
                    // because it'll get updated
                    component = (ModelInstance)entity.AddComponent(model.GetType());
                }

                // update component with new model data
                ((ModelInstance)component).OnUpdate(entityModel);
            }
        }

        // Register our entity callbacks
        public void RegisterEntityCallbacks()
        {
            ToriiEvents.Instance.OnEntityUpdated += HandleEntityUpdate;
        }
    }
}