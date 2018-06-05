using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class WinnerMessage : MonoBehaviour {

    private Text txt;

    public static WinnerMessage winnerMessageInstance = null;

    void Awake()
    {
        //Check if instance already exists
        if (winnerMessageInstance == null)

            //if not, set instance to this
            winnerMessageInstance = this;
        
        //If instance already exists and it's not this:
        else if (winnerMessageInstance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        XRSettings.enabled = false;
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        txt.text = CustomNetwork.instance.GetWinnerInfo();
	}
}
