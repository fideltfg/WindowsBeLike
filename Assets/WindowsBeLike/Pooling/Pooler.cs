using System;
using System.Collections.Generic;
using UnityEngine;


namespace Pooling
{
    /// <summary>
    /// Pooler
    /// Writen By Leigh Edwards (aka FidelTFG)
    /// 2015
    /// 
    /// Add this component anyplace in your scene. 
    /// 
    /// Designed to take the pain out of pooling,
    /// Automaticaly takes care of its self.
    /// 
    /// 
    /// 
    /// </summary>
    public class Pooler : MonoBehaviour
    {

        /// <summary>
        /// This is where all pools are held. the root pool is simply a pool of pools
        /// </summary>
        public static Pooler root = null;

        /// <summary>
        /// A referance set of objects that are currently pooled.
        /// </summary>
        private List<GameObject> pooledObjects = new List<GameObject>(); // a list of the objects that are currently pooled here

        /// <summary>
        ///  this is the list of all the pools in the root pool
        /// </summary>
        public List<ObjectPool> pools = new List<ObjectPool>();// list of the pools

        /// <summary>
        /// Singletone!!! stuff
        /// </summary>
        void Awake()
        {
            if (root != null)
            {
                GameObject.Destroy(gameObject);
            }
            else
            {
                root = this;
            }
        }

        void FixedUpdate()
        {
            // run pooled object clean up!!
            // this removes un-utalized pooled objects from there respective pools
            List<ObjectPool> tempList = new List<ObjectPool>();
            foreach (ObjectPool objectPool in pools)
            {
                objectPool.CleanUp();
                if (objectPool.Count == 0)
                {
                    tempList.Add(objectPool);
                }
            }
            foreach (ObjectPool op in tempList)
            {
                pooledObjects.Remove(op.PooledObject);
                pools.Remove(op);
            }
        }

        /// <summary>
        /// method to reset the pooler.
        /// This method clears out and destroys all pooled objects, both Active and inactive.
        /// Then it destroys all pools.
        /// </summary>
        public void Reset()
        {
            List<GameObject> tempList = new List<GameObject>();
            foreach (ObjectPool op in pools)
            {
                foreach (PoolObject g in op)
                {
                    tempList.Add(g.gameObject);
                }
            }

            foreach (GameObject go in tempList)
            {
                GameObject.Destroy(go);
            }

            pooledObjects = new List<GameObject>();
            pools = new List<ObjectPool>();
        }

