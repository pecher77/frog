using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyTrigger trigger;
    private bool started = false;
    virtual public void Start()
    {
        
    }

    virtual public void Update()
    {
        //if (trigger.IsPlayerReached() && !started)
        //{
        //    Attack();
        //}
    }

    virtual public void Attack()
    {

    }

}
