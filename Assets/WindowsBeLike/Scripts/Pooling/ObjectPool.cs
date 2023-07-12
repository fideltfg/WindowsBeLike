using UnityEngine;
using System.Collections.Generic;

namespace Pooling
{
    /// <summary>
    /// Object pool 
    /// Writen By Leigh Edwards (aka FidelTFG)
    /// 2015
    /// 
    /// This class provides methods to pool gmaeobjects and resue them 
    /// </summary>
    /// 
    [System.Serializable]
    public class ObjectPool : List<PoolObject>
    {

        /// <summary>
        /// the pool name is taken from the sourceObject when the pool is created.
        /// </summary>
        public string name;

        /// <summary>
        /// referance copy of the object pooled here
        /// </summary>
        public GameObject PooledObject;

        /// <summary>
        /// how long objects in this pool should exist after being disabled.
        /// Set to 0 to prevent objects from this pool from been cleaned out
        /// </summary>
        public float poolTime = 0;


        /// <summary>
        /// Constructor for the object pool. This should not be called directly. 
        /// This is called from the pooler root when it needs to create a new object pool. 
        /// </summary>
        /// <param name="poolID">The index of the pool in the pooler.root.pools list</param>
        /// <param name="sourceObject">The GameObject to be pooled here</param>
        /// <param name="size">The size the pool should grow to on creation</param>
        /// <param name="poolTime">The time objects in this pool should persist without being utilized</param>
        public ObjectPool(int poolID, GameObject sourceObject, int size = 1, float poolTime = 60)
        {
            this.PooledObject = sourceObject;
            this.name = sourceObject.name;
            this.poolTime = poolTime;
            Grow(size);
        }


        /// <summary>
        /// method to get an object from this pool. if th epool is empty this method will add an 
        /// object to the pool (grow) and return it.
        /// </summary>
        /// <returns>GameObject</returns>
        public PoolObject Get()
        {
            // find an inactive object in this pool and return it
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] != null && !this[i].gameObject.activeInHierarchy)
                {
                    return this[i];
                }
            }
            // if there is no inactive objects in this pool grow the pool and return the new object
            return this.Grow();
        }

        /// <summary>
        /// this method grows the pool by one pooledObject and returns that object.
        /// </summary>
        /// <returns></returns>
        private PoolObject Grow()
        {
            GameObject newObject = GameObject.Instantiate<GameObject>(PooledObject); // instantiate a new object for this pool
            PoolObject poolObject = newObject.AddComponent<PoolObject>(); // add the poolObject component
            poolObject.poolTime = poolTime; // set the pool time ont he pooled object
           // newObject.transform.parent = Pooler.root.transform; // parent it to the pooler
            newObject.SetActive(false);// ensure its not active
            Add(poolObject); // add the new object to the pool
            return poolObject; // return the new object
        }

        /// <summary>
        /// method to grow this pool to the given size.
        /// If the pool is at or larger then the given size this method does nothing
        /// </summary>
        /// <param name="size"></param>
        private void Grow(int size)
        {
            if (this.Count < size)
            {
                for (int i = 0; i < (size - this.Count); i++)
                {
                    Grow();
                }
            }
        }

        /// <summary>
        /// method used to clean out un used pooled objects. this is called on each pool by the pool root 
        /// to reduce memory usage if objects are not being used for a long time
        /// The amount of time an object can exist unused in the pool is set via the poolTime value of the objct pool it is in.
        /// Set poolTime = 0 to prevent pool from cleaning itsself
        /// </summary>
        public void CleanUp()
        {
            if (poolTime > 0) // if the pooled objects have a time clean them
            {
                List<PoolObject> tempList = new List<PoolObject>(); // create a temp list

                foreach (PoolObject poolObject in this) // loop through the objects in this pool and call their clean up methods
                {
                    if (poolObject.CleanUp()) // check if the object has reached it max time in the pool as inactive
                    {
                        tempList.Add(poolObject); // add objects to be cleaned to the temp list
                    }
                }

                foreach (PoolObject g in tempList) // loop through the temp list and clean up objects 
                {
                    this.RemoveAt(this.IndexOf(g));
                    GameObject.Destroy(g.gameObject);
                }
            }
        }
    }
}