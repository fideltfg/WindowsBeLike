using UnityEngine;
using System.Collections.Generic;

namespace Pooling
{
    /// <summary>
    /// add on to pooling package that provides methods to pool and reuse audio clips
    /// writen by Leigh Edwards (aka FidelTFG) 2016
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundPool : MonoBehaviour
    {
        public List<AudioClip> poolList;
        public AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        /// <summary>
        /// method to return a random audio clip from the list
        /// but within the given range of eliments
        /// </summary>
        public AudioClip GetRandomInRange(int a, int b)
        {
            if (poolList.Count > 0)
            {
                return poolList[Random.Range(a, b)];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// method to return a random sound from the list
        /// </summary>
        public AudioClip GetRandom()
        {
            if (poolList.Count > 0)
            {
                return poolList[Random.Range(0, poolList.Count - 1)];
            }
            else
            {
                Debug.Log("no sound to player");
                return null;
            }
        }

        public void PlayPooledClip(int poolIndex)
        {
            if (poolIndex < poolList.Count && poolList[poolIndex] != null)
            {
                if (source.enabled)
                {
                    source.PlayOneShot(poolList[poolIndex]);
                }
            }
        }

        /// <summary>
        /// will play the selected clip on a loop.
        /// </summary>
        /// <param name="poolIndex"></param>
        public void LoopPooledClip(int poolIndex)
        {
            source.clip = poolList[poolIndex];
            source.loop = true;
            source.Play();
        }

        public void PlayRandom()
        {
            if (poolList.Count > 0)
            {
                source.PlayOneShot(GetRandom());
            }
        }

        public void PlayRandomAtPoint(Vector3 point)
        {
            AudioSource.PlayClipAtPoint(GetRandom(), point);
        }

        public void PlayRandomInRange(int a, int b)
        {
            if (poolList.Count > 0)
            {
                if ( b < poolList.Count && a >= 0 )
                {
                    source.PlayOneShot(GetRandomInRange(a, b));
                }
                else
                {
                    Debug.Log("requested sound index was out of range for this pool");
                }
            }
            else
            {
                Debug.Log("poolLost is empty");
            }
        }
    }
}