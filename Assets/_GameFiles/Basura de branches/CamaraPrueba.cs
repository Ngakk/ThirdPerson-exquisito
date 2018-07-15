using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPrueba : MonoBehaviour {

    public Transform target;
    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0, 3.4f, 0);
        public float lookSmooth = 100f;
        public float distanceFromTarget = -8;
        public float zoomSmooth = 10;
        public float maxZoom = -2;
        public float minZoom = -15;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -20;
        public float yRotation = -180;
        public float maxXRotation = 25;
        public float minXRotation = -85;
        public float vOrbitSmooth = 150;
        public float hOrbitSmooth = 150;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string Snap = "Snap";
        public string cam_Horizontal = "cam_horizontal";
        public string cam_Vertical = "cam_vertical";
        public string zoom = "zoom";
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    CharacterController charController;
    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput;

	// Use this for initialization
	void Start () {
        SetCameraTarget(target);

        targetPos = target.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += target.position;
        transform.position = destination;
    }

    void Update()
    {
        GetInput();
        OrbitTarget();
        ZoomInOnTarget();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        MoveToTarget();
        LookAtTarget();
	}

    void MoveToTarget()
    {
        targetPos = target.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += target.position;
        transform.position = destination;
    }

    void LookAtTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
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
    }

    void OrbitTarget()
    {
        if(hOrbitSnapInput > 0)
        {
            orbit.yRotation = -180;
        }

        orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += -hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;

    }

    void ZoomInOnTarget()
    {

    }
}
