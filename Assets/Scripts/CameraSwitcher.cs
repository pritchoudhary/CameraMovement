using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    public Camera FPSCamera;
    public Camera TPCamera;

    private bool camSwitch = false;

	// Use this for initialization
	void Start ()
    {
        FPSCamera.gameObject.SetActive(true);
        TPCamera.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.C))
        {
            camSwitch = !camSwitch;
        }

        if(camSwitch)
        {
            FPSCamera.gameObject.SetActive(false);
            TPCamera.gameObject.SetActive(true);
        }
        else
        {
            FPSCamera.gameObject.SetActive(true);
            TPCamera.gameObject.SetActive(false);
        }
	}
}
