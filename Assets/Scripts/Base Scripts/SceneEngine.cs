using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEngine : MonoBehaviour
{

    //Disable buttons use when tutorial is open...

    public GameObject tutorialWindow;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartOverLevel()
    {
        Scene sceneLoaded = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneLoaded.buildIndex);
    }

    public void NextScene(string nextASceneName)
    {
        Scene sceneLoaded = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneLoaded.buildIndex + 1);

        //SceneManager.LoadScene(nextASceneName, LoadSceneMode.Additive);
    }


    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
        //SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
    }


    public void ToInfo()
    {
        //SceneManager.LoadScene(12);
    }


    public void CloseTutorial()
    {
        tutorialWindow.SetActive(false);
        
    }
}
