using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollision : MonoBehaviour {

    void OnCollisionEnter(Collision other)
    {
        print(other.gameObject.name);

    }

    void OnCollisionExit(Collision other)
    {

        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        transform.localPosition = Vector3.zero;
        transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
