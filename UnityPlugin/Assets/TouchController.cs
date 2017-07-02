using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

    public bool isRightHanded;
    public bool translate;
    public bool camTranslate;
    public Transform Cam;

    public bool rotate;
    public bool camRotate;

    public Vector3 offset;

    public Vector3 camOffset;

	// Use this for initialization
	void Start () {
        //camOffset = new Vector3(0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 angle = transform.localEulerAngles;
        Debug.Log("here");



        if (translate)
        {
            Debug.Log("here");
            Vector3 touchPos;

            if (isRightHanded)
            {
                touchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            }
            else
            {
                touchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            }

            transform.localPosition = new Vector3(touchPos[0] + offset[0], touchPos[1] + offset[1], touchPos[2] + offset[2]);

        }



        if (camTranslate)
        {
            transform.localPosition = new Vector3(Cam.localPosition[0] + camOffset[0], Cam.localPosition[1] + camOffset[1], Cam.localPosition[2] + camOffset[2]);

        }

        if (camRotate)
        {

            //angle[1] = Cam.localEulerAngles[1];
           // angle[0] = Cam.localEulerAngles[0];
            //angle[2] = Cam.localEulerAngles[2];


            transform.localEulerAngles = angle;
        }

        if (rotate)
        {
           Vector3 touchRot;
            if (isRightHanded)
            {
                touchRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch).eulerAngles;
            }
            else
            {
                touchRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch).eulerAngles;
            }

            transform.localEulerAngles = new Vector3(touchRot[0], touchRot[1], touchRot[2]);
        }


        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            rotate = !rotate;
            translate = !translate;

        }



        if (OVRInput.GetUp(OVRInput.Button.Two))
        {
            translate = !translate;
            rotate = !rotate;

        }

        //if (rotate && OVRInput.GetUp(OVRInput.Button.One))
        //{
        //    transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        //}
    }
}
