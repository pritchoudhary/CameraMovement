using UnityEngine;
using System.Collections;

public class CameraCollisionHandler : MonoBehaviour {

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
            for(int index = 0; index < clipPoints.Length; index++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[index] - fromPosition);
                float distance = Vector3.Distance(clipPoints[index], fromPosition);

                if(Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }

            return false;
        }

       

        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for(int index = 0; index < desiredCameraClipPoints.Length; index++)
            {
                Ray ray = new Ray(from, desiredCameraClipPoints[index] - from);
                RaycastHit hit;

                if(Physics.Raycast (ray, out hit))
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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
