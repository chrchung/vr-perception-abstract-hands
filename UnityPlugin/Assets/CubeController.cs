using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

    public DistortionsController distortionsController;
    public DistortionsController anotherDistortionsController;

	// Use this for initialization
	void Start () {
        transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.0f, 0.5f), Random.Range(0.0f, 0.5f));
        transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        distortionsController.trialId = 0;
    }
	
	// Update is called once per frame
	void Update () {
       //if (OVRInput.GetUp(OVRInput.Button.One))
       // {
       //     transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.0f, 0.5f), Random.Range(0.0f, 0.5f));
       //     transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

       //     //int r = Random.Range(0, 2);
       //     int r = distortionsController.trialId + 1;

       //     if  (r == 5)
       //     {
       //         r = 0;
       //     }

       //     distortionsController.trialId = r;
       //     distortionsController.needUpdate = true;

       //     anotherDistortionsController.trialId = r;
       //     anotherDistortionsController.needUpdate = true;
       // }
    }
}
