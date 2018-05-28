using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetIPScript : MonoBehaviour {

    public InputField IP;
	// Use this for initialization
	void Start () {
        XRSettings.enabled = false;
        Debug.Log("GetIP SCENE START!");
	}
	
    public void GetIP()
    {
        Debug.Log("SERVER IP: " + IP.text);
        PlayerPrefs.SetString("gameserver_ip", IP.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
