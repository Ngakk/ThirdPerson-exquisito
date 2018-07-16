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
        }

        public virtual void OnWeaponDrop()
        {
            InteractRange.enabled = true;
        }
    }
}
