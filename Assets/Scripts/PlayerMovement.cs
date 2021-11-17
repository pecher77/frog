using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed = 7.0f;
    public float minSpeed = 1.0f;
    public float maxSpeed = 20.0f;
    public bool DebagEnabled = false;

    public float noJumpTime = 0.5f;
    private float _noJumpAccum;
    private bool canJump = true;
    private bool normalRotation = true;

    public bool salto = true;
    public float saltoSpeed = 1.0f;

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

    public float jumpForce = 12.0f;
    public bool runner = false;

    private Rigidbody2D _body;
    private ControllerColliderHit _contact;
    private BoxCollider2D _collider;

    private RaycastHit2D groundHit;

    private Vector3 _persScale;
    private GameObject _currentPlatform;

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
        ProcessState();
        GetInput();
    }

    private void CheckState()
    {
        //чтобы наверн€ка завершить переворот сальто
        //if (CheckGround(0.2f, 0.3f))
        //{
        //    transform.rotation = new Quaternion(0, 0, 0, 0);
        //    normalRotation = true;
        //    _body.freezeRotation = true;
        //}
        if (CheckGround())
        {
            state = State.GROUNDED;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            normalRotation = true;
            _body.freezeRotation = true;
            if (CheckPlatform())
            {
                state = State.ON_PLATFORM;
            }
        }
        else
        {
            state = State.IN_JUMP;
            ResetParentTransform();
        }
    }

    private void ProcessState()
    {
        if (state == State.IN_JUMP)
        {
            _body.mass = _normalMass;
        }
    }

    public State GetState() { return state;  }

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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space))
            _jumpPressed = true;
        else
            _jumpPressed = false;
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
        if (_noJumpAccum < noJumpTime)
        {
            _noJumpAccum += Time.deltaTime;
            return;
        }

        if (DebagEnabled)
        {
            Debug.Log("jump pressed: " + _jumpPressed);
            Debug.Log("state: " + state);
            Debug.Log("normalRotation: " + normalRotation);
            Debug.Log("_body.freezeRotation: " + _body.freezeRotation);
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

    private void DoSalto()
    {
        _body.freezeRotation = false;

        var direction = Random.RandomRange(0, 100);
        if (direction < 50.0f)
            saltoSpeed *= -1;

        var normalSalto = saltoSpeed;
        var doubleSalto = Random.RandomRange(0, 100);
        if (doubleSalto < 50.0f)
            saltoSpeed *= 2;

        _body.AddTorque(saltoSpeed);
        saltoSpeed = normalSalto;
        normalRotation = false;
    }

    bool CheckGround()
    {
        List<ContactPoint2D> points = new List<ContactPoint2D>();
        int count = _collider.GetContacts(points);

        foreach (var point in points)
        {
            if (point.point.y < transform.position.y)
            {
                return true;
            }
        }
        return false;

        //RaycastHit2D[] allHits;
        //Vector2 leftCorner = new Vector2(_collider.bounds.min.x, _collider.bounds.min.y - leftOffset);
        //Vector2 rightCorner = new Vector2(_collider.bounds.max.x, _collider.bounds.min.y - rightOffset);
        //Collider2D hit = Physics2D.OverlapArea(leftCorner, rightCorner);

        //if (hit)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        //var y = transform.position.y - (_collider.size.y * transform.localScale.y) / 2;
        //allHits = Physics2D.RaycastAll(transform.position, Vector3.down);

        //var dis = (_collider.size.y * transform.localScale.y) / distanceRatio;
        //foreach (var hit in allHits)
        //{
        //    if (hit.collider != _collider && hit.distance <= dis)
        //    {
        //        groundHit = hit;
        //        return true;
        //    }
        //}

        //return false;
    }

    bool CheckPlatform()
    {
        //MovingPlatform platform = null;
        //Vector3 pScale = Vector3.one;

        //platform = groundHit.collider.gameObject.GetComponent<MovingPlatform>();
        //if (platform)
        //{
        //    _currentPlatform = groundHit.collider.gameObject;
        //    transform.parent = platform.transform;
        //    pScale = platform.transform.localScale;
        //    transform.localScale = new Vector3(_persScale.x / pScale.x, _persScale.y / pScale.y, 1);
        //    return true;
        //}
        //else
        //{
        //    transform.localScale = _persScale;
        //    transform.parent = null;
        //    return false;
        //}
        return false;
    }

    void ResetParentTransform()
    {
        transform.parent = null;
        transform.localScale = _persScale;
    }
}
