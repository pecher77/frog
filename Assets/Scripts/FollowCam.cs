using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;

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
    }

    void LateUpdate()
    {

        float y = player.position.y + offsetY;
        if (y < lowLimit)
        {
            y = lowLimit;
        }
        else if (y > highLimit)
        {
            y = highLimit;
        }

        Vector3 playerPosition = new Vector3(player.position.x + offsetX, y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref _velocity, smoothTime);
    }
}
