using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
	public class Enemigo : MangosBehaviour
	{
		public Vector3 Velocidad;
		
		Rigidbody rigi;
		
		float delay = 1.0f;
		
		float delay_triple = 0.5f;
		
		int contador = 0;
		
		float hp = 100.0f;
		
		public Animator anim;
		
		bool Steps = false;
		
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
			delay = 1.0f;
			
			rigi = gameObject.GetComponent<Rigidbody>();
		}
		
		// Update is called once per frame
		void Update () 
		{
			delay -= Time.deltaTime;
			
			hp -= Time.deltaTime;
			
			if(hp > 75.0f && hp <= 100.0f)
			{
				if(delay <= 0.0f)
				{
					Disparo();
					
					delay = 1.0f;
				}
			}
			
			if(hp > 50.0f && hp <= 75.0f)
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
			
			if(hp > 25.0f && hp <= 50.0f)
			{
				
			}
			
			if(hp > 0.0f && hp <= 25.0f)
			{
				
			}
		}
		
		void OnCollisionEnter(Collision _col)
		{
			/*if(_col.collider.CompareTag("Bala_Player"))
			{
			StartCoroutine("DamageStop");
				print("Recibi daño");
			}*/
		}
		
		void Disparo()
		{
			GameObject go = Generar_Bala();
		
			go.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
		}
		
		/*IEnumerator DamageStop()
		{
			vulnerable = false;
			claw.GetComponentInChildren<MeshRenderer> ().material.SetColor("_EmissionColor", Color.white);
			float startTime = Time.time;
			
			while (Time.time < startTime +invulnerableTime) 
			{
				claw.gameObject.GetComponentInChildren<MeshRenderer> ().material.SetColor("_EmissionColor", Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(startTime, startTime+invulnerableTime, Time.time) ));
				yield return null;
			}
			
			claw.gameObject.GetComponentInChildren<MeshRenderer> ().material.SetColor("_EmissionColor", Color.black);
			vulnerable = true;
		}*/
	}
}