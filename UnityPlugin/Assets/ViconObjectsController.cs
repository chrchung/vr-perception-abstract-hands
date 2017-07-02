using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestUnityVicon;

public class ViconObjectsController : MonoBehaviour {

    List<Program> handTextures;
    public int trialId;
    public DistortionsController distortionsController;

	// Use this for initialization
	void Start () {
        var arr = FindObjectsOfType(typeof(Program)) as Program[];
        handTextures = new List<Program>(arr);
        trialId = -1;


        int lim = handTextures.Count;
        for (int i = 0; i < lim; i++)
        {
            handTextures[i].gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (distortionsController.trialId != trialId)
        {
            trialId = distortionsController.trialId;

            int lim = handTextures.Count;
            for (int i = 0; i < lim; i++)
            {
                //if (handTextures[i].gameObject.name == distortionsController.illusions[trialId].renderStyle)
                //{

                if (handTextures[i].gameObject.name == "BodyTexturedHands") { 
                    handTextures[i].gameObject.SetActive(true);
                } else
                {
                    handTextures[i].gameObject.SetActive(false);
                }
            }


        }
    }
}
