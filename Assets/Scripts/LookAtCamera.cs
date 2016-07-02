using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

    public GameObject target;

    //camera's transform to look at the target object
    //LateUpdate because it gives a smoother camera motion. Happens after Update().
    void LateUpdate()
    {
        transform.LookAt(target.transform);
    }

}

