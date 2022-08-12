using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    
    [SerializeField] Text ScoreText;
    //PlayerMovement player;

    public GameObject player;

    public static float score;
    float _score;
    private float _startX;

    void Start()
    {
        ResetScore();
        ///player = GetComponent<PlayerMovement>();
        _startX = player.transform.position.x;
    }

    void Update()
    {
        AddScore();
    }

    void ResetScore()
    {
        score = 0;
    }

    void AddScore()
    {
        //score++;
        var distance = (int)(player.transform.position.x - _startX);
        //_score = (int)score;
        ScoreText.text = "Distance: " + distance.ToString() + "m";
    }



}
