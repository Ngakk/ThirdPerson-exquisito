using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPrueba : MonoBehaviour {

    public Transform target;
    public float lookSmooth = 0.09f;
    public float xRotation;
    public float yRotation;
    public float xTilt = 10;

    Vector3 destination = Vector3.zero;
    CharacterController charController;
    float rotateVel = 0;

	// Use this for initialization
	void Start () {
        SetCameraTarget(target);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        MoveToTarget();
        LookAtTarget();
	}

    void MoveToTarget()
    {
        //destination = charController.TargetRotation * offsetFromTarget;
        destination += target.position;
        transform.position = destination;
    }

    void LookAtTarget()
    {
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0);
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
}
