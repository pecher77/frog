using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public Transform backGround;

    private Camera _camera;

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

        Vector3 position = new Vector3(x, y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, position, ref _velocity, smoothTime);
    }
}
