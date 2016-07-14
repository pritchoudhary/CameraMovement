using UnityEngine;
using System.Collections;

public class RaycastSphere : MonoBehaviour
{
    public float maxRaycastDistance = 1000.0f;

    public Vector3 topLeftOffset = new Vector3(1, 1, 0);
    public Vector3 topMiddleOffset = new Vector3(0, 1, 0);
    public Vector3 topRightOffset = new Vector3(-1, 1, 0);
    public Vector3 middleLeftOffset = new Vector3(1, 0, 0);
    public Vector3 middleRightOffset = new Vector3(-1, 0, 0);

    public GameObject playerCharacter;

    Ray topLeft;
    Ray topMiddle;
    Ray topRight;
    Ray middleLeft;
    Ray middleRight;

	// Use this for initialization
	void Start ()
    {
        topLeft = new Ray (topLeftOffset, transform.forward);
        topMiddle = new Ray (topMiddleOffset, transform.forward);
        topRight = new Ray(topRightOffset, transform.forward);
        middleLeft = new Ray(middleLeftOffset, transform.forward);
        middleRight = new Ray(middleRightOffset, transform.forward);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Physics.Raycast(topLeft, maxRaycastDistance))
        {

        }
	}
}
