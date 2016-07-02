using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;

    public float horizontalMovement = 45f;
    public float verticalMovement = 15f;

    
    public void moveHorizontal(bool left)
    {
        float direction = 1;
        if (!left)
            direction *= -1;
        transform.RotateAround(target.position, Vector3.up, horizontalMovement * direction);
    }

    public void moveVertical(bool up)
    {
        float direction = 1;
        if (!up)
            direction *= -1;
        transform.RotateAround(target.position, transform.TransformDirection(Vector3.right), verticalMovement * direction);
    }
}



	
