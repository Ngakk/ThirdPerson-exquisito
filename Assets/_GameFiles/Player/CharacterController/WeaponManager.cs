using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    public class WeaponManager : MangosBehaviour
    {
        Weapon weapon;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        //TODO: talves cambiar weapon a lista de weapons, hacer que suelte el arma o la guarde cuando agarras una arma nueva
        //hacer que ataque cuando puedes atacar, hacer el sistema de "combos" y seleccion de ataques dinamico
        //decirle a santiago que si se va a rifar con mas animaciones


        public void setWeapon(GameObject w)
        {
            Debug.Log("Weapon set");
            weapon = w.GetComponent<Weapon>();
            weapon.OnWeaponSet();
        }

    }
}
