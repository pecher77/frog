using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseMovement
{
    public float minSpeed = 1.0f;
    public float maxSpeed = 20.0f;

    public float noJumpTime = 0.5f;
    private float _noJumpAccum;

    private bool normalRotation = true;

    public float BrakeRatio = 0.9f;
    public float AccelarationRatio = 1.05f;
    
    public State state;
    public enum State
    {
        UNDEFINED = 0,
        GROUNDED,
        IN_JUMP,
        HITTED_BY_ENEMY
    }
    
    private bool _brakePressed = false;
    private bool _accelarationPressed = false;

    private bool _jumpPressed = false;

    void Update()
    {
        CheckState();
        GetInput();
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

    public State GetState() { return state;  }

    public override void FixedUpdate()
    {
        if (state == State.HITTED_BY_ENEMY)
        {
            return;
        }
        Move();
        Jump();
    }

    void GetInput()
    {
        var axis = Input.GetAxis("Horizontal");
        _brakePressed = axis < -0.001f;
        _accelarationPressed = axis > 0.001f;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space))
        {
            _jumpPressed = true;
        }
        else
        {
            _jumpPressed = false;
        } 
    }

    public override void Move()
    {
        float braking = _currentSpeed > normalSpeed && !_accelarationPressed ? BrakeRatio : 1.0f;
        float addBraking = _brakePressed ? BrakeRatio : 1.0f;
        float accelerating = _currentSpeed < normalSpeed && !_brakePressed ? AccelarationRatio : 1.0f;
        float addAccelaration = _accelarationPressed ? AccelarationRatio : 1.0f;
        _currentSpeed = _currentSpeed * braking * addBraking * accelerating * addAccelaration;
        if (_currentSpeed < minSpeed)
        {
            _currentSpeed = minSpeed;
        }        
        if (_currentSpeed > maxSpeed)
        {
            _currentSpeed = maxSpeed;
        }

        _body.AddForce(new Vector2(_currentSpeed, _body.velocity.y), ForceMode2D.Force);
    }

    public override void Jump()
    {
        if (_noJumpAccum < noJumpTime)
        {
            _noJumpAccum += Time.deltaTime;
            return;
        }

        if (_jumpPressed && state == State.GROUNDED && normalRotation && _body.freezeRotation)
        {
            state = State.IN_JUMP;
            _noJumpAccum = 0.0f;

            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            if (salto)
            {
                DoSalto();
            }
        }
    }

    bool CheckGround()
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

    public void AddEnemyForce(Vector2 force)
    {
        StartCoroutine(routine: AddEnemyForceCoroutine(force));
    }

    public IEnumerator AddEnemyForceCoroutine(Vector2 force)
    {
        state = State.HITTED_BY_ENEMY;

        _body.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(10);
        state = State.UNDEFINED;
    }
}
