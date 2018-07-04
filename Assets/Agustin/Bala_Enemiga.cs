using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
	public class Bala_Enemiga : MangosBehaviour 
	{
		
		float tiempo = 5.0f;
		float delay;
		
		Rigidbody rigi;
	
		// Use this for initialization
		void Start () 
		{
			rigi = gameObject.GetComponent<Rigidbody>();
		}
		
		// Update is called once per frame
		void Update () 
		{
			delay -= Time.deltaTime;
			
			if(delay <= 0.0f)
			{
				gameObject.SetActive(false);
				
				delay = tiempo;
			}
		}
		
		public void CambiarDireccion()
		{
			rigi.velocity = -rigi.velocity;
		}
	}
}
