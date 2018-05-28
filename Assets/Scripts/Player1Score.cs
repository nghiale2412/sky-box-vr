using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1Score : MonoBehaviour {

    private Text txt;
    public static Player1Score player1ScoreInstance = null;
    string ScoreText = "";

    void Awake()
    {
        //Check if instance already exists
        if (player1ScoreInstance == null)

            //if not, set instance to this
            player1ScoreInstance = this;

        //If instance already exists and it's not this:
        else if (player1ScoreInstance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    public void Start () {
        txt = GetComponent<Text>();
	}

    public void SetScore(string scoreStr)
    {
        ScoreText = "" + scoreStr;
    }

        /*
    public void UpdateScore()
    {
        score++;
    }
   

    public string GetScore()
    {
        return score.ToString();
    }
    */
	
	// Update is called once per frame
	void Update () {
        txt.text = "Your score: " + ScoreText;
	}
}
