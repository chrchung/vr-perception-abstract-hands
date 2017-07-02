using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MatchHandTaskController : MonoBehaviour
{

    public DistortionsController distortionsController;

    private DateTime trialStart;
    private DateTime trialEnd;
    public ExperimentController experimentController;
    public int trialId;
    public int mode;

    public TextMesh instructions;

    public GameObject origin;
    public GameObject obstacle;
    public GameObject hand;

    // Use this for initialization
    void Start()
    {
        mode = 3;
        trialId = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (distortionsController.trialId != -1 && distortionsController.trialId != trialId)
        {
            trialId = distortionsController.trialId;
            mode = 1;
            trialStart = System.DateTime.Now;
            UpdateTars(origin, hand, true);
            origin.GetComponent<TaController>().isCollision = false;

            //if (distortionsController.illusions[trialId].practice)
            //{
            //    instructions.text = "*This is for practice only* \n Place your hand on the pink square.";
            //} else
            //{
            instructions.text = "Place your hand on the pink square.";
            //}
        }


        if (mode == 1)
        {
            if (origin.GetComponent<TaController>().isCollision)
            {
                origin.GetComponent<TaController>().isCollision = false;
                UpdateTars(hand, origin, false);

                trialEnd = System.DateTime.Now;
                trialStart = System.DateTime.Now;
                mode = 2;
        
                instructions.text = "Match the position and orientation of the red hand.";

                experimentController.ResumeTimer();
                
            }
        }
        else if (mode == 2)
        {
            if (OVRInput.GetUp(OVRInput.Button.One))
            {
                trialEnd = System.DateTime.Now;
                mode = 3;
                UpdateTars(null, hand, false);

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
            } else
            {
                
            }

        }

        if (off)
        {
            off.SetActive(false);
           

            if (distortionsController.illusions[distortionsController.trialId].size.ContainsKey("obstacle"))
            {
                obstacle.GetComponent<TextureController>().UpdateObstacle(distortionsController.illusions[distortionsController.trialId].text,
                distortionsController.illusions[distortionsController.trialId].size["obstacle"], distortionsController.illusions[distortionsController.trialId].obsPos);
            } else
            {
                obstacle.GetComponent<TextureController>().UpdateObstacle(distortionsController.illusions[distortionsController.trialId].text,
                new Vector3(0.0f, 0.0f, 0.0f), distortionsController.illusions[distortionsController.trialId].obsPos);
            }



            if (isTar)
            {
                off.transform.localPosition = distortionsController.illusions[distortionsController.trialId].pos;
            }
        }
    }
}
