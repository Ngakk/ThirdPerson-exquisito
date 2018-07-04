using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 150;
    public Vector3 ax;
    public Space relativeTo;
	
	void Update ()
    {
        transform.Rotate(ax * speed * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel"),relativeTo);
		
	}
}
