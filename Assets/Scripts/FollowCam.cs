using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.2f;
    private Vector3 _velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerPosition = new Vector3(player.position.x + 5, player.position.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref _velocity, smoothTime);
    }
}
