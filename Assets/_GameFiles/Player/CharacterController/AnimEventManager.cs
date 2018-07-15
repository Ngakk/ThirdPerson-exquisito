using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    public class AnimEventManager : MangosBehaviour
    {
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

        public void pickItUp()
        {
            playerController.pickItUp();
        }
    }
}
