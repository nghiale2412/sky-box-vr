using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MainMenu : MonoBehaviour {

    private void Start()
    {
        XRSettings.enabled = false;
        Debug.Log("MENU SCENE START!");
    }
    public void PlayGame()
    {
        Debug.Log("PLAY!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
