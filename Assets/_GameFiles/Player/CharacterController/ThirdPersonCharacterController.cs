﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    //TODO: hacer que entre a use, y de una vez unos ejemplos de items
    [RequireComponent(typeof(Rigidbody))]
    public class ThirdPersonCharacterController : MangosBehaviour
    {
		Rigidbody rigi;
		Camera cam;
        Animator anim;
        GameObject interactuable;
        WeaponManager weaponManager;
        public Transform rightHand, leftHand;
		[HideInInspector]
		public Vector3 dir, lookDir;
		Vector3 scaler;
		public float speed;
		[Range(0, 1)]
	    public float rotationSpeed;
	    public float m_angle;

        private bool canMove;
        private bool holdingItem;
        private bool canInteract;

        private void Awake()
        {
            StaticManager.playerController = this;
        }

        // Use this for initialization
        void Start()
        {
            rigi = GetComponent<Rigidbody>();
			cam = Camera.main;
            anim = GetComponentInChildren<Animator>();
            weaponManager = GetComponent<WeaponManager>();
			scaler = new Vector3 (1, 0, 1);
	        lookDir = rigi.velocity;
	        m_angle = 0;
            canMove = true;
        }

        void Update()
        {

        }

        public void Move(float xAxis, float yAxis)
	    {
            if (!canMove)
            {
                rigi.velocity = Vector3.Scale(rigi.velocity, new Vector3(0, 1, 0));
                anim.SetBool("IsMoving", false);
                return;
            }

			//Movement
		    dir = Vector3.Scale (cam.transform.forward, scaler).normalized * yAxis + Vector3.Scale(cam.transform.right, scaler).normalized * xAxis;
			rigi.velocity = speed * dir.normalized;

		    //Rotation
		    lookDir += (rigi.velocity - lookDir) * rotationSpeed;
			if (rigi.velocity != Vector3.zero) {
				transform.LookAt (transform.position + lookDir.normalized);
			}

            //Animacion
            if (rigi.velocity == Vector3.zero)
                anim.SetBool("IsMoving", false);
            else
                anim.SetBool("IsMoving", true);
        }

        public void onActionDown()
        {
            anim.SetTrigger("Use");
        }

        public void onDashDown()
        {
            anim.SetTrigger("Dash");
        }

        public void onInteractDown()
        {
            if (interactuable != null)
            {
                Interactuable temp = interactuable.GetComponent<Interactuable>();
                if (temp.checkRequisite())
                {
                    switch (temp.actionType)
                    {
                        case ActionId.pickup:
                            anim.SetTrigger("Interact");
                            anim.SetInteger("HoldId", (int)temp.holdId);
                            break;
                        case ActionId.pocket:

                            break;
                        case ActionId.use:

                            break;
                    }
                    temp.OnInteract();
                }
            }
        }

        public void setCanMove(bool b){ canMove = b; }

        public void setHoldingItem(bool b) { holdingItem = b; }

        private void setInteractuable(GameObject go)
        {
            if (interactuable == null)
                interactuable = go;
            else if ((gameObject.transform.position - interactuable.transform.position).magnitude > (gameObject.transform.position - go.transform.position).magnitude)
	            interactuable = go;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Interactuable"))
                setInteractuable(other.gameObject);
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Interactuable"))
                interactuable = null;
        }

        public void pickItUp()
        {
            if(interactuable != null)
            {
                Interactuable temp = interactuable.GetComponent<Interactuable>();
                switch (temp.actionType)
                {
                    case ActionId.pickup:
                        weaponManager.setWeapon(interactuable);
                        takeWeaponInHands();
                        break;
                    case ActionId.pocket:
                        break;
                    case ActionId.use:
                        break;
                }
            }
        }

        public void takeWeaponInHands()
        {
            Debug.Log("weapon taken");
            if(interactuable.GetComponent<Weapon>() != null)
            {
                Weapon tempW = interactuable.GetComponent<Weapon>();
                if((int)tempW.GetHoldId() <= 3)
                {
                    interactuable.transform.parent = rightHand;
                }
                else
                {
                    interactuable.transform.parent = leftHand;
                }

                interactuable.transform.localRotation = Quaternion.identity;
                interactuable.transform.localPosition = Vector3.zero;
                tempW.InteractRange.enabled = false;
                interactuable = null;
            }
            
        }
    } 
}