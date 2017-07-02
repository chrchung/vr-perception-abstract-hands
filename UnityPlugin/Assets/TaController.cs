using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaController : MonoBehaviour {

    public bool isCollision = false;
    public Color col1;
    public Color col2;

    void Awake()
    {
       
    }

    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().material.color = col1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Tile01")
        {
            isCollision = true;
            GetComponent<Renderer>().material.color = col2;
        }

    }

    void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material.color = col1;
    }

}
