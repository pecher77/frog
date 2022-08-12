using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathArea : MonoBehaviour
{
    public Transform StartPoint;
    public Transform StartPointEnemy;
    public Transform cam;

    private float offsetX;

    private void Start()
    {
        offsetX = transform.position.x - cam.position.x;
    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(cam.position.x + offsetX, transform.position.y, transform.position.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = StartPoint.position;
        }
        if (collision.gameObject.tag == "FollowEnemy")
        {
            collision.gameObject.transform.position = StartPointEnemy.position;
        }
    }
}
