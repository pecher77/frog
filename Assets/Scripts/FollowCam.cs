using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public Transform backGround;

    private Camera _camera;

    public float borderY = 3;
    private float startY;

    public bool followEnemy = false;
    public bool freeCamera = false;

    private Vector3 _velocity = Vector3.zero;
    public float offsetX = 5.0f;
    public float offsetY = 5.0f;
    public float lowLimitY = 5.0f;
    public float highLimitY = 5.0f;

    private float smoothTime = 0.2f;
    private float lowLimit;
    private float highLimit;

    void Start()
    {
        lowLimit = player.position.y - lowLimitY;
        highLimit = player.position.y + highLimitY;
        startY = followEnemy ? (enemy.position.y + offsetY) : (player.position.y + offsetY);
        _camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {

        float y = followEnemy ? (enemy.position.y + offsetY) : (player.position.y + offsetY);
        float x = followEnemy ? (enemy.position.x + offsetX) : (player.position.x + offsetX);
        
        

        if (!freeCamera)
        {
            if (y < lowLimit)
            {
                y = lowLimit;
            }
            else if (y > highLimit)
            {
                y = highLimit;
            }
        }

        if (y > startY + borderY)
        {
            var overAdded = y - (startY + borderY);
            //Vector3.SmoothDamp(transform.position, position, ref _velocity, smoothTime).y;
            _camera.orthographicSize = Vector3.SmoothDamp(new Vector3(_camera.orthographicSize, 0.0f, 0.0f), new Vector3(10.0f, 0.0f, 0.0f), ref _velocity, smoothTime).x;
            //backGround.localScale = new Vector3(2.5f, 2.5f, backGround.localScale.z);
        }
        else
        {
            //_camera.orthographicSize = 6;
            _camera.orthographicSize = Vector3.SmoothDamp(new Vector3(_camera.orthographicSize, 0.0f, 0.0f), new Vector3(6.0f, 0.0f, 0.0f), ref _velocity, smoothTime).x;
            //backGround.localScale = new Vector3(1f, 1f, backGround.localScale.z);
        }

        Vector3 position = new Vector3(x, y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, position, ref _velocity, smoothTime);
    }
}
