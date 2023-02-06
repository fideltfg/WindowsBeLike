using UnityEngine;

namespace Pooling
{
    /// <summary>
    /// componet applied automaticly to pooled objects.
    /// This is used by the pooler while cleaning up the pools.
    /// 
    /// writen by Leigh Edwards (aka FidelTFG) 2015
    /// </summary>
    public class PoolObject : MonoBehaviour
    {
        [HideInInspector]
        public float timeOfDeath = 0f; // the time this object was disabled
        //[HideInInspector]
        public float poolTime = 0f; // how long the object should be kept in the pool after it has been disabled

        private void OnDisable()
        {
            timeOfDeath = Time.time;
        }

        public bool CleanUp()
        {
            //Debug.Log("Pooled object cleaned.");
            if (this.gameObject != null && this.gameObject.activeInHierarchy)
            {
                return false;
            }
            if (timeOfDeath + poolTime < Time.time)
            {
                timeOfDeath = 0;
                return true;
            }
            return false;
        }
    }
}
