using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RigidController : MonoBehaviour {

    List<string> collided;
    Rigidbody rigidbody;


	// Use this for initialization
	void Start () {
        collided = new List<string>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
	}

    void OnCollisionEnter(Collision other)
    {
        ////gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ////gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        ////if (other.gameObject.name == "Sphere")
        ////{
        ////    collider = other.gameObject;

        ////}


        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit))
        //{
        //   // Debug.Log("Point of contact: " + hit.point);

        //}
       

        if (other.gameObject.name.Contains("Sphere") || other.gameObject.name.Contains("Cylinder"))
        {
            string joint = other.gameObject.name.Substring(other.gameObject.name.Length - 4);
            print(joint);
            collided.Add(joint);

        }



    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name.Contains("Sphere") || other.gameObject.name.Contains("Cylinder"))
        {
            string joint = other.gameObject.name.Substring(other.gameObject.name.Length - 4);
            int index = collided.IndexOf(joint);

            collided.RemoveAt(index);
        }

    }


    // Update is called once per frame
    void Update () {
        int d1 = collided.IndexOf("R1D1");
        int d2 = collided.IndexOf("R1D2");

        if (collided.Count >= 2 && (d1 != -1 || d2 != -1)) {
            //if (d1 != - 1)
            //{
            //    transform.parent = GameObject.Find("R1D1").transform;
            //}
            //else
            //{
            //    transform.parent = GameObject.Find("R1D2").transform;
            //}

            transform.parent = GameObject.Find("R3D1").transform;

            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;

            Transform temp = transform.Find("b");
            Transform temp2 = temp.Find("bc");

            temp2.localEulerAngles = Vector3.zero;
            temp.localPosition = new Vector3(0.0f, 0.0f, -temp2.localScale[2] / 1.7f);
            
       

            rigidbody.isKinematic = true;

            // rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        }

        //else if (collided.Count < 2)
        //{
        //    transform.parent = null;
        //    rigidbody.isKinematic = false;
        //}
        //if (transform.parent.gameObject.name == "R3D2")
        //{
           
        //}
    }

    public void Reset()
    {
        transform.parent = GameObject.Find("ViconObjects").transform;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;

        Transform temp = transform.Find("b");
        Transform temp2 = temp.Find("bc");
        temp2.localEulerAngles = Vector3.zero;
    }
}
