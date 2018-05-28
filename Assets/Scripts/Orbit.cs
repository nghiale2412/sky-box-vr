using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

    private void Awake()
    {
        gameObject.tag = "Sphere";
    }

    // Update is called once per frame
    void Update () {
		transform.Rotate(new Vector3(0, Time.deltaTime*100, 0));
	}
}
