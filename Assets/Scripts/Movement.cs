using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float moveSpeed = 10;
    public float turnSpeed = 50;

    CameraOrbit camerOrbit;

    void Start()
    {
        camerOrbit = GetComponent<CameraOrbit>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        transform.Rotate(0, horizontal, 0);

        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(0, 0, vertical);
    }
}
