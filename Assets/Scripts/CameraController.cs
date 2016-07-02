using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;
    
    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPositionOffset = new Vector3(0, 3.4f, 0);
        public float smoothen = 100f;
        public float distanceFromTarget = -4f;
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
        public float verticalOrbitSmooth = 150;
        public float horizontalOrbitSmooth = 150;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";
        public string ORBIT_HORZIZONTAL = "OrbitHorizontal";
        public string ORBIT_VERTICAL = "OrbitVertical";
        public string ZOOM = "Mouse ScrollWheel";
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();

    Vector3 targetPosition = Vector3.zero;
    Vector3 destination = Vector3.zero;

    CharacterController characterController;
    float verticalOrbitInput, horizontalOrbitInput, zoomInput, horizontalOrbitSnapInput;

	// Use this for initialization
	void Start () {

        SetCameraTarget(target);

        targetPosition = target.position + position.targetPositionOffset;

        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPosition;
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

        //moving
        MoveToTarget();

        //rotating
        LookAtTarget();
	
	}

    void GetInput()
    {
        verticalOrbitInput = Input.GetAxisRaw(input.ORBIT_VERTICAL);
        horizontalOrbitInput = Input.GetAxisRaw(input.ORBIT_HORZIZONTAL);
        horizontalOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL_SNAP);
        zoomInput = Input.GetAxisRaw(input.ZOOM);
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
        targetPosition = target.position + position.targetPositionOffset;

        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPosition;
        transform.position = destination;
    }

    void LookAtTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.smoothen * Time.deltaTime);
    }

    void OrbitTarget()
    {
        if(horizontalOrbitSnapInput > 0)
        {
            orbit.yRotation = -180;
        }

        orbit.xRotation += -verticalOrbitInput * orbit.verticalOrbitSmooth * Time.deltaTime;
        orbit.yRotation += -horizontalOrbitInput * orbit.horizontalOrbitSmooth * Time.deltaTime;

        if(orbit.xRotation > orbit.maxXRotation)
        {
            orbit.xRotation = orbit.maxXRotation;
        }
        if (orbit.xRotation < orbit.minXRotation)
        {
            orbit.xRotation = orbit.minXRotation;
        }
    }

    void ZoomInOnTarget()
    {
        position.distanceFromTarget += zoomInput * position.zoomSmooth;

        if(position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
        }

        if (position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
        }


    }

}
