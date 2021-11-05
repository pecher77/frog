using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.2f;
    private Vector3 _velocity = Vector3.zero;
    public float offsetX = 5.0f;
    public float offsetY = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerPosition = new Vector3(player.position.x + offsetX, player.position.y + offsetY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref _velocity, smoothTime);
    }
}
