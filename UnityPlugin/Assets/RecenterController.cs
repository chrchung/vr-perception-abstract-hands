using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class RecenterController : MonoBehaviour {

    public Transform hand;
    public Transform cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.F12))
        {
            UnityEngine.VR.InputTracking.Recenter();
            print(cam.localPosition);
           // hand.position = new Vector3(cam.position[0], cam.position[1], cam.position[2]);

        }
	}
}
