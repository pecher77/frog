using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    public bool moveByPhysics = true;
    public float normalSpeed = 7.0f;
    protected float _currentSpeed;

    protected bool normalRotation = true;

    public bool salto = true;
    public float saltoSpeed = 1.0f;

    public float jumpForce = 12.0f;

    protected int _lastUpdateFrame;
    protected long _currFixedUpdateFrame = 0;
    protected long _prevFixedUpdateFrame = 0;

    protected Rigidbody2D _body;
    protected BoxCollider2D _collider;
    protected Animator _animator;

    public bool _canMove = true;

    public State state;
    public enum State
    {
        UNDEFINED = 0,
        IN_JUMP,
        GROUNDED,
        SWING,//висит
        IN_JUMP_AFTER_SWING,
        HITTED_BY_ENEMY
    }

    virtual public void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _currentSpeed = normalSpeed;
    }

    virtual public void Update()
    {
        _lastUpdateFrame = Time.frameCount;
        CheckState();
    }

    virtual public void FixedUpdate()
    {
        //вторая запись делает _prevFixedUpdateFrame == _currFixedUpdateFrame
        _prevFixedUpdateFrame = _currFixedUpdateFrame;
        _currFixedUpdateFrame = Time.frameCount;

        if (_canMove)
        {
            Move();
        }
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
        if (moveByPhysics)
        {
            _body.AddForce(new Vector2(_currentSpeed, _body.velocity.y), ForceMode2D.Force);
        }
        else
        {
            transform.position += transform.right * _currentSpeed * Time.fixedDeltaTime;
        }
    }

    private void CheckState()
    {
        if (CheckGround())
        {
            state = State.GROUNDED;
            OnGround();
            transform.rotation = new Quaternion(0, 0, 0, 0);
            normalRotation = true;
            _body.freezeRotation = true;
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

    virtual public void OnGround()
    {

    }

    virtual public void OnJump()
    {

    }

    virtual public void Jump()
    {
        _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        state = State.IN_JUMP;
        OnJump();
    }

    virtual public void DoSalto()
    {
        _body.freezeRotation = false;

        //if (Random.RandomRange(0, 100) < 50.0f)
        //{
        //    saltoSpeed *= -1;
        //}
            
        //_body.AddTorque(saltoSpeed);
    }

}
