using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class SplashScreenText : MonoBehaviour {

    private Text txt;
    private string textTemp = "From now on, you and your opponent will be locked in a box";
    private string textTemp1 = "You will move automatically in the direction that you are looking at";
    private string textTemp2 = "Your only objective is to collect " + "\n" + "the keys";
    private string textTemp3 = "Go through them is the only way to do it";
    private string textTemp4 = "Who has more keys will be the winner";
    private string textTemp5 = "Easy right? :-)" +"\n" + "Getting your VR headset ready!" + "\n" + "The game will begin shortly...";
   
    IEnumerator Start()
    {
        Debug.Log("SPLASH SCREEN SCENE START!");
        XRSettings.enabled = false;
        txt = GetComponent<Text>();
        yield return new WaitForSeconds(3f);
        SetText(textTemp);
        yield return new WaitForSeconds(7f);
        SetText(textTemp1);
        yield return new WaitForSeconds(6f);
        SetText(textTemp2);
        yield return new WaitForSeconds(5f);
        SetText(textTemp3);
        yield return new WaitForSeconds(5f);
        SetText(textTemp4);
        yield return new WaitForSeconds(5f);
        SetText(textTemp5);
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void SetText(string textTem)
    {
        txt.text = textTem;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
