using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    [RequireComponent(typeof(BoxCollider))]
    public class Hitbox : MangosBehaviour
    {
        public float m_lifetime;
        BoxCollider m_collider;

        private void Start()
        {
            m_collider = GetComponent<BoxCollider>();
            m_collider.isTrigger = true;
            m_collider.enabled = false;
        }

        public void TurnOn()
        {
            m_collider.enabled = true;
            Invoke("TurnOff", m_lifetime);
        }

        public void TurnOff()
        {
            CancelInvoke(); 
            m_collider.enabled = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Weapon"))
                other.gameObject.SendMessage("GetHit", SendMessageOptions.DontRequireReceiver);
        }
    }
}
