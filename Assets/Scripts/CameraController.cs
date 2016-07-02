using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;
    public float smoothen = 0.09f;

    public Vector3 offsetFromTarget = new Vector3(0, 6, -8);
    public float xTilt = 10;

    Vector3 destination = Vector3.zero;

    CharacterController characterController;

    float rotateVelocity = 0;

	// Use this for initialization
	void Start () {

        SetCameraTarget(target);
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

        //moving
        MoveToTarget();

        //rotating
        LookAtTarget();
	
	}

    void SetCameraTarget(Transform t)
    {
        target = t;

        if(target != null)
        {
            if (target.GetComponent<CharacterController>())
            {
                characterController = target.GetComponent<CharacterController>();
            }
            
            else
                Debug.LogError("Tareget needs a character controller");
        }

        else
        {
            Debug.LogError("Camera needs a target");
        }
    }

    void MoveToTarget()
    {
        destination = characterController.TargetRotation * offsetFromTarget;
        destination += target.position;
        transform.position = destination;
    }

    void LookAtTarget()
    {
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVelocity, smoothen);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0);
    }


}
