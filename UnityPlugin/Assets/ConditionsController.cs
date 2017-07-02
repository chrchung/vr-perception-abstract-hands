using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Linq;
using System.IO;


public class ConditionsController : MonoBehaviour
{

    public string filename;
    public Dictionary<int, Illusion> illusions;
    public int trialId;
    public bool needUpdate;
    public int numTrials;

    // Use this for initialization
    void Start()
    {
        trialId = -1;
        numTrials = 0;
        illusions = new Dictionary<int, Illusion>();
        string jsonString = File.ReadAllText(Application.dataPath + filename);

        JsonData jsonData = JsonMapper.ToObject(jsonString);

        for (int i = 0; i < jsonData["data"].Count; i++)
        {
            // parse illusion
            Illusion ill = new Illusion(jsonData["data"][i]);
            illusions.Add((int)jsonData["data"][i]["trialId"], ill);
            numTrials = numTrials + 1;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
