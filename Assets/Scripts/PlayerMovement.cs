using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseMovement
{
    public float minSpeed = 1.0f;
    public float maxSpeed = 20.0f;
    public bool infinityJump = false;
    public float noJumpTime = 0.5f;
    private float _noJumpAccum;

    public float BrakeRatio = 0.9f;
    public float AccelarationRatio = 1.05f;
    
    private bool _brakePressed = false;
    private bool _accelarationPressed = false;

    private bool _jumpPressed = false;

    public override void Update() 
    {
        base.Update();
        GetInput();
    }

    public override void FixedUpdate()
    {
        if (state == State.HITTED_BY_ENEMY)
        {
            return;
        }
        base.FixedUpdate();
        Jump();
    }

    void GetInput()
    {
        var axis = Input.GetAxis("Horizontal");
        _brakePressed = axis < -0.001f;
        _accelarationPressed = axis > 0.001f;

        if (Input.GetKey(KeyCode.Space) || infinityJump && Input.GetKeyDown(KeyCode.Space))
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
