using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2OpponentScore : MonoBehaviour
{
    private Text txt;
    public static Player2OpponentScore player2OpponentScoreInstance = null;
    private string ScoreText = "";

    void Awake()
    {
        //Check if instance already exists
        if (player2OpponentScoreInstance == null)

            //if not, set instance to this
            player2OpponentScoreInstance = this;

        //If instance already exists and it's not this:
        else if (player2OpponentScoreInstance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    public void Start()
    {
        txt = GetComponent<Text>();
    }

    public void SetScore(string scoreStr)
    {
        ScoreText = "" + scoreStr;
    }


    public string GetScore()
    {
        return ScoreText;
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = "Opponent score: " + ScoreText;
    }
}