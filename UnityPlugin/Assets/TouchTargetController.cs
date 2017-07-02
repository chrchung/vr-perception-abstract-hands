using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;
using System.Linq;
using UnityEngine.UI;
using TestUnityVicon;


public class TouchTargetController : MonoBehaviour
{

    // 0, 1, 2
    public int mode;
    public GameObject origin;
    public GameObject target;
    public GameObject obstacle;
    //public Text score;
    //public Text Instructions;
    private float start;

    private float t;
    public DistortionsController distortionsController;
    public ExperimentController experimentController;
    public int trialId;

    private DateTime trialStart;
    private DateTime trialEnd;

    public TextMesh instructions;


    // Use this for initialization
    void Start()
    {
        trialId = -1;
        start = 0f;
        mode = 0;
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (distortionsController.trialId != -1 && distortionsController.trialId != trialId)
        {
            trialId = distortionsController.trialId;
            SetupNextTrial();
        }


        if (mode == 1)
        {
            if (origin.GetComponent<TaController>().isCollision)
            {

                trialEnd = System.DateTime.Now;

                start = Time.time;
                trialStart = System.DateTime.Now;
                t = Time.time - start;
                UpdateTars(target, origin, false);
                mode = 2;
                instructions.text = "Place your hand on the pink square that has appeared.";

                experimentController.ResumeTimer();

            }
        }
        else if (mode == 2)
        {
            //Instructions.text = "";
            //score.text = "Time:" + (t + Time.time - start).ToString();

            if (target.GetComponent<TaController>().isCollision)
            {
                trialEnd = System.DateTime.Now; 
                mode = 3;
                UpdateTars(null, target, false);
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
            on.GetComponent<TaController>().isCollision = false;

            if (!isTar)
            {
                obstacle.SetActive(true);
            }
        }

        if (off)
        {
            off.SetActive(false);
            off.GetComponent<TaController>().isCollision = false;

            if (isTar)
            {
                off.transform.localPosition = distortionsController.illusions[distortionsController.trialId].tarPos;
            }
        }
    }

    public void SetupNextTrial()
    {
        UpdateTars(origin, target, true);        
        obstacle.GetComponent<TextureController>().UpdateObstacle(distortionsController.illusions[distortionsController.trialId].text, 
            distortionsController.illusions[distortionsController.trialId].size["obstacle"], distortionsController.illusions[distortionsController.trialId].obsPos);
        mode = 1;

        instructions.text = "Check if your finger corresponds to each virtual one then, place your hand on the pink square.";


        trialStart = System.DateTime.Now;
    }
}