        /// <summary>
        /// method to disable all active pooled bojects in the given pool (ID)
        /// </summary>
        /// <param name="poolID">ID of the pool to be cleard</param>
        public void ClearPool(int poolID)
        {
            foreach (PoolObject go in pools[poolID])
            {
                go.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// method to diable all objects in the given objects pool
        /// </summary>
        /// <param name="sourceObject"></param>
        public void ClearPool(GameObject sourceObject)
        {
            if (pooledObjects != null)
            {
                int poolID = pooledObjects.IndexOf(sourceObject);
                if (poolID > -1)
                {
                    ClearPool(poolID);
                }
            }
        }

        /// <summary>
        /// method to create a new pool
        /// </summary>
        /// <param name="sourceObject">The object you want to make a pool of</param>
        /// <returns>the created object pool or if the pool already exists the current pool. 
        /// Note ifpool already exits it may or may not be empty. Check pool count before accesing.
        /// </returns>
        public ObjectPool AddPool(GameObject sourceObject, int count = 1)
        {
            // the return value
            int newPoolIndex = -1;

            // make sure we have a pooled objects list
            if (pooledObjects != null)
            {
                // check if the source object already exists in the pooled objects list
                newPoolIndex = pooledObjects.IndexOf(sourceObject);

                // < 0 indicates the source object is not in the pooled objects list
                if (newPoolIndex < 0)
                {
                    pooledObjects.Add(sourceObject);// add the object  to the pooled objects list and get its index
                    int i = pooledObjects.IndexOf(sourceObject); // get the index of the new pooled object
                    ObjectPool newObjectPool = new ObjectPool(i, pooledObjects[i], count); // create the new pool
                    pools.Add(newObjectPool); // add the new pool to the list of pools
                    return newObjectPool; // return the new pool
                }
                else
                {
                    return pools[newPoolIndex]; // return the existing pool
                }
            }
            else
            {
                Debug.LogWarning("Pool Root object is null. Can not add or create a new pool for null objects!");
                return null;
            }
        }

        /// <summary>
        /// method to return the number of the object that are currently pooled
        /// </summary>
        /// <param name="sourceObject">Object to count</param>
        /// <param name="active">set true to return the count of the active objects only</param>
        /// <returns></returns>
        public int GetPooledInstanceCount(GameObject sourceObject, bool active = true)
        {
            int poolIndex = pooledObjects.IndexOf(sourceObject);
            return GetPooledInstanceCount(poolIndex, active);
        }


        /// <summary>
        /// method to return the number of the object that are currently pooled
        /// </summary>
        /// <param name="poolIndex">Object to count</param>
        /// <param name="active">set true to return the count of the active objects only</param>
        /// <returns></returns>
        public int GetPooledInstanceCount(int poolIndex, bool active = true)
        {
            // < 0 indicates the source object is not in the pooled objects list
            if (poolIndex >= 0 && pools.Count >= poolIndex)
            {
                if (active)
                {
                    ObjectPool pool = pools[poolIndex];
                    int count = 0;
                    foreach (PoolObject g in pool)
                    {
                        if (g.gameObject.activeInHierarchy)
                        {
                            count++;
                        }
                    }
                    return count;
                }
                else
                {
                    return pools[poolIndex].Count;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Method to return an Instance of the given object from the pool.
        /// if the gameObject is not already pooled a new pool is created and 
        /// an Instance is added then retured
        /// </summary>
        /// <param name="sourceObject">The object to get from the pool</param>
        /// <returns>An Instance of the given object from the pool.</returns>
        public GameObject GetPooledInstance(GameObject sourceObject)
        {
            int poolID = -1;
            return GetPooledInstance(sourceObject, out poolID);
        }

        /// <summary>
        /// Method to return an Instance of the given object from the pool.
        /// if the gameObject is not already pooled a new pool is created and 
        /// an Instance is added then retured
        /// </summary>
        /// <param name="poolName">The name of the pool, or object pooled there (sourceObject name)</param>
        /// <returns></returns>
        public GameObject GetPooledInstance(string poolName)
        {
            foreach (ObjectPool pool in pools)
            {
                if (pool.name == poolName)
                {
                    return pool.Get().gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Method to return an Instance of the given object from the pool.
        /// if the gameObject is not already pooled a new pool is created and 
        /// an Instance is added then retured
        /// </summary>
        /// <param name="sourceObject">The object to get from the pool</param>
        /// <param name="poolID">OUT param to return the pool ID for the pooled object</param>
        /// <returns>An Instance of the given object from the pool.</returns>
        public GameObject GetPooledInstance(GameObject sourceObject, out int poolID)
        {
            // get the pool ID for the object we want (sourceObject)
            poolID = pooledObjects.IndexOf(sourceObject);
            // if the pool ID is <0 there is no pool for this object
            if (poolID < 0)
            {
                // create a new pool
                ObjectPool op = AddPool(sourceObject);
                return op.Get().gameObject;
            }
            return GetPooledInstance(poolID);
        }

        /// <summary>
        /// Method to return an Instance of the given object from the pool.
        /// if the gameObject is not already pooled a new pool is created and 
        /// an Instance is added then retured
        /// </summary>
        /// <param name="poolID"></param>
        /// <returns>GameObject from the pool with the given ID. Returns null if pool ID does not exist</returns>
        public GameObject GetPooledInstance(int poolID)
        {
            if (pools.Count >= poolID)
            {
                return pools[poolID].Get().gameObject;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// method to return the pool for the given source object
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <returns>ObjectPool else null if no pool found</returns>
        public ObjectPool GetPool(GameObject sourceObject)
        {
            return GetPool(sourceObject.name);
        }

        /// <summary>
        /// method to return the pool with the given ID
        /// </summary>
        /// <param name="poolID"></param>
        /// <returns>ObjectPool else null if no pool found</returns>
        public ObjectPool GetPool(int poolID)
        {
            if (pools.Count >= poolID)
            {
                return pools[poolID];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// method to return the pool holding objects with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>ObjectPool else null if no pool found</returns>
        public ObjectPool GetPool(string name)
        {
            foreach (ObjectPool pool in pools)
            {
                if (pool.name == name)
                {
                    return pool;
                }
            }
            return null;
        }


        /// <summary>
        /// method to return the first active object in the given pool(ID).
        /// </summary>
        /// <param name="poolID"></param>
        /// <returns>GameObject else null if no actve object found</returns>
        public GameObject GetPoolLeader(int poolID)
        {
            if (pools[poolID].Count > 0)
            {
                for (int i = 0; i < pools[poolID].Count; i++)
                {
                    if (pools[poolID][i].gameObject.activeInHierarchy)
                    {
                        return pools[poolID][i].gameObject;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// method to return the first active object of the type given
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <returns>GameObject else null if no actve object found</returns>
        public GameObject GetPoolLeader(GameObject sourceObject)
        {
            // get the pool ID for the object we want (sourceObject)
            int poolID = pooledObjects.IndexOf(sourceObject);
            // if the pool ID is <0 there is no pool for this object
            if (poolID > -1)
            {
                return GetPoolLeader(poolID);
            }
            return null;
        }
    }
}