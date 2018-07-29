using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    public class WeaponManager : MangosBehaviour
    {
        List<Weapon> weapons = new List<Weapon>();
        int currentPrimaryWeapon;
        int currentSecondaryWeapon;
        int currentSheatedWeapon;

        void Start()
        {
            currentPrimaryWeapon = -1;
            currentSecondaryWeapon = -1;
            currentSheatedWeapon = -1;
        }

        void Update()
        {

        }

        public void setWeapon(GameObject w)
        {
            weapons.Add(w.GetComponent<Weapon>());
            weapons[weapons.Count-1].OnWeaponSet();
            Weapon temp = w.GetComponent<Weapon>();
            if(temp.GetHoldId() <= 3)
            {
                currentPrimaryWeapon = weapons.Count - 1;
            }
            else if(temp.GetHoldId() <= 5)
            {
                currentSecondaryWeapon = weapons.Count - 1;
            }
            else
            {
                currentPrimaryWeapon = weapons.Count - 1;
                currentSecondaryWeapon = -1;
            }
            Debug.Log("Primary: " + currentPrimaryWeapon + ", Secondary: " + currentSecondaryWeapon + ", Sheated: " + currentSheatedWeapon);
        }

        public bool[] onPrePickup(int hold)
        {
            Debug.Log("prePickUp Weaponm: " + currentPrimaryWeapon + ", " + currentSecondaryWeapon + ", " + currentSheatedWeapon +", hold: " + hold);
            bool[] actions = new bool[3];
            if(hold <= 3)
            {
                if(currentPrimaryWeapon != -1 && currentSheatedWeapon != -1)
                {
                    actions[0] = true;
                }
                else if(currentPrimaryWeapon != -1 && currentSheatedWeapon == -1)
                {
                    actions[1] = true;
                }
            }
            else if(hold <= 5 )
            {
                if(currentSecondaryWeapon != -1)
                {
                    actions[2] = true;
                }
            }
            else
            {
                Debug.Log("trying to pickup two handed: " + currentPrimaryWeapon + ", " + currentSheatedWeapon);
                if (currentSecondaryWeapon != -1)
                {
                    actions[2] = true;
                }
                if (currentPrimaryWeapon != -1 && currentSheatedWeapon != -1)
                {
                    actions[0] = true;
                }
                else if (currentPrimaryWeapon != -1 && currentSheatedWeapon == -1)
                {
                    actions[1] = true;
                }
            }
            Debug.Log(actions[0] + ", " + actions[1] + ", " + actions[2]);
            return actions;
        }
           
        public void SheatWeapon()
        {
            currentSheatedWeapon = currentPrimaryWeapon;
            currentPrimaryWeapon = -1;
        }

        public void UnsheatWeapon()
        {
            currentPrimaryWeapon = currentSheatedWeapon;
            currentSheatedWeapon = -1;
        }

        public GameObject getCurrentPrimaryWeapon()
        {
            if (currentPrimaryWeapon != -1)
                return weapons[currentPrimaryWeapon].gameObject;
            else return null;
        }

        public GameObject getCurrentSecondaryWeapon()
        {
            if (currentSecondaryWeapon != -1)
                return weapons[currentSecondaryWeapon].gameObject;
            else return null;
        }

        public GameObject getCurrentSheatedWeapon()
        {
            if (currentSheatedWeapon != -1)
                return weapons[currentSheatedWeapon].gameObject;
            else return null;
        }

        public int attack()
        {
            if(currentPrimaryWeapon != -1)
            {
                return 1;
            }
            else if (currentPrimaryWeapon == -1 && currentSheatedWeapon != -1)
            {
                return 0;
            }

            return -1;
        }

        public int attack2()
        {
            if (currentSecondaryWeapon != -1)
            {
                return 2;
            }

            return -1;
        }

        public void throwSecondary()
        {
            currentSecondaryWeapon = -1;
        }
    }
}
