using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed = 7.0f;
    public float minSpeed = 1.0f;
    public float maxSpeed = 20.0f;

    public float BrakeRatio = 0.9f;
    public float AccelarationRatio = 1.05f;

    
    private float currentSpeed;
    public State state;
    public enum State
    {
        GROUNDED = 0,
        IN_JUMP,
        ON_PLATFORM
    }


    private float _currentSpeed;

    public float jumpForce = 12.0f;
    public float groundedDrag;
    public float jumpDrag;
    public bool runner = false;

    private Rigidbody2D _body;
    private ControllerColliderHit _contact;
    private BoxCollider2D _collider;

    private Vector3 _persScale;

    private GameObject _currentPlatform;

    private float _moveHorDirection;
    private bool _brakePressed = false;
    private bool _accelarationPressed = false;
    
    private bool _jumpPressed = false;



    private float _normalMass;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _persScale = transform.localScale;
        _normalMass = _body.mass;
        currentSpeed = normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        CheckGroundAndPlatform();
        GetInput();
    }

    private void CheckState()
    {
        if (state == State.IN_JUMP)
        {
            //runner = false;
            _body.mass = _normalMass;
        }
        else
        {
           //runner = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    void GetInput()
    {
        var axis = Input.GetAxis("Horizontal");
        _brakePressed = axis < -0.001f;
        _accelarationPressed = axis > 0.001f;

        if (Input.GetKeyDown(KeyCode.Space) && state == State.GROUNDED)
            _jumpPressed = true;
    }

    void Move()
    {
        float braking = currentSpeed > normalSpeed && !_accelarationPressed ? BrakeRatio : 1.0f;
        float addBraking = _brakePressed ? BrakeRatio : 1.0f;
        float accelerating = currentSpeed < normalSpeed && !_brakePressed ? AccelarationRatio : 1.0f;
        float addAccelaration = _accelarationPressed ? AccelarationRatio : 1.0f;
        currentSpeed = currentSpeed * braking * addBraking * accelerating * addAccelaration;
        if (currentSpeed < minSpeed)
            currentSpeed = minSpeed;
        if (currentSpeed > maxSpeed)
            currentSpeed = maxSpeed;

        _body.velocity = new Vector2(currentSpeed, _body.velocity.y);
    }

    void Jump()
    {
        if (_jumpPressed)
        {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            state = State.IN_JUMP;
            _jumpPressed = false;
        }
    }

    void CheckGroundAndPlatform()
    {
        RaycastHit2D[] allHits;
        allHits = Physics2D.RaycastAll(transform.position, Vector3.down);

        state = State.IN_JUMP;
        var dis = (_collider.size.y * transform.localScale.y) / 1.7f;
        foreach (var hit in allHits)
        {
            if (hit.collider != _collider && hit.distance <= dis)
            {
                state = State.GROUNDED;
                CheckPlatfotm(hit);
                return;
            }
        }
       
        ResetParentTransform();
    }

    void CheckPlatfotm(RaycastHit2D hit)
    {
        MovingPlatform platform = null;
        Vector3 pScale = Vector3.one;

        platform = hit.collider.gameObject.GetComponent<MovingPlatform>();
        if (platform)
        {
            _currentPlatform = hit.collider.gameObject;
            transform.parent = platform.transform;
            pScale = platform.transform.localScale;
            transform.localScale = new Vector3(_persScale.x / pScale.x, _persScale.y / pScale.y, 1);
            state = State.ON_PLATFORM;
        }
        else
        {
            transform.localScale = _persScale;
            transform.parent = null;
        }
    }

    void ResetParentTransform()
    {
        transform.parent = null;
        transform.localScale = _persScale;
    }
}
