using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;

    
    private Vector3 _velocity = Vector3.zero;
    public float offsetX = 5.0f;
    public float offsetY = 5.0f;
    public float deathY = 5.0f;

    private float smoothTime = 0.2f;
    private float normalY;

    void Start()
    {
        normalY = player.position.y;
    }

    void LateUpdate()
    {
        Vector3 playerPosition = new Vector3(player.position.x + offsetX, player.position.y + offsetY, transform.position.z);
        if (playerPosition.y < normalY - deathY)
        {
            playerPosition.y = transform.position.y;
        }
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref _velocity, smoothTime);
    }
}
