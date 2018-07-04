using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char : MonoBehaviour
{
    public float speed = 10;

    private Vector3 right, forward;
    

	void Update ()
    {
        right = Camera.main.transform.right;
        forward = Camera.main.transform.forward;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        

        transform.Translate((Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward) * speed * Time.deltaTime, Space.World);
		
	}
}
