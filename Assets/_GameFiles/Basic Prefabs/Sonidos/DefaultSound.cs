using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    public class DefaultSound : MonoBehaviour
    {
        public AudioSource dj;

        void Start()
        {
            dj = GetComponent<AudioSource>();
        }

        public virtual void SelfDespawn()
        {
            PoolManager.Despawn(gameObject);
        }

        public virtual void OnDespawn()
        {
            dj.Stop();
        }

        public virtual void OnSpawn()
        {
            Invoke("SelfDespawn", dj.clip.length);
            dj.Play();
        }
    }
}