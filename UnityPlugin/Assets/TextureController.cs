using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.PyroParticles;

public class TextureController : MonoBehaviour {


    // public int[] modes;

    // private trial 

    private float speed = 0f;

    public List<GameObject> Obstacles;


	// Use this for initialization
	void Start ()
    {
        foreach (Transform child in transform)
        {
            Obstacles.Add(child.gameObject);
        }

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {

    }

    // 0 = saw, 1 = wood
    public void UpdateObstacle(string mode,  Vector3 scale, Vector3 pos)
    {
        for (var i = 0; i < Obstacles.Count; i++)
        {
            if (Obstacles[i].name != mode)
            {
                Obstacles[i].SetActive(false);
            }
            else
            {
                if (mode == "WallOfFire")
                {
                    Obstacles[i].GetComponent<FireConstantBaseScript>().enabled = true;
                }
                Obstacles[i].SetActive(true);
            }

        }

        if (scale != new Vector3(0.0f, 0.0f, 0.0f))
        {
            transform.localScale = scale;
        }

        if (pos != new Vector3(0.0f, 0.0f, 0.0f))
        {
            transform.localPosition = pos;
        }

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
