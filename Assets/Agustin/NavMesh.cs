using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour {
	
	public NavMeshAgent nav;
	public Transform Obj;
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		nav.SetDestination(Obj.position);
	}
}
