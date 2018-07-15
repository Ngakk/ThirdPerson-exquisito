using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    [RequireComponent(typeof (SphereCollider))]
    public class Interactuable : MangosBehaviour
    {
        public ActionId actionType;
        public ObjectHoldId holdId;
        public SphereCollider InteractRange;

        private void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            gameObject.tag = "Interactuable";
            InteractRange.isTrigger = true;
        }

        public virtual void OnInteract() { }

        public virtual int GetHoldId()
        {
            return (int)holdId;
        }

        public virtual void OnUse() { }

        public virtual bool checkRequisite(){
            return true;
        }
    }
}
