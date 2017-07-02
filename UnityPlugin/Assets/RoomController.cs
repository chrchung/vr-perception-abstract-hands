using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

    private List<GameObject> Rooms;

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform)
        {
           Rooms.Add(child.gameObject);

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SetupRoom(string room)
    {
        for (var i = 0; i < Rooms.Count; i++)
        {
            if (Rooms[i].name != room)
            {
                Rooms[i].SetActive(false);
            } else
            {
                Rooms[i].SetActive(true);
            }

        }

    }
}
