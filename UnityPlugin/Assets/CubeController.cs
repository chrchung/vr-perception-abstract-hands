using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

    public DistortionsController distortionsController;

	// Use this for initialization
	void Start () {
        transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.0f, 0.5f), Random.Range(0.0f, 0.5f));
        transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        distortionsController.trialId = 0;
    }
	
	// Update is called once per frame
	void Update () {
       if (OVRInput.GetUp(OVRInput.Button.One))
        {
            transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.0f, 0.5f), Random.Range(0.0f, 0.5f));
            transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

            distortionsController.trialId = Random.Range(0, 2);
            distortionsController.needUpdate = true;
        }
    }
}
