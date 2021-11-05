using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed = 7.0f;
    public float minSpeed = 1.0f;
    public float maxSpeed = 20.0f;
    public float saltoSpeed = 1.0f;

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
    public bool runner = false;

    private Rigidbody2D _body;
    private ControllerColliderHit _contact;
    private BoxCollider2D _collider;

    private Vector3 _persScale;

    private GameObject _currentPlatform;


    private bool _brakePressed = false;
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
        else if (state == State.GROUNDED)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            _body.freezeRotation = false;
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

        if (Input.GetKeyDown(KeyCode.Space) && state == State.GROUNDED)
            _jumpPressed = true;
    }

    void Move()
    {
        float braking = _brakePressed ? 0.9f : 1.0f;
        float accelerating = currentSpeed < normalSpeed && !_brakePressed ? 1.05f : 1.0f;
        currentSpeed = currentSpeed * braking * accelerating;
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

            DoSalto();
        }
    }

    private void DoSalto()
    {
        _body.freezeRotation = false;

        var direction = Random.RandomRange(0, 100);
        if (direction < 50.0f)
            saltoSpeed *= -1;

        //var normalSalto = saltoSpeed;
        //var doubleSalto = Random.RandomRange(0, 100);
        //if (doubleSalto < 50.0f)
        //    saltoSpeed *= 2;

        _body.AddTorque(saltoSpeed);
        //saltoSpeed = normalSalto;
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
