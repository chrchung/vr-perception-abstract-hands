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
    public Dictionary<string, Vector3> dof;
    public Vector3 vel;

    public Illusion(JsonData illusions)
    {
        size = new Dictionary<string, Vector3>();
        dof = new Dictionary<string, Vector3>();
        prop = new Dictionary<string, Vector3>();
        vel = new Vector3(1.0f, 1.0f, 1.0f);

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
                dof.Add(vals[0], new Vector3(float.Parse(vals[1]), float.Parse(vals[2]), float.Parse(vals[3])));
            }
        }

    }

}

public class DistortionsController : MonoBehaviour {

    public string filename;
    public Dictionary<int, Illusion> illusions;
    public int trialId;
    public bool needUpdate;

    // Use this for initialization
    void Start () {
        illusions = new Dictionary<int, Illusion>();
        string jsonString = File.ReadAllText(Application.dataPath + filename);

        JsonData jsonData = JsonMapper.ToObject(jsonString);

        for (int i = 0; i < jsonData["data"].Count; i++)
        {
            // parse illusion
            Illusion ill = new Illusion(jsonData["data"][i]);
            illusions.Add((int)jsonData["data"][i]["trialId"], ill);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
