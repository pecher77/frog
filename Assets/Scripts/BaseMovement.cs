using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    public float normalSpeed = 7.0f;
    protected float _currentSpeed;

    public bool salto = true;
    public float saltoSpeed = 1.0f;

    public float jumpForce = 12.0f;

    protected Rigidbody2D _body;
    protected BoxCollider2D _collider;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        _currentSpeed = normalSpeed;
    }

    virtual public void FixedUpdate()
    {
        Move();
    }

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }

    public void SetCurrentSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    virtual public void Move()
    {
        _body.AddForce(new Vector2(_currentSpeed, _body.velocity.y), ForceMode2D.Force);
    }

    virtual public void Jump()
    {
        _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (salto)
        {
            DoSalto();
        }
    }

    virtual public void DoSalto()
    {
        if (Random.RandomRange(0, 100) < 50.0f)
        {
            saltoSpeed *= -1;
        }
            
        _body.AddTorque(saltoSpeed);
    }

}
