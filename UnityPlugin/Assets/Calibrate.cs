using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrate : MonoBehaviour {

    public Transform Obj1;
    public Transform Obj2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(Obj1.localPosition, Obj2.localPosition) < 0.05)
        {
            print("Within threshold");
        }	
	}
}
