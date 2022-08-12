using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    public float normalSpeed = 7.0f;
    protected float _currentSpeed;

    protected bool normalRotation = true;

    public bool salto = true;
    public float saltoSpeed = 1.0f;

    public float jumpForce = 12.0f;

    protected Rigidbody2D _body;
    protected BoxCollider2D _collider;

    public State state;
    public enum State
    {
        UNDEFINED = 0,
        GROUNDED,
        IN_JUMP,
        HITTED_BY_ENEMY
    }

    virtual public void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        _currentSpeed = normalSpeed;
    }

    virtual public void Update()
    {
        CheckState();
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

    private void CheckState()
    {

        if (CheckGround())
        {
            state = State.GROUNDED;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            normalRotation = true;
            _body.freezeRotation = true;
        }
        else
        {
            state = State.IN_JUMP;
        }
    }

    protected bool CheckGround()
    {

        List<ContactPoint2D> points = new List<ContactPoint2D>();
        int count = _collider.GetContacts(points);


        foreach (var point in points)
        {
            if (point.point.y < transform.position.y && point.point.x < _collider.bounds.max.x)
            {
                return true;
            }
        }
        return false;
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
        _body.freezeRotation = false;
        if (Random.RandomRange(0, 100) < 50.0f)
        {
            saltoSpeed *= -1;
        }
            
        _body.AddTorque(saltoSpeed);
    }

}
