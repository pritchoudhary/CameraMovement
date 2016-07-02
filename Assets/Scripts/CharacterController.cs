using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    public float inputDelay = -0.1f;
    public float forwardVelocity = 10.0f;
    public float rotateVelocity = 100.0f;

    Quaternion targetRotation;
    Rigidbody rb;

    float forwardInput, turnInput;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }



	// Use this for initialization
	void Start () {

        targetRotation = transform.rotation;
        
        if (GetComponent<Rigidbody>())
            rb = GetComponent<Rigidbody>();

        else
            Debug.LogError("The character needs a rigidbody ");

        forwardInput = turnInput = 0;
            

    }
	
	// Update is called once per frame
	void Update () {

        GetInput();
        Turn();
	}

    void FixedUpdate()
    {
        Run();
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    void Run()
    {
        //Deadzone check
        if (Mathf.Abs(forwardInput) > inputDelay)
        {
            //Run
            rb.velocity = transform.forward * forwardInput * forwardVelocity;
        }

        else
            //zero velocity
            rb.velocity = Vector3.zero;
    }

    void Turn()
    {
        //Deadzone check
        if (Mathf.Abs(turnInput) > inputDelay)
        {
            //turn
            targetRotation *= Quaternion.AngleAxis(rotateVelocity * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    
}
