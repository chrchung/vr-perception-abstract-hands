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

    public int trialId;


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
        if (distortionsController.trialId != trialId)
        {
            trialId = distortionsController.trialId;
            SetupNextTrial();
        }



        if (mode == 1)
        {
            Debug.Log(mode);

            //Instructions.text = "Touch the pink square when you are ready to start the next trial. A new pink square will target. Touch it as quickly as you can.";
            if (origin.GetComponent<TaController>().isCollision)
            {
                Debug.Log(mode);

                start = Time.time;
                t = Time.time - start;
                UpdateTars(target, origin, false);
                mode = 2;
            }
        }
        else if (mode == 2)
        {
            //Instructions.text = "";
            //score.text = "Time:" + (t + Time.time - start).ToString();

            if (target.GetComponent<TaController>().isCollision)
            {

                mode = 3;
            
            }
        }

    }


    void UpdateTars(GameObject on, GameObject off, bool isTar)
    {
        on.SetActive(true);
        off.SetActive(false);
        off.GetComponent<TaController>().isCollision = false;

        if (isTar)
        {
            off.transform.localPosition = distortionsController.illusions[distortionsController.trialId].pos;
        }
    }

    public void SetupNextTrial()
    {
        UpdateTars(origin, target, true);        
        obstacle.GetComponent<TextureController>().UpdateObstacle(distortionsController.illusions[distortionsController.trialId].text, distortionsController.illusions[distortionsController.trialId].size["obstacle"]);
        mode = 1;
    }
}