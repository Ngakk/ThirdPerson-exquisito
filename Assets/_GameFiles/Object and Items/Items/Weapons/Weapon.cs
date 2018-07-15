using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    public class Weapon : Interactuable
    {
        public Transform handle;

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
        }

        public virtual void OnWeaponDrop()
        {
            InteractRange.enabled = true;
        }
    }
}
