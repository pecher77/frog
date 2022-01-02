using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class HomeButton : MonoBehaviour
{
    private int MAIN_MENU = 0;

    public void GoHome()
    {
        SceneManager.LoadScene(MAIN_MENU);

    
    }
}
