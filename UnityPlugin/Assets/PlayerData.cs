using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Results(int trialId, string task, DateTime start, DateTime end)
    {
        this.trialId = trialId;
    }
}

public class PlayerData : MonoBehaviour {

    private List<Results> results = new List<Results>();
    public int participantId;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateNewResult (int trialId, string task, DateTime start, DateTime end)
    {
        Results r = new Results(trialId, task, start, end);
        results.Add(r);
    }

    public void Save()
    {
        JsonData json = JsonMapper.ToJson(results);
        File.WriteAllText(Application.dataPath + "/TrialData/" + participantId + ".txt", json.ToString());
    }
}
