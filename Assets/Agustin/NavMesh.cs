using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour {
	
	public NavMeshAgent nav;
	public Transform Obj;
	
	float delay = 1.0f;
	
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		nav.SetDestination(Obj.position);
		
		if(nav.speed == 0)
		{
			delay -= Time.deltaTime;
			
			if(delay <= 0.0f)
			{
				GameObject go = Generar_Bala();
				
				go.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
				
				delay = 1.0f;
			}
		}
	}
}
