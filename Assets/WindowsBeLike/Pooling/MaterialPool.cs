using UnityEngine;
using System.Collections.Generic;


namespace Pooling
{
    /// <summary>
    /// add on to pooling package that provides method to pool and reuse materials
    /// writen by Leigh Edwards (aka FidelTFG) 2016
    /// </summary>

    [System.Serializable]
    public class MaterialPool
    {
        public List<Material> pool;

        public Material GetMat(string name)
        {
            if (pool != null && pool.Count > 0 && name != null && name != "")
            {
                foreach (Material mat in pool)
                {
                    if (mat.name == name)
                    {
                        return mat;
                    }
                }
            }
            return null;
        }

        public Material GetMat(int index)
        {
            if (pool != null && pool.Count > 0 && index >= 0 && index < pool.Count)
            {
                return pool[index];
            }
            return null;
        }

        public Texture GetTex(string name)
        {
            if (pool != null && pool.Count > 0 && name != null && name != "")
            {
                foreach (Material mat in pool)
                {
                    if (mat.name == name)
                    {
                        return mat.mainTexture;
                    }
                }
            }
            return null;
        }
        public Texture GetTex(int index)
        {
            if (pool != null && pool.Count > 0 && index >= 0 && index < pool.Count)
            {
                return pool[index].mainTexture;
            }
            return null;
        }
    }
}