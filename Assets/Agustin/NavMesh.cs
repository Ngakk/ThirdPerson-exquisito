﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mangos
{
	public class NavMesh : MangosBehaviour {
		
		public NavMeshAgent nav;
		public Transform Obj;
		public Animator anim;
		
		float delay = 1.0f;
		
		float delay_triple = 0.5f;
			
		int contador = 0;
		
		Vector3 PosActual;
		
		float hp;
		
		// Pool Bala
		List<GameObject> Bala = new List<GameObject>();
		
		GameObject Generar_Bala()
		{
			for (int i = 0; i < Bala.Count; i++)
			{
				if(Bala[i].activeSelf == false)
				{
					Bala[i].SetActive(true);
					Bala[i].transform.position = transform.position;
					Bala[i].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
					return Bala[i];
				}
			}
			
			GameObject Prefab_Bala = Resources.Load<GameObject>("Sphere");
			GameObject go = Instantiate(Prefab_Bala, transform.position, Quaternion.identity);
			Bala.Add(go);
			return go;
		}
	
		// Use this for initialization
		void Start () 
		{
			PosActual = gameObject.transform.position;
			delay = 1.0f;
			hp = 100.0f;
		}
		
		// Update is called once per frame
		void Update () 
		{
			nav.SetDestination(Obj.position);
			
			if(hp <= 0.0f)
			{
				gameObject.SetActive(false);
			}
			
			if(PosActual != transform.position)
			{
				anim.SetBool("Steps", true);
			}
			else
			{
				anim.SetBool("Steps", false);
				
				delay -= Time.deltaTime;
				
				if(hp > 75 && hp <= 100)
				{
					if(delay <= 0.0f)
					{
						Disparo();
					
						delay = 1.0f;
					}
				}
				else if(hp > 50.0f && hp <= 75.0f)
				{
					if(delay <= 0.0f)
					{
						delay_triple -= Time.deltaTime;
						
						if(delay_triple <= 0)
						{
							Disparo();
							
							contador++;
							
							delay_triple = 0.5f;
							
							if(contador == 3)
							{
								delay_triple = 0.5f;
								
								delay = 1.0f;
								
								contador = 0;
							}
						}
					}
				}
				else if(hp > 25.0f && hp <= 50.0f)
				{
					
				}
				
				else if(hp > 0.0f && hp <= 25.0f)
				{
					
				}
			}
			
			PosActual = transform.position;
		}
		
		void Disparo()
		{
			GameObject go = Generar_Bala();
			
			go.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
		}
	}
}
