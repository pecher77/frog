using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour
{
    public Camera camera;
    public Transform bg;
    public float normalCameraScale = 6.0f;
    public float farCameraScale = 10.0f;
    public float normalBGScale = 1.2f;
    public float farBGScale = 2.5f;

    private Vector3 _velocity = Vector3.zero;
    public float smoothCameraTime = 0.2f;
    public float smoothBGTime = 0.2f;

    private float offsetX;


    public enum State
    {
        Normal = 0,
        Farring,
        Nearing
    }
    public State state;

    private void Start()
    {
        offsetX = transform.position.x - camera.transform.position.x;
    }

    void LateUpdate()
    {
        switch (state)
        {
            case State.Normal:
                break;
            case State.Farring:
                camera.orthographicSize = Vector3.SmoothDamp(new Vector3(camera.orthographicSize, 0.0f, 0.0f), new Vector3(farCameraScale, 0.0f, 0.0f), ref _velocity, smoothCameraTime).x;
                //bg.localScale = Vector3.SmoothDamp(new Vector3(bg.localScale.x, bg.localScale.y, 0.0f), new Vector3(farBGScale, farBGScale, 0.0f), ref _velocity, smoothBGTime);
                break;
            case State.Nearing:
                camera.orthographicSize = Vector3.SmoothDamp(new Vector3(camera.orthographicSize, 0.0f, 0.0f), new Vector3(normalCameraScale, 0.0f, 0.0f), ref _velocity, smoothCameraTime).x;
                //bg.localScale = Vector3.SmoothDamp(new Vector3(bg.localScale.x, bg.localScale.y, 0.0f), new Vector3(normalBGScale, normalBGScale, 0.0f), ref _velocity, smoothBGTime);
                break;
        }
        
        if (camera.orthographicSize == normalCameraScale)
        {
            state = State.Normal;
        }
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(camera.transform.position.x + offsetX, transform.position.y, transform.position.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "FollowEnemy")
        {
            if (camera.orthographicSize <= farCameraScale)
            {
                state = State.Farring;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "FollowEnemy")
        {
            if (camera.orthographicSize <= farCameraScale)
            {
                state = State.Farring;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "FollowEnemy")
        {
            if (camera.orthographicSize <= farCameraScale)
            {
                state = State.Nearing;
            }
        }
    }
}
