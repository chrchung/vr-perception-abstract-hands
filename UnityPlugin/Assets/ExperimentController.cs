using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExperimentController : MonoBehaviour {

    List<DistortionsController> tasks;
    List<int[]> trials;
    DistortionsController hand;
    public bool done;
    int trialNo;

    // Use this for initialization
    void Start () {
        var arr  = FindObjectsOfType(typeof(DistortionsController)) as DistortionsController[];
        tasks = new List<DistortionsController>(arr);
        trials = new List<int[]>();
        trialNo = -1;   

        for (int i = 0; i < tasks.Count; i ++)
        {
            if (tasks[i].filename == "trialdata.txt")
            {
                hand = tasks[i];
                tasks.RemoveAt(i);
                continue;
            }

            for (int j = 0; j < tasks[i].numTrials; j++)
            {
                int[] trial = new int[]{i, j};
                trials.Add(trial);
            }
        }

        Shuffle(trials);

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            done = true;
        }

        if (done)
        {
            if (trialNo < trials.Count - 1)
            {
                trialNo++;
            }

            tasks[trials[trialNo][0]].trialId = trials[trialNo][1];

            done = false;
        }
    }

    public static void Shuffle(List<int[]> alpha)
    {
        for (int i = 0; i < alpha.Count; i++)
        {
            int[] temp = alpha[i];
            int randomIndex = UnityEngine.Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }

    //bool GetRandomTask()
    //{
    //    bool none = true;
    //    for (int i = 0; i < tasks.Count; i++)
    //    {
    //        if (tasks[i].trialNo < tasks[i].numTrials - 1)
    //        {
    //            none = false;
    //            break;
    //        }
    //    }

    //    if (none)
    //    {
    //        return none;
    //    }

    //    int ran = Random.Range(0, tasks.Length - 1);
       
    //    while(tasks[ran].trialNo >= tasks[ran].numTrials)
    //    {
    //        ran = Random.Range(0, tasks.Length - 1);
    //    }

    //    tasks[ran].trialNo++;

    //    return none;
    //}

}
