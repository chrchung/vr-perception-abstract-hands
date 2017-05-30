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

    }

}

public class DistortionsController : MonoBehaviour {

    public string filename;
    public Dictionary<int, Illusion> illusions;
    public int trialId;
    public bool needUpdate;
    public int numTrials;

    // Use this for initialization
    void Start () {
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
	void Update () {
		
	}
}
