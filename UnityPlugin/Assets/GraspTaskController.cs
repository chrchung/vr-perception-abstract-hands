using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GraspTaskController : MonoBehaviour {

    public DistortionsController distortionsController;

    private DateTime trialStart;
    private DateTime trialEnd;
    public ExperimentController experimentController;
    public int trialId;
    public int mode;

    public TextMesh instructions;

    public GameObject origin;
    public GameObject obstacle;
    public GameObject cube;

    // Use this for initialization
    void Start () {
        mode = 3;
        trialId = -1;


    }

    // Update is called once per frame
    void Update()
    {
        if (distortionsController.trialId != -1 && distortionsController.trialId != trialId)
        {
            UpdateTars(origin, cube, true);
            trialId = distortionsController.trialId;
            mode = 1;
            trialStart = System.DateTime.Now;

            instructions.text = "Place your hand on the pink square.";
        }


        if (mode == 1)
        {
            if (origin.GetComponent<TaController>().isCollision)
            {
                

                UpdateTars(cube, origin, false);

                instructions.text = "Move the white cube to match the position and orientation of the red cube. Press A when you are done.";
 
                trialEnd = System.DateTime.Now;

                trialStart = System.DateTime.Now;
                mode = 2;

                experimentController.ResumeTimer();

            }
        }
        else if (mode == 2)
        {
            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                trialEnd = System.DateTime.Now;
                mode = 3;
                UpdateTars(null, cube, false);
                obstacle.SetActive(false);
                experimentController.EndTrial(trialId, distortionsController.filename, trialStart, trialEnd, true);

            }
        }
    }

    void UpdateTars(GameObject on, GameObject off, bool isTar)
    {
        if (on)
        {
            on.SetActive(true);

            if (!isTar)
            {
                obstacle.SetActive(true);
                experimentController.UpdateProp();            

            }
            else
            {
                on.GetComponent<TaController>().isCollision = false;

            }

        }

        if (off)
        {
            off.SetActive(false);

            if (distortionsController.illusions[distortionsController.trialId].size.ContainsKey("obstacle"))
            {
                obstacle.GetComponent<TextureController>().UpdateObstacle(distortionsController.illusions[distortionsController.trialId].text,
                distortionsController.illusions[distortionsController.trialId].size["obstacle"], distortionsController.illusions[distortionsController.trialId].obsPos);
            }
            else
            {
                obstacle.GetComponent<TextureController>().UpdateObstacle(distortionsController.illusions[distortionsController.trialId].text,
                new Vector3(0.0f, 0.0f, 0.0f), distortionsController.illusions[distortionsController.trialId].obsPos);
            }


            if (isTar)
            {
                var temp = distortionsController.illusions[distortionsController.trialId].pos;
                temp[2] = temp[2] + distortionsController.illusions[distortionsController.trialId].propSize[2] / 2.0f;

                cube.transform.localPosition = temp;
                cube.transform.localScale = distortionsController.illusions[distortionsController.trialId].propSize;



            }
        }
    }
}
