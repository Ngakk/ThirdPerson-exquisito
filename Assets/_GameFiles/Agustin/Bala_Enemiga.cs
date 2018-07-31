﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
	public class Bala_Enemiga : MangosBehaviour 
	{
		float delay = 5.0f;
		
		Rigidbody rigi;
	
		// Use this for initialization
		void Start () 
		{
			rigi = gameObject.GetComponent<Rigidbody>();
			delay = 5.0f;
		}
		
		// Update is called once per frame
		void Update () 
		{
			delay -= Time.deltaTime;
			
			if(delay <= 0.0f)
			{
				gameObject.SetActive(false);
				
				delay = 5.0f;
			}
		}
		
		void OnTriggerEnter(Collider _col)
		{
			if(_col.CompareTag("Player"))
			{
				gameObject.SetActive(false);
                StaticManager.playerController.life--;
			}
		}
		
		public void CambiarDireccion()
		{
			rigi.velocity = -rigi.velocity;
		}
	}
}
