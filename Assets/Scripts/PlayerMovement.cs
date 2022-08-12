using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseMovement
{
    public float minSpeed = 1.0f;
    public float maxSpeed = 20.0f;
    public bool infinityJump = false;

    public float BrakeRatio = 0.9f;
    public float AccelarationRatio = 1.05f;
    
    private bool _brakePressed = false;
    private bool _accelarationPressed = false;

    private bool _jumpWasPressedInFrame = false;
    private bool _jumpHolding = false;
    private bool _needUnhook = false;

    private AnchoredJoint2D _joint = new AnchoredJoint2D();

    private bool _isColliding = false;

    private GameObject _jointObject;
    private Joint _jointSettings = null;
    private Vector3 _jointObjectWorldPoint;
    public int jumpForceFromJoint = 50;
    
    public float timeBetweenSwings = 0.1f;

    public float gravityScaleAfterSwing = 10.0f;
    private float _normalGravityScale;

    private Vector3 _startPointToJumpFromJoint;
    public override void Start()
    {
        base.Start();
        _normalGravityScale = _body.gravityScale;
    }

    bool RecallFixedUpdateInFrame()
    {
        //вызывается второй раз в кадре
        if (_prevFixedUpdateFrame == Time.frameCount)
        {
            return true;
        }
        return false;
    }

    public override void Update() 
    {
        _jumpWasPressedInFrame = false;

        base.Update();
        GetInput();

        _isColliding = false;
    }

    void GetInput()
    {
        var axis = Input.GetAxis("Horizontal");
        _brakePressed = axis < -0.001f;
        _accelarationPressed = axis > 0.001f;

        //отжали после держания
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _jumpWasPressedInFrame = false;
            _jumpHolding = false;
            if (state == State.SWING)
            {
                _needUnhook = true;
            }

            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            //только что нажата
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpWasPressedInFrame = true;
            }
            else //держим
            {
                _jumpHolding = true;
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate(); //Move

        if (state == State.SWING)
        {
            //if (CanSwing())
            //{
            Swing();
            //}
        }

        if (RecallFixedUpdateInFrame())
        {
            return;
        }

        Jump();
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

        base.Move();
    }

    public override void Jump()
    {
        //if (state == State.HANG)
        //{
        //    //отжали кнопку в висе
        //    if (_needUnhook || _jumpWasPressedInFrame)
        //    {
        //        OnUnhook();
        //        return;
        //    }

        //    //if (_jumpWasInHang)
        //    //{
        //    //    //раскачиваемся
        //    //    if (_jumpHolding)
        //    //    {
        //    //        //даем силу только в 3 доле
        //    //        if (CanSwing())
        //    //        {
        //    //            //Swing();
        //    //        }
        //    //        return;
        //    //    }

        //    //}
        //    //else //прыжок не был нажат при висе
        //    //{
        //    //    if (_jumpWasPressedInFrame)
        //    //    {
        //    //        _jumpWasInHang = true;
        //    //        //раскачиваемся
        //    //        //Swing();
        //    //        return;
        //    //    }
        //    //}
        //    //return;
        //}

        if (!_jumpWasPressedInFrame && !_jumpHolding)
        {
            return;
        }

        //обычный прыжок
        if (state == State.GROUNDED && normalRotation && _body.freezeRotation)
        {
            base.Jump();
        }

    }

    public void Swing()
    {
        if (_needUnhook)
        {
            OnUnhook();
            return;
        }

        if (_joint != null)
        {
            var direction = GetDirectionForSwing();
            var force = GetForceForSwing();
            var forceType = _jointSettings.forceType;
            _body.AddForce(direction * force, forceType);
        }
        else
        {
            OnUnhook();
        }
    }

    public void OnUnhook()
    {
        _needUnhook = false;
        
        _startPointToJumpFromJoint = transform.position;
        
        gameObject.GetComponent<AnchoredJoint2D>().enabled = false;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        _body.freezeRotation = true;

        Destroy(_joint);
        _joint = null;

        state = State.IN_JUMP_AFTER_SWING;
        _body.gravityScale = gravityScaleAfterSwing;
        _body.AddForce(GetDirectionForSwing() * jumpForceFromJoint, ForceMode2D.Impulse);
        _jointSettings = null;

    }

    public Vector3 GetDirectionForSwing()
    {
        var rightDirection = Vector2.right;
        var directionToJoint = _jointObjectWorldPoint - transform.position;
        var rotatedDirection = Quaternion.Euler(0, 0, _jointSettings.forceAngle) * directionToJoint;
        return rotatedDirection;
    }

    public float GetForceForSwing()
    {
        return _jointSettings.force;
    }

    public bool CanSwing() 
    {
        return transform.position.y < _jointObjectWorldPoint.y || 
            (transform.position.y > _jointObjectWorldPoint.y &&
            transform.position.x < _jointObjectWorldPoint.x);
    }

    public void OnJoint(Joint joint)
    {
        //Time.timeScale = 0.8f;
        _jointSettings = joint;
        _body.gravityScale = _normalGravityScale;
        _jointObject = joint.gameObject;
        _body.freezeRotation = false;
        transform.position = _jointObject.transform.position + joint.playerStartPositionOffset;
        _jointObjectWorldPoint = _jointObject.transform.position;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        _canMove = false;

        switch (joint.jointType)
        {
            case Joint.JointType.HINGE_JOINT:
                SetupHingeJoint(joint);
                break;
            case Joint.JointType.SPRING_JOINT:
                SetupSpringJoint(joint);
                break;
            case Joint.JointType.WHEEL_JOINT:
                SetupWheelJoint(joint);
                _joint = gameObject.AddComponent<WheelJoint2D>();
                break;
            case Joint.JointType.FIXED_JOINT:
                SetupFixedJoint(joint);
                _joint = gameObject.AddComponent<FixedJoint2D>();
                break;
            default:
                break;
        }

       
        _joint.enabled = true;
        state = State.SWING;
        
    }

    private void SetupHingeJoint(Joint joint)
    {
        _joint = gameObject.AddComponent<HingeJoint2D>();
        _joint.anchor = transform.InverseTransformPoint(_jointObjectWorldPoint);
        _joint.connectedBody = _jointObject.GetComponent<Rigidbody2D>();
        _joint.connectedAnchor = new Vector3(0, 0, 0);
    }

    private void SetupSpringJoint(Joint joint)
    {
        _joint = gameObject.AddComponent<SpringJoint2D>();
        var springJoint = ((SpringJoint2D)_joint);
        _joint.connectedBody = _jointObject.GetComponent<Rigidbody2D>();
        springJoint.autoConfigureDistance = false;
        springJoint.distance = joint.springDistance;
        springJoint.frequency = joint.springFrequency;
        springJoint.dampingRatio = joint.springDampingRatio;
        //анчор на игроке
        _joint.anchor = new Vector3(0, 0, 0);
        //коннектед анчор - конец веревки на ящике
        _joint.connectedAnchor = new Vector3(0, 0, 0);

    }

    private void SetupWheelJoint(Joint joint)
    {
        _joint = gameObject.AddComponent<WheelJoint2D>();
        var wheelJoint = ((WheelJoint2D)_joint);
        _joint.connectedBody = _jointObject.GetComponent<Rigidbody2D>();
        //springJoint.autoConfigureDistance = false;
        //springJoint.distance = joint.springDistance;
        //springJoint.frequency = joint.springFrequency;
        //анчор на игроке
        _joint.anchor = new Vector3(0, 0, 0);
        //коннектед анчор - конец веревки на ящике
        _joint.connectedAnchor = new Vector3(0, 0, 0);
    }

    private void SetupFixedJoint(Joint joint)
    {
        _joint = gameObject.AddComponent<FixedJoint2D>();
        var fixedJoint = ((FixedJoint2D)_joint);
        _joint.connectedBody = _jointObject.GetComponent<Rigidbody2D>();
        fixedJoint.autoConfigureConnectedAnchor = false;
        //анчор на игроке
        _joint.anchor = new Vector3(0, 0, 0);
        //коннектед анчор - конец веревки на ящике
        _joint.connectedAnchor = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isColliding)
        {
            return;
        }

        if (_jointObject == collision.gameObject)
        {
            return;
        }

        _isColliding = true;

        var isJoint = collision.gameObject.GetComponent<Joint>();
        if (isJoint != null)
        {
            OnJoint(isJoint);
        }
    }

    public override void OnGround()
    {
        base.OnGround();
        _canMove = true;
        _animator.SetInteger("state", (int)state);
        _jointObject = null;
        _body.gravityScale = _normalGravityScale;
        Time.timeScale = 1.0f;
    }

    public override void OnJump()
    {
        base.OnJump();
        _animator.SetInteger("state", (int)state);
    }

    void OnDrawGizmos()
    {
        if (state == State.SWING && _joint != null)
        {
            var direction = GetDirectionForSwing();
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }
    }
}
