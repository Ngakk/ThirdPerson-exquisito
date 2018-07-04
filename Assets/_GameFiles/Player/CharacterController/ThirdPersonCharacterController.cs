using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    [RequireComponent(typeof(Rigidbody))]
    public class ThirdPersonCharacterController : MangosBehaviour
    {

        public Camera cam;
        public float speed;
        Rigidbody rigi;
        public Transform trans;

        private void Awake()
        {
            StaticManager.playerController = this;
        }

        // Use this for initialization
        void Start()
        {
            rigi = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Move(float xAxis, float yAxis)
        {            
            Vector3 axis = new Vector3(xAxis, 0, yAxis).normalized;
            Vector3 camForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));

            Quaternion temp = cam.transform.rotation;
            temp.SetFromToRotation(new Vector3(0, 0, 1), axis);
            trans.rotation = temp;

            rigi.velocity = speed * trans.forward;

        }
    }
}