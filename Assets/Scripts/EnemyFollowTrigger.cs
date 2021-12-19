using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowTrigger : MonoBehaviour
{
    public bool setSpeed = false;
    public float speed = 10.0f;
    public float timeToChangeSpeed = 1.0f;
    public bool instantly = false;
    [Space(10)]
    public bool jump = false;
    public float jumpForce = 10.0f;

    private bool _speedChanging = false;
    private int _framesToChange;
    private float _speedAddInFrame;
    private float accumDelta = 0.0f;
    private EnemyFollowMovement enemyMove;

    private void Update()
    {
        if (_speedChanging)
        { 

            if (!Mathf.Approximately(enemyMove.GetCurrentSpeed(), speed))
            {
                enemyMove.SetCurrentSpeed(enemyMove.GetCurrentSpeed() + _speedAddInFrame);
            }
            accumDelta += Time.deltaTime;
            //пересчет скорости в кадр
            _speedAddInFrame = (speed - enemyMove.GetCurrentSpeed()) / ((timeToChangeSpeed - accumDelta) / Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FollowEnemy")
        {
            enemyMove = collision.gameObject.GetComponent<EnemyFollowMovement>();
            if (!enemyMove)
                return;

            if (jump)
            {
                enemyMove.jumpForce = jumpForce;
                enemyMove.Jump();
            }
            if (setSpeed)
            {
                var needAdd = speed - enemyMove.GetCurrentSpeed();
                if (instantly)
                {
                    enemyMove.SetCurrentSpeed(enemyMove.GetCurrentSpeed() + needAdd);
                }
                else
                {
                    _speedChanging = true;
                    _framesToChange = (int)(timeToChangeSpeed / Time.deltaTime);
                    _speedAddInFrame = needAdd / _framesToChange;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, new Vector3(0.5f,0.5f,.0f));
    }
}
