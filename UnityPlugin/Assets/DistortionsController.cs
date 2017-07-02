using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Linq;
using System.IO;

[System.Serializable]
public class Illusion
{
    public Dictionary<string, Vector3> prop;
    public Dictionary<string, Vector3> size;
    public Dictionary<string, List<float>> dof;
    public Dictionary<string, List<float>> kink;
    public Vector3 vel;
    public Vector3 pos;
    public bool fixedPos;
    public string text;
    public bool practice;
    public Vector3 origin;
    public bool originOffset;
    public Vector3 tarPos;
    public string room;
    public Vector3 obsPos;
    public int handId;
    public Vector3 propSize;
    public List<string> hide;
    public Dictionary<string, string> parent;
    public Vector3 virtualPropPos;
    public Quaternion virtualPropRot;
    public List<string> sticks;
    public bool solid;

    public Illusion(JsonData illusions)
    {
        size = new Dictionary<string, Vector3>();
        dof = new Dictionary<string, List<float>>();
        kink = new Dictionary<string, List<float>>();
        prop = new Dictionary<string, Vector3>();
        vel = new Vector3(1.0f, 1.0f, 1.0f);
        pos = new Vector3(0.0f, 0.0f, 0.0f);
        fixedPos = false;
        text = null;
        practice = false;
        originOffset = false;
        origin = new Vector3(0.0f, 0.0f, 0.0f);
        tarPos = new Vector3(0.0f, 0.0f, 0.0f);
        obsPos = new Vector3(0.0f, 0.0f, 0.0f);
        room = null;
        handId = 0;
        propSize = new Vector3(0.0f, 0.0f, 0.0f);
        hide = new List<string>();
        parent = new Dictionary<string, string>();
        virtualPropPos = Vector3.zero;
        virtualPropRot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        solid = false;
        sticks = new List<string>();

        if (illusions["vel"] != null)
        {
            vel = new Vector3(float.Parse((string)illusions["vel"][0]), 
                float.Parse((string)illusions["vel"][1]), 
                float.Parse((string)illusions["vel"][2]));
        }


        if (illusions["size"] != null)
        {
            for (int i = 0; i < illusions["size"].Count; i ++)
            {
                List<string> vals = ((string)illusions["size"][i]).Split(':').ToList<string>();
                size.Add(vals[0], new Vector3(float.Parse(vals[1]), float.Parse(vals[2]), float.Parse(vals[3])));
            }
        }

        if (illusions["prop"] != null)
        {
            for (int i = 0; i < illusions["prop"].Count; i++)
            {
                List<string> vals = ((string)illusions["prop"][i]).Split(':').ToList<string>();
                prop.Add(vals[0], new Vector3(float.Parse(vals[1]), float.Parse(vals[2]), float.Parse(vals[3])));
            }
        }


        if (illusions["dof"] != null)
        {
            for (int i = 0; i < illusions["dof"].Count; i++)
            {
                List<string> vals = ((string)illusions["dof"][i]).Split(':').ToList<string>();
                dof.Add(vals[0], new List<float> { float.Parse(vals[1]), float.Parse(vals[2]),
                    float.Parse(vals[3]), float.Parse(vals[4]), float.Parse(vals[5]), float.Parse(vals[6])});
            }
        }

        if (illusions["pos"] != null)
        {
            fixedPos = true;
            pos = new Vector3(float.Parse((string)illusions["pos"][0]),
                float.Parse((string)illusions["pos"][1]),
                float.Parse((string)illusions["pos"][2]));
        }

        if (illusions["break"] != null)
        {
            for (int i = 0; i < illusions["break"].Count; i++)
            {
                List<string> vals = ((string)illusions["break"][i]).Split(':').ToList<string>();
                kink.Add(vals[0], new List<float> { float.Parse(vals[1]), float.Parse(vals[2]),
                    float.Parse(vals[3]), float.Parse(vals[4]), float.Parse(vals[5]), float.Parse(vals[6])});
            }
        }

        if (illusions["text"] != null)
        {
            text = (string)illusions["text"];
        }

        if (illusions["practice"] != null)
        {
            practice = (bool)illusions["practice"];
        }

        if (illusions["origin"] != null)
        {
            originOffset = true;
            origin = new Vector3(float.Parse((string)illusions["origin"][0]),
                float.Parse((string)illusions["origin"][1]),
                float.Parse((string)illusions["origin"][2]));
        }

        if (illusions["room"] != null)
        {
            room = (string)illusions["room"];
        }

        if (illusions["tarPos"] != null)
        {
            tarPos = new Vector3(float.Parse((string)illusions["tarPos"][0]),
                            float.Parse((string)illusions["tarPos"][1]),
                            float.Parse((string)illusions["tarPos"][2]));
        }


        if (illusions["obsPos"] != null)
        {
            obsPos = new Vector3(float.Parse((string)illusions["obsPos"][0]),
                            float.Parse((string)illusions["obsPos"][1]),
                            float.Parse((string)illusions["obsPos"][2]));
        }

        if (illusions["handId"] != null)
        {
            handId = (int)illusions["handId"];
        }

        if (illusions["propSize"] != null)
        {
            propSize = new Vector3(float.Parse((string)illusions["propSize"][0]),
                            float.Parse((string)illusions["propSize"][1]),
                            float.Parse((string)illusions["propSize"][2]));
        }

        if (illusions["hide"] != null)
        {
            for (int i = 0; i < illusions["hide"].Count; i++)
            {
                hide.Add((string)illusions["hide"][i]);
            }
        }

        if (illusions["parent"] != null)
        {
            for (int i = 0; i < illusions["parent"].Count; i++)
            {
                List<string> vals = ((string)illusions["parent"][i]).Split(':').ToList<string>();
                parent.Add(vals[0], vals[1]);
            }
        }

        if (illusions["virtualPropPos"] != null)
        {
            virtualPropPos = new Vector3(float.Parse((string)illusions["virtualPropPos"][0]),
                            float.Parse((string)illusions["virtualPropPos"][1]),
                            float.Parse((string)illusions["virtualPropPos"][2]));
            virtualPropRot = Quaternion.Euler(new Vector3(float.Parse((string)illusions["virtualPropPos"][3]),
                            float.Parse((string)illusions["virtualPropPos"][4]),
                            float.Parse((string)illusions["virtualPropPos"][5])));
        }

        //if (illusions["solid"] != null)
        //{
        //    solid = (bool)illusions["solid"];
        //}

        //if (illusions["sticks"] != null)
        //{
        //    for (int i = 0; i < illusions["sticks"].Count; i++)
        //    {
        //        sticks.Add((string)illusions["sticks"][i]);
        //    }
        //}


    }

}

public class DistortionsController : MonoBehaviour {

    public string filename;
    public Dictionary<int, Illusion> illusions;
    public int trialId;
    public bool needUpdate;
    public int numTrials;
    public int numTrialsCompleted;

    // Use this for initialization
    void Start () {
        trialId = -1;
        numTrials = 0;
        numTrialsCompleted = 0;
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

    public void StartNewTrial(int id)
    {
        trialId = id;
        numTrialsCompleted = numTrialsCompleted + 1;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
