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
        public bool smoothFollow = true;
        public float smooth = 0.05f;

        [HideInInspector]
        public float newDistance = -8; //set by zoomInput
        [HideInInspector]
        public float adjustmentDistance = -8;
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

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLines = true;
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public DebugSettings debug = new DebugSettings();
    public CollisionHandler collision = new CollisionHandler();

    Vector3 targetPosition = Vector3.zero;
    Vector3 destination = Vector3.zero;

    Vector3 adjustedDestination = Vector3.zero;
    Vector3 cameraVelocity = Vector3.zero;

    CharacterControllerTP characterController;
    float verticalOrbitInput, horizontalOrbitInput, zoomInput, horizontalOrbitSnapInput;

    enum PlayerState
    {
        psWalking,
        psInCover,

        psNumOfPlayerStates               
    }

	// Use this for initialization
	void Start () {

        SetCameraTarget(target);

        targetPosition = target.position + position.targetPositionOffset;

        MoveToTarget();

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameralClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);
    }

    void Update()
    {
        GetInput();
        OrbitTarget();
        ZoomInOnTarget();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        //moving
        MoveToTarget();

        //rotating
        LookAtTarget();

        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameralClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        //draw debug lines
        for(int index = 0; index < 5; index++)
        {
            if(debug.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPosition, collision.desiredCameraClipPoints[index], Color.white);
            }
            if(debug.drawAdjustedCollisionLines)
            {
                Debug.DrawLine(targetPosition, collision.adjustedCameralClipPoints[index], Color.green);
            }
        }

        collision.CheckColliding(targetPosition); //using raycasts;

        position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPosition);


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
            if (target.GetComponent<CharacterControllerTP>())
            {
                characterController = target.GetComponent<CharacterControllerTP>();
            }
            
            else
                Debug.LogError("Target needs a character controller");
        }

        else
        {
            Debug.LogError("Camera needs a target");
        }
    }

    void MoveToTarget()
    {
        targetPosition = target.position + Vector3.up * position.targetPositionOffset.y + Vector3.forward * position.targetPositionOffset.z ;

        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPosition;

        if(collision.colliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * position.adjustmentDistance;
            adjustedDestination += targetPosition;

            if (position.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref cameraVelocity, position.smooth);
            }
            else
                transform.position = adjustedDestination;       
        }
        else
        {
            if (position.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref cameraVelocity, position.smooth);

            }
            else
                transform.position = destination;
        }
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



    //Camera collision
    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedCameralClipPoints;
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;

        Camera camera;

        public void Initialize(Camera cam)
        {
            camera = cam;
            adjustedCameralClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!camera)
                return;

            //clear the contents of intoArray
            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
            float y = x / camera.aspect;

            //top left clip point
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition; //added at rotated point relative to camera

            //top right clip point
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;

            //bottom left clip point
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;

            //bottom right clip point
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition;

            //camera's position
            intoArray[4] = cameraPosition - camera.transform.forward;
        }


        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int index = 0; index < clipPoints.Length; index++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[index] - fromPosition);
                float distance = Vector3.Distance(clipPoints[index], fromPosition);

                if (Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }

            return false;
        }



        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for (int index = 0; index < desiredCameraClipPoints.Length; index++)
            {
                Ray ray = new Ray(from, desiredCameraClipPoints[index] - from);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
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

            else return distance;
        }

        public void CheckColliding(Vector3 targetPosition)
        {
            if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
            {
                colliding = true;
            }
            else
                colliding = false;
        }
    }

}
