using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowMovement : BaseMovement
{
    public float forceToPlayerX = 5.0f;
    public float forceToPlayerY = 5.0f;

    private bool _shouldHitPlayer = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _shouldHitPlayer)
        {
            collision.gameObject.GetComponent<PlayerMovement>().AddEnemyForce(new Vector2(forceToPlayerX, forceToPlayerY));
            _shouldHitPlayer = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _shouldHitPlayer = true;
        }
    }
}
