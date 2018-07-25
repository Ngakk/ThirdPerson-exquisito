using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPrueba : MonoBehaviour {

    public Transform target;
    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0, 3.4f, 0);
        public float distanceFromTarget = -8;
        public float zoomSmooth = 10;
        public float zoomStep = 2;
        public float maxZoom = -2;
        public float minZoom = -15;
        public bool smoothFollow = true;
        public float smooth = 0.05f;
        public float secondSmooth = 15f;

        [HideInInspector]
        public float newDistance = -8;
        [HideInInspector]
        public float adjustmentDistance = -8;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -20;
        public float yRotation = -180;
        public float maxXRotation = 25;
        public float minXRotation = -5;
        public float vOrbitSmooth = 7;
        public float hOrbitSmooth = 16;
    }

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLines = true;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string Snap = "Snap";
        public string cam_Horizontal = "camHorizontal";
        public string cam_Vertical = "camVertical";
        public string zoom = "Mouse ScrollWheel";
        public string mouseOrbitH = "Mouse Orbit";
        public string mouseOrbitV = "MouseOrbitVertical";
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public DebugSettings debug = new DebugSettings();
    public CollisionHandler collision = new CollisionHandler();

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero;
    Vector3 camVel = Vector3.zero;
    CharacterController charController;
    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput, mouseOrbitInput, vMouseOrbitInput;
    Vector3 previousMousePos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;

	// Use this for initialization
	void Start () {
        SetCameraTarget(target);

        vOrbitInput = hOrbitInput = zoomInput = hOrbitInput = mouseOrbitInput = vMouseOrbitInput = 0;

        MoveToTarget();

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        previousMousePos = currentMousePos = Input.mousePosition;
    }

    void Update()
    {
        GetInput();
        ZoomInOnTarget();
    }

    void FixedUpdate()
    {
        MoveToTarget();
        LookAtTarget();
        OrbitTarget();
        OrbitTarget();

        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        for(int i = 0; i<5; i++)
        {
            if(debug.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.desiredCameraClipPoints[i], Color.white);
            }
            if(debug.drawAdjustedCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.adjustedCameraClipPoints[i], Color.green);
            }
        }

        collision.CheckColliding(targetPos);
        position.adjustmentDistance = collision.GetAdjustedDistanceWithRay(targetPos);
    }

    void MoveToTarget()
    {
        targetPos = target.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += target.position;

        if(collision.colliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * position.adjustmentDistance;
            adjustedDestination += targetPos;

            if(position.smoothFollow)
            {
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref camVel, position.smooth);
            }
            else
            {
                transform.position = adjustedDestination;
            }
        } else
        {
            if (position.smoothFollow)
            {
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVel, position.smooth);
            }
            else
            {
                transform.position = destination;
            }
        }
    }

    void LookAtTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.secondSmooth * Time.deltaTime);
    }

    void SetCameraTarget(Transform t)
    {
        target = t;
        if(target != null)
        {
            if(target.GetComponent<CharacterController>())
                charController = target.GetComponent<CharacterController>();
        }
    }
    
    void GetInput()
    {
        vOrbitInput = Input.GetAxis(input.cam_Vertical);
        hOrbitInput = Input.GetAxis(input.cam_Horizontal);
        hOrbitSnapInput = Input.GetAxis(input.Snap);
        zoomInput = Input.GetAxis(input.zoom);
        //mouseOrbitInput = Input.GetAxis(input.);
        //vMouseOrbitInput = Input.GetAxis("");
    }

    void MouseOrbitTarget()
    {
        previousMousePos = currentMousePos;
        currentMousePos = Input.mousePosition;

        Vector3.Normalize(previousMousePos);
        Vector3.Normalize(currentMousePos);

        if(mouseOrbitInput > 0)
        {
            orbit.xRotation += (currentMousePos.y - previousMousePos.y) * orbit.vOrbitSmooth;
            orbit.yRotation += (currentMousePos.x - previousMousePos.x) * orbit.hOrbitSmooth;
        }

        if(vMouseOrbitInput > 0)
        {
            orbit.xRotation += (currentMousePos.y - previousMousePos.y) * (orbit.vOrbitSmooth / 2);
        }

        CheckVerticalRotation();
    }

    void OrbitTarget()
    {
        if(hOrbitSnapInput > 0)
        {
            orbit.yRotation = -180;
        }

        orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += -hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;

        CheckVerticalRotation();
    }

    void ZoomInOnTarget()
    {
        position.newDistance += position.zoomStep * zoomInput;

        position.distanceFromTarget = Mathf.Lerp(position.distanceFromTarget, position.newDistance, position.zoomSmooth * Time.deltaTime);

        if(position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
            position.newDistance = position.maxZoom;
        }
        if(position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
            position.newDistance = position.minZoom;
        }
    }

    void CheckVerticalRotation()
    {
        if (orbit.xRotation > orbit.maxXRotation)
        {
            orbit.xRotation = orbit.maxXRotation;
        }
        if (orbit.xRotation < orbit.minXRotation)
        {
            orbit.xRotation = orbit.minXRotation;
        }
    }
    
    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints;
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;

        Camera camera;

        public void Initialize(Camera cam)
        {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!camera)
                return;

            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
            float y = x / camera.aspect;

            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition; //top-left
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition; //top-right
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition; //bottom-left
            intoArray[3] = (atRotation * new Vector3(x, -y, -z)) + cameraPosition; //bottom-right
            intoArray[4] = cameraPosition - camera.transform.forward; //camera position
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for(int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if(Physics.Raycast(ray, distance, collisionLayer))
                    return true;
            }
            return false;
        }

        public float GetAdjustedDistanceWithRay(Vector3 from)
        {
            float distance = -1;

            for(int i = 0; i < desiredCameraClipPoints.Length; i++)
            {
                Ray ray = new Ray(from, adjustedCameraClipPoints[i] - from);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    if(distance == -1)
                        distance = hit.distance;
                    else
                    {
                        if(hit.distance < distance)
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
            if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
            {
                colliding = true;
            }
            else
            {
                colliding = false;
            }
        }
    }
}
