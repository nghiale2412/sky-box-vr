using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

    

    // Use this for initialization
    void Start () {
        float moveAreaX = gameObject.GetComponent<Renderer>().bounds.size.x / 2;
        float moveAreaY = gameObject.GetComponent<Renderer>().bounds.size.y / 2;
        float moveAreaZ = gameObject.GetComponent<Renderer>().bounds.size.z / 2;
        Vector3 center = gameObject.GetComponent<Renderer>().bounds.center;
        Debug.Log("x: " + moveAreaX + "y: " + moveAreaY + "z: " + moveAreaZ + "center: " + center);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
