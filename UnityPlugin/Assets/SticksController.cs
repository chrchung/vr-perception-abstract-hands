using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SticksController : MonoBehaviour {

    public DistortionsController distortionsController;

	// Use this for initialization
	void Start () {
       
        for (int i = 0; i < 16; i++)
        {
            GameObject Stick = new GameObject(i.ToString());
            Stick.transform.parent = transform;
            Stick.AddComponent<LineRenderer>();

        }

   
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DrawSticks(int trialId)
    {

        Reset();

        for (int i = 0; i < distortionsController.illusions[trialId].sticks.Count; i++)
        {
            string boneNames = distortionsController.illusions[trialId].sticks[i];
            LineRenderer lr = transform.Find(i.ToString()).gameObject.GetComponent<LineRenderer>();
            string[] s = boneNames.Split(':');

            Vector3 start = GameObject.Find(s[0]).transform.position;
            Vector3 end = GameObject.Find(s[1]).transform.position;

            DrawStick(lr, start, end);
        }
    }

    void DrawStick(LineRenderer lr, Vector3 start, Vector3 end)
    {
        lr.useWorldSpace = false;
        lr.material = new Material(Shader.Find("Particles/Additive"));
        lr.SetColors(Color.black, Color.black);
        lr.SetWidth(0.01f, 0.01f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

    }


    void Reset()
    {
        foreach (Transform child in transform)
        {
            LineRenderer lr = child.gameObject.GetComponent<LineRenderer>();
            lr.SetPosition(0, Vector3.zero);
            lr.SetPosition(1, Vector3.zero);
        }
    }
}
