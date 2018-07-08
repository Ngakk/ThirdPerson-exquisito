using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    [RequireComponent(typeof(Rigidbody))]
    public class ThirdPersonCharacterController : MangosBehaviour
    {
		Rigidbody rigi;
		Camera cam;
		[HideInInspector]
		public Vector3 dir, lookDir;
		Vector3 scaler;
		public float speed;
		[Range(0, 1)]
	    public float rotationSpeed;
	    public float m_angle;

        private void Awake()
        {
            StaticManager.playerController = this;
        }

        // Use this for initialization
        void Start()
        {
            rigi = GetComponent<Rigidbody>();
			cam = Camera.main;
			scaler = new Vector3 (1, 0, 1);
	        lookDir = rigi.velocity;
	        m_angle = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Move(float xAxis, float yAxis)
	    {            
        	
			//Movement
		    dir = Vector3.Scale (cam.transform.forward, scaler).normalized * yAxis + Vector3.Scale(cam.transform.right, scaler).normalized * xAxis;
		    float dotProduct = Vector3.Dot(rigi.velocity.normalized, dir.normalized);
			rigi.velocity = speed * dir.normalized;

		    //Rotation
		    lookDir += (rigi.velocity - lookDir) * rotationSpeed;
			if (rigi.velocity != Vector3.zero) {
				transform.LookAt (transform.position + lookDir.normalized);
			}
        }
    }
}