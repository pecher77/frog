using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZigZag : EnemyBase
{
    public float speedY = 1.0f;
    public float speedX = 1.0f;
    public float amplitude = 2.0f;
    static float t = 0.0f;
    //public Vector3 startPoint;

    private float positionY;
    private float upLimitY;
    private float downLimitY;

    public override void Start()
    {
        positionY = transform.position.y;
        upLimitY = positionY + amplitude;
        downLimitY = positionY - amplitude;
    }
    public override void Attack()
    {
        t += speedY * Time.deltaTime;
        if (t > 1.0f || t < 0.00f)
        {
            speedY = -speedY;
        }

        var y = Mathf.Lerp(downLimitY, upLimitY, t);
        transform.position = new Vector3(transform.position.x - speedX * Time.deltaTime, y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
    }
}
