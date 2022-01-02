using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEgg : MonoBehaviour
{
    static public float distanceForUse = 10.0f;
    public float force = 10.0f;

    private Rigidbody2D _body;

    static private GameObject _player;
    enum Owner
    {
        PLAYER,
        ENEMY
    }


    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.AddForce(new Vector2(1, 1) * force, ForceMode2D.Impulse);
    }

    static public bool CanUse(Transform enemys)
    {
        if (!_player)
        {
            _player = FindObjectOfType<PlayerMovement>().gameObject;
        }

        if ((enemys.position.x - _player.transform.position.x) < distanceForUse)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
