using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    // —сылки на менеджеров
    public GameObject windowManager;
    public float timeScale = 1.0f;
    void Awake()
    {
        if (WindowManager.instance == null)
        {
            Instantiate(windowManager);
        }
    }

    private void Update()
    {
        //Time.timeScale = timeScale;
    }
}
