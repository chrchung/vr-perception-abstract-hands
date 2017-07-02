using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using LitJson;
using System.IO;

[System.Serializable]
public class Results
{
    public int trialId;
    public DateTime start;
    public DateTime end;
    public string task;
    public bool isTrial;

    public Results(int trialId, string task, DateTime start, DateTime end, bool isTrial)
    {
        this.trialId = trialId;
        this.task = task;
        this.start = start;
        this.end = end;
        this.isTrial = isTrial;
    }
}

[System.Serializable]
public class TrackingData
{
    public DateTime time;
    public Dictionary<string, List<string>> data;

    public TrackingData(DateTime time)
    {
        this.time = time;
        data = new Dictionary<string, List<string>>();
    }

    public void AddData(string bone, Vector3 localRotation, Vector3 localPosition, Vector3 globalRotation, Vector3 globalPosition)
    {
        List<string> temp = new List<string>() {
            localRotation[0].ToString(), localRotation[1].ToString(), localRotation[2].ToString(),
            globalRotation[0].ToString(), globalRotation[1].ToString(), globalRotation[2].ToString(),
            localPosition[0].ToString(), localPosition[1].ToString(), localPosition[2].ToString(),
            globalPosition[0].ToString(), globalPosition[1].ToString(), globalPosition[2].ToString()
        };

        data.Add(bone, temp);
    }

}

public class ExperimentController : MonoBehaviour {

    List<DistortionsController> tasks;
    List<int[]> trials;
    public int len;
    List<int[]> practiceTrials;
    
    DistortionsController handDistortionsController;
    public DistortionsController cubeDistortionsController;

    public bool done;
    public int trialNo;
    private List<Results> results = new List<Results>();
    public int participantId;

    public TextMesh modeText;
    public TextMesh timerText;

    public float totalTime;
    public float curTrialPrevTime;
    public bool countTime;


    // Use this for initialization
    void Start () {
        var arr  = FindObjectsOfType(typeof(DistortionsController)) as DistortionsController[];
        tasks = new List<DistortionsController>(arr);
        practiceTrials = new List<int[]>();
        trials = new List<int[]>();
        trialNo = 0;
        done = true;
        modeText.text = "Practice";

        totalTime = 0;
        curTrialPrevTime = 0;
        countTime = false;

        int lim = tasks.Count;
        for (int i = 0; i < lim; i ++)
        {
            if (tasks[i].filename == "/TrialData/trialdata.txt")
            {
                handDistortionsController = tasks[i];
                tasks.RemoveAt(i);
                i--;
            }

            else if (tasks[i].filename == "/TrialData/trialdata5.txt")
            {
                cubeDistortionsController = tasks[i];

                tasks.RemoveAt(i);
                i--;
            } else
            {
                for (int j = 0; j < tasks[i].numTrials; j++)
                {
                    int[] trial = new int[] { i, j };
                    if (tasks[i].illusions[j].practice)
                    {
                        practiceTrials.Add(trial);
                    }
                    else
                    {
                        trials.Add(trial);
                    }
                }
            }


        }



        Shuffle(practiceTrials);
        Shuffle(trials);
    }

    // Update is called once per frame
    void Update()
    {
        //print(done);
        //if (OVRInput.GetUp(OVRInput.Button.One))
        //{
        //    done = true;
        //}

        if (countTime)
        {
            UpdateTimer();
        }



        switch (modeText.text)
        {
            case "Practice":
                if (done && trialNo < practiceTrials.Count - 1)
                {
                    trialNo++;
                    SetupTrial(practiceTrials);
                    done = false;
                    
                } else if (done & trialNo == practiceTrials.Count - 1) {
                    trialNo = 0;
                    done = false;
                    modeText.text = "The real experiment begins now. Press the A button on your controller to start.";
                }
                break;
            case "The real experiment begins now. Press the A button on your controller to start.":
                if (OVRInput.GetUp(OVRInput.Button.One))
                {
                    modeText.text = "";
                    SetupTrial(trials);
                    done = false;
                }
                break;
            case "":
                if (done && trialNo < trials.Count - 1)
                {
                    trialNo++;
                    SetupTrial(trials);
                    done = false;
                } else
                {
                    Save();
                }
                break;
        }
    
    }

    void SetupTrial(List<int[]> trials)
    {
        len = trials.Count;
        countTime = false;

        tasks[trials[trialNo][0]].StartNewTrial(trials[trialNo][1]);

        handDistortionsController.StartNewTrial(tasks[trials[trialNo][0]].illusions[trials[trialNo][1]].handId);

        if (tasks[trials[trialNo][0]].filename == "/TrialData/trialdata4.txt")
        {
            cubeDistortionsController.gameObject.SetActive(false);

            //cubeDistortionsController.gameObject.SetActive(true);
            //cubeDistortionsController.trialId = trials[trialNo][1];
        }
        else
        {
            cubeDistortionsController.gameObject.SetActive(false);
        }

        //for (int i = 0; i < tasks.Count; i++)
        //{
        //    if (i != trials[trialNo][0])
        //    {
        //        tasks[i].transform.parent.gameObject.SetActive(False);
        //    }

        //}
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
    
    public void EndTrial(int trialId, string task, DateTime start, DateTime end, bool isTrial)
    {
        done = true;
        CreateNewResult(trialId, task, start, end, isTrial);
    }

    public void CreateNewResult(int trialId, string task, DateTime start, DateTime end, bool isTrial)
    {
        Results r = new Results(trialId, task, start, end, isTrial);
        results.Add(r);
    }

    public void Save()
    {
        JsonData json = JsonMapper.ToJson(results);
        File.WriteAllText(Application.dataPath + "/TrialData/" + participantId + ".txt", json.ToString());
    }

    public void ResumeTimer()
    {
        curTrialPrevTime = Time.time;
        countTime = true;
    }

    public void UpdateTimer()
    {
        totalTime = totalTime + Time.time - curTrialPrevTime;
        curTrialPrevTime = Time.time;

        timerText.text = "Time: " + totalTime.ToString();
    }

    public void UpdateProp()
    {
        cubeDistortionsController.gameObject.SetActive(true);
        
        switch (modeText.text)
        {
            case "Practice":
                cubeDistortionsController.trialId = practiceTrials[trialNo][1];
                break;
            case "The real experiment begins now. Press the A button on your controller to start.":
                break;
            case "":
                cubeDistortionsController.trialId = trials[trialNo][1];
                break;
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
