using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowMovement : MonoBehaviour
{
    public float normalSpeed = 7.0f;

    public bool salto = true;
    public float saltoSpeed = 1.0f;

    public float _currentSpeed;
    public State state;
    public enum State
    {
        GROUNDED = 0,
        IN_JUMP,
        ON_PLATFORM
    }

    private Rigidbody2D _body;
    private ControllerColliderHit _contact;
    private BoxCollider2D _collider;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _currentSpeed = normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        if (CheckGround())
        {
            state = State.GROUNDED;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            _body.freezeRotation = true;
        }
        else
        {
            state = State.IN_JUMP;
        }
    }


    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        _body.velocity = new Vector2(_currentSpeed, _body.velocity.y);
    }

    public void ChangeSpeed(float value)
    {

    }
    public void Jump(float force)
    {

        _body.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        //if (salto)
        //{
        //    DoSalto();
        //}
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

    }

    bool CheckGround()
    {
        List<ContactPoint2D> contactPoint2Ds = new List<ContactPoint2D>();
        int count = _collider.GetContacts(contactPoint2Ds);

        foreach (var point in contactPoint2Ds)
        {
            if (point.point.y < _collider.bounds.min.y)
            {
                return true;
            }
        }
        return false;
    }

}
