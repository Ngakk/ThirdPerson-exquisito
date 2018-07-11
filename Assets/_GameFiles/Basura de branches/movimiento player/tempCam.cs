using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempCam : MonoBehaviour {

    public GameObject playah;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(playah.transform.position);	
	}
}
