using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public OpeningManager om;
    public void OnGameRestart()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void OnGameQuit()
    {
        SceneManager.LoadScene("Opening");
    }
    public void OnGameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnGameStart()
    {
        om.SREButtonEnable(false);
        om.GoEnable(true);
    }
    public void OnGameRules()
    {
        om.SREButtonEnable(false);
        om.IntroductionButtonEnable(true);
        om.ShowPageNumber(true);
        om.PageReset();
        om.PageChange(0);
    }
    public void OnGameBack()
    {
        om.SREButtonEnable(true);
        om.IntroductionButtonEnable(false);
        om.ShowPageNumber(false);
        om.HideText();
    }
    public void OnGameLeft()
    {
        om.PageChange(-1);
    }
    public void OnGameRight() 
    {
        om.PageChange(1);
    }
    public void OnGameGo() 
    {
        om.SetGameCount();
        SceneManager.LoadScene("MainScene");
    }
}
