using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mangos
{
    public class AnimEventManager : MangosBehaviour
	{
    	
		public UnityEvent[] eventToCall;
		
        WeaponManager weaponManager;
        ThirdPersonCharacterController playerController;
        // Use this for initialization
        void Start()
        {
            playerController = GetComponentInParent<ThirdPersonCharacterController>();
            weaponManager = GetComponentInParent<WeaponManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        
		public void CallEventIndex(int eventID)
		{
			eventToCall[eventID].Invoke();
		}
    }
}
