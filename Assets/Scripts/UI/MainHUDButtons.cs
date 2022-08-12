using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MainHUDButtons : MonoBehaviour
{
    private int MAIN_MENU = 0;

    public void OnHomePressed()
    {
        SceneManager.LoadScene(MAIN_MENU);
    }

    public void OnSettingsPressed()
    {
        var settingsWindow = WindowManager.instance.GetWindowById("SettingsWindow");
        //settingsWindow.CallBeforeOpen(() =>
        //{
        //    Time.timeScale = 0f;
        //});

        //settingsWindow.CallAfterClose(() =>
        //{
        //    Time.timeScale = 1f;
        //});

        settingsWindow.Open();
    }
}
