using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mangos
{
    public class CamaraThirdPerson : MangosBehaviour
    {
        private const float Y_ANGLE_MIN = 0f;
        private const float Y_ANGLE_MAX = 50f;

        public GameObject Objective;
        public Transform camTransform;
        public LayerMask collisionLayer;
        public bool colliding = false;
        public Vector3[] adjustedCamClipPoints;
        public Vector3[] desiredCamClipPoints;

        private Camera Cam;
        private float distance = 10f;
        private float currentX = 0f;
        private float currentY = 0f;
        private float sensivityX = 4f;
        private float sensivityY = 1f;

        // Use this for initialization
        void Start()
        {
            camTransform = transform;
            Cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            float camHorizontal = Input.GetAxis("camHorizontal");
            float camVertical = Input.GetAxis("camVertical");

            if (Input.GetJoystickNames().Length > 0) {
                currentX += camHorizontal;
                currentY += camVertical;
            } else {
                currentX += Input.GetAxis("Mouse X");
                currentY += Input.GetAxis("Mouse Y");
            }

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }

        void FixedUpdate()
        {

        }

        void LateUpdate()
        {
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            camTransform.position = Objective.transform.position + rotation * dir;
            camTransform.LookAt(Objective.transform.position);
        }

        public void Initialize(Camera cam)
        {
            Cam = cam;
            adjustedCamClipPoints = new Vector3[5];
            desiredCamClipPoints = new Vector3[5];
        }

        bool CollisionDetectedAtClipPoints (Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if(Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!Cam)
                return;

            intoArray = new Vector3[5];

            float z = Cam.nearClipPlane;
            float x = Mathf.Tan(Cam.fieldOfView / 3.41f) * z;
            float y = x / Cam.aspect;

            //Top-left
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition;
            //Top-right
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;
            //Bottom-left
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;
            //Bottom-left
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition;

            //cam's position
            intoArray[4] = cameraPosition - Cam.transform.forward;
        }

        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1f;

            for(int i = 0; i < desiredCamClipPoints.Length; i++)
            {
                Ray ray = new Ray(from, desiredCamClipPoints[i] - from);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    if (distance == -1)
                        distance = hit.distance;
                    else
                    {
                        if (hit.distance < distance)
                            distance = hit.distance;
                    }
                }
            }

            if (distance == -1)
                return 0;
            else
                return distance;
        }

        public void CheckColliding(Vector3 targetPosition)
        {
            if (CollisionDetectedAtClipPoints(desiredCamClipPoints, targetPosition))
                colliding = true;
            else
                colliding = false;
        }
    }
}