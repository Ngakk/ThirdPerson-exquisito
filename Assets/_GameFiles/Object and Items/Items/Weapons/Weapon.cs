using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    public class Weapon : Interactuable
    {
        public override void OnUse()
        {
            base.OnUse();
            
        }

        public override void OnInteract()
        {

        }

        public virtual void OnWeaponSet()
        {
            InteractRange.enabled = false;
            rigi.isKinematic = true;
            Collider[] colls = GetComponentsInChildren<Collider>();
            for(int i = 0; i < colls.Length; i++)
            {
                colls[i].isTrigger = true;
            }
        }

        public virtual void OnWeaponDrop()
        {
            InteractRange.enabled = true;
            rigi.isKinematic = false;
            Collider[] colls = GetComponentsInChildren<Collider>();
            for (int i = 0; i < colls.Length; i++)
            {
                colls[i].isTrigger = false;
            }
            InteractRange.isTrigger = true;
        }
    }
}
