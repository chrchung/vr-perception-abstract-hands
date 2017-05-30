using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureController : MonoBehaviour {


    // public int[] modes;

    // private trial 

    private float speed = 0f;

    public GameObject harmful;
    public GameObject unharmful;


	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    // 0 = saw, 1 = wood
    public void UpdateObstacle(string mode,  Vector3 scale)
    {
        Debug.Log(mode);
        Debug.Log(scale);

        if (mode == "blade") {
            harmful.SetActive(true);
            unharmful.SetActive(false);
        } else {
            harmful.SetActive(false);
            unharmful.SetActive(true);
        }

        //transform.localPosition = pos;
        transform.localScale = scale;
    }

    private void reshuffle(int[] ar)
    {
        for (int i = 0; i < ar.Length; i++) {
            int tmp = ar[i];
            int r = Random.Range(i, ar.Length);
            ar[i] = ar[r];
            ar[i] = tmp;
        }
    }
}
