using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mangos
{
    //TODO: hacer que entre a use, y de una vez unos ejemplos de items
    [RequireComponent(typeof(Rigidbody))]
    public class ThirdPersonCharacterController : MangosBehaviour
    {
		Rigidbody rigi;
		Camera cam;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public GameObject interactuable;
        WeaponManager weaponManager;
        public Transform rightHand, leftHand, sheat;
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
        private bool velZeroFrame;

        public int life = 15;
        public int kills = 0;

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

        void FixedUpdate() {
            rigi.AddForce(Vector3.down * 1000 * Time.fixedDeltaTime);
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
            //dir = Vector3.Scale(transform.forward, scaler).normalized * yAxis + Vector3.Scale(transform.right, scaler).normalized * xAxis;
            rigi.velocity = new Vector3(speed * dir.normalized.x, rigi.velocity.y, speed*dir.normalized.z);

		    //Rotation
		    lookDir += (Vector3.Scale(rigi.velocity, scaler) - lookDir) * rotationSpeed;
			if (rigi.velocity != Vector3.zero) {
				transform.LookAt (transform.position + lookDir.normalized);
			}

            //Animacion
            Debug.Log(Vector3.Scale(rigi.velocity, scaler).magnitude >= 0.1f);
            if (Vector3.Scale(rigi.velocity, scaler).magnitude >= 0.1f && velZeroFrame)
                anim.SetBool("IsMoving", true);
            else if (Vector3.Scale(rigi.velocity, scaler).magnitude >= 0.1f)
                velZeroFrame = true;
            else
            {
                anim.SetBool("IsMoving", false);
                velZeroFrame = false;
            }
        }

        public void onActionDown()
        {
            int useid = weaponManager.attack();
            if(useid != -1)
            {
                anim.SetTrigger("ExitHold");
                anim.SetInteger("UseId", useid);
                anim.SetTrigger("Use");
            }
            
        }

        public void onAction2Down()
        {
            int useid = weaponManager.attack2();
            if(useid != -1)
            {
                anim.SetTrigger("ExitHold");
                anim.SetInteger("UseId", useid);
                anim.SetTrigger("Use");
            }
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
                            onPrePickup((int)temp.GetHoldId());
                            anim.SetInteger("InteractId", temp.GetHoldId());
                            anim.SetTrigger("Interact");
                            anim.SetInteger("HoldId", (int)temp.holdId);
                            transform.LookAt(new Vector3(interactuable.transform.position.x, transform.position.y, interactuable.transform.position.z));
                            break;
                        case ActionId.pocket:

                            break;
                        case ActionId.use:

                            break;
                    }
                    temp.OnInteract();
                }
            }
            else
            {
                if (weaponManager.getCurrentPrimaryWeapon() != null && weaponManager.getCurrentSheatedWeapon() == null)
                    startSheatWeapon();
                else if (weaponManager.getCurrentPrimaryWeapon() != null && weaponManager.getCurrentSheatedWeapon() != null)
                    startSwitchWeapon();
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

        public void onPrePickup(int hold)
        {
            bool[] actions = weaponManager.onPrePickup(hold);
            if (actions[0])
            {
                dropWeapon(weaponManager.getCurrentPrimaryWeapon());
            }
            if (actions[1])
            {
                setCanMove(false);
                startSheatWeapon();
            }
            else
            {
                anim.SetTrigger("ExitHold");
            }
            if (actions[2])
            {
                dropWeapon(weaponManager.getCurrentSecondaryWeapon());
            }
        }

        public void startSheatWeapon()
        {
            anim.SetInteger("UseId", 3);
            anim.SetTrigger("Use");
            anim.SetTrigger("ExitHold");
        }

        public void startSwitchWeapon()
        {
            anim.SetInteger("UseId", 4);
            anim.SetTrigger("Use");
            anim.SetTrigger("ExitHold");
        }

        public void dropWeapon(GameObject weapon)
        {
            weapon.transform.parent = null;
            weapon.GetComponent<Weapon>().OnWeaponDrop();
        }

        public void SheatWeapon()
        {
            GameObject toSheat = weaponManager.getCurrentPrimaryWeapon();
            toSheat.transform.parent = sheat;
            toSheat.transform.localRotation = Quaternion.identity;
            toSheat.transform.localPosition = Vector3.zero;
            weaponManager.SheatWeapon();
        }

        public void UnsheatWeapon()
        {
            GameObject toUnsheat = weaponManager.getCurrentSheatedWeapon();
            if (toUnsheat != null)
            {
                toUnsheat.transform.parent = rightHand;
                toUnsheat.transform.localRotation = Quaternion.identity;
                toUnsheat.transform.localPosition = Vector3.zero;
                weaponManager.UnsheatWeapon();
            }
        }

        public void SwapWeapon()
        {
            GameObject toHand = weaponManager.getCurrentSheatedWeapon();
            GameObject toSheat = weaponManager.getCurrentPrimaryWeapon();

            if(toHand != null)
            {
                toHand.transform.parent = rightHand;
                toHand.transform.localRotation = Quaternion.identity;
                toHand.transform.localPosition = Vector3.zero;
            }

            if(toSheat != null)
            {
                toSheat.transform.parent = sheat;
                toSheat.transform.localRotation = Quaternion.identity;
                toSheat.transform.localPosition = Vector3.zero;
            }

            weaponManager.SheatWeapon();
        }

        public void throwSecondary()
        {
            GameObject toThrow = weaponManager.getCurrentSecondaryWeapon();
            if(toThrow != null)
            {
                toThrow.transform.parent = null;
                toThrow.GetComponent<Weapon>().OnWeaponDrop();
                toThrow.GetComponent<Weapon>().rigi.velocity = rigi.velocity;
                toThrow.GetComponent<Weapon>().rigi.AddForce(transform.forward * 10, ForceMode.Impulse);
                weaponManager.throwSecondary();
            }
        }

        public void Dead()
        {
            if (life <= 0)
                SceneManager.LoadScene(3);
        }

        public void Win()
        {
            if (kills >= 5)
                SceneManager.LoadScene(4);
        }
    } 
}