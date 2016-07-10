using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour // wrong name // should be named Camera Controller
{
    public Camera FPCamera;
    public Camera TPCamera;

    private float minZoom = 5;
    private float maxZoom = 15;
    public float zoomSpeed = 50;

    [Tooltip("true = TP, false = FP")]
    private bool TPCamActive = false;

	// Use this for initialization
	void Start ()
    {
        // Enable first person camera at start and disable third person
        FPCamera.gameObject.SetActive(true);
        TPCamera.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Switch cam with "F"
	    if (Input.GetKeyDown(KeyCode.F))
        {
            TPCamActive = !TPCamActive;
        }

        // check what cam should be enabled
        if(TPCamActive)
        {
            FPCamera.gameObject.SetActive(false);
            TPCamera.gameObject.SetActive(true);
        }
        else
        {
            FPCamera.gameObject.SetActive(true);
            TPCamera.gameObject.SetActive(false);
        }

        // If TPCamera is active enable scrolling (because we dont need it in FPCamera).
        // if TPCamera is close to head -> switch to FPCamera
        if (TPCamActive)
        {
            if (Vector3.Distance(TPCamera.transform.position, transform.position) > minZoom)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    TPCamera.transform.position = Vector3.MoveTowards(TPCamera.transform.position, transform.position, zoomSpeed * Time.deltaTime);
                }
            }
            else
            {
                TPCamera.transform.position = Vector3.MoveTowards(TPCamera.transform.position, transform.position, -zoomSpeed * Time.deltaTime);
                TPCamActive = !TPCamActive;
            }
            if (Vector3.Distance(TPCamera.transform.position, transform.position) < maxZoom)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    TPCamera.transform.position = Vector3.MoveTowards(TPCamera.transform.position, transform.position, -zoomSpeed * Time.deltaTime);
                }
            }
            else
            {
                TPCamera.transform.position = Vector3.MoveTowards(TPCamera.transform.position, transform.position, zoomSpeed * Time.deltaTime);
            }
        }
    }
}
