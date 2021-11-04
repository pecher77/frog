using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpin : MonoBehaviour
{
    public enum SpinType
    {
        NOT_DEFINED,
        SIMPLE,
        REVERSING,
        STOPPING,
        STOPPING_MIRROR,
        STOPPING_RANDOM,
        ACCELERATING
    }

    [Header("Common")]
    public float speedX = 0.0f;
    public float speedY = 0.0f;
    public float speedZ = 100.0f;
    public GameObject platform1;
    public GameObject platform2;

    private Rigidbody2D _circleBody;
    private Rigidbody2D _pl1Body;
    private Rigidbody2D _pl2Body;
    private float _currentRotateSpeed;

    public SpinType spin = SpinType.SIMPLE;

    [Header("StoppingSpin")]
    public float stopPeriod = 1.0f;
    public float movePeriod = 1.0f;
    private float stopping = 0.0f;
    private float moving = 0.0f;

    bool firstMove = true;
    enum StoppingSpinState
    {
        STOP = 0,
        MOVE
    }
    public enum StoppingSpinType
    {
        SIMPLE = 0,
        MIRROR,
        RANDOM
    }
    private StoppingSpinState state;

    [Header("ReversingSpin")]
    public float reversingIndex = 0.5f;
    static float t = -1.0f;

    [Header("AcceleratingSpin")]
    public float minSpeed = 0.0f;
    public float maxSpeed = 10.0f;
    public float acelerationIndex = 1.0f;
    static float t2 = -1.0f;

    private void Start()
    {
        _circleBody = GetComponent<Rigidbody2D>();
        //_pl1Body = platform1.GetComponent<Rigidbody2D>();
        //_pl2Body = platform1.GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        
        //transform.Rotate(speedX, speedY, currRotateSpeed * Time.deltaTime);
        //_pl1Body.MoveRotation(-currRotateSpeed * Time.deltaTime);
        //_pl2Body.MoveRotation(-currRotateSpeed * Time.deltaTime);

        //platform1.transform.Rotate(-speedX, -speedY, -currRotateSpeed * Time.deltaTime);
        //platform2.transform.Rotate(-speedX, -speedY, -currRotateSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //_circleBody.MoveRotation(GetRotateSpeed());
        var rotateSpeed = GetRotateSpeed();
        //_circleBody.MoveRotation(rotateSpeed);
        _circleBody.AddTorque(rotateSpeed);
        //_pl1Body.rotation = -rotateSpeed;
        //_pl2Body.rotation = -rotateSpeed;
        //_circleBody.angularVelocity = 1;
        // transform.Rotate(speedX, speedY, currRotateSpeed * Time.deltaTime);

    }

    float GetRotateSpeed()
    {
        switch (spin)
        {
            case SpinType.NOT_DEFINED:
                break;
            case SpinType.SIMPLE:
                _currentRotateSpeed = SimpleSpin();
                break;
            case SpinType.STOPPING:
                _currentRotateSpeed = StoppingSpin(StoppingSpinType.SIMPLE);
                break;
            case SpinType.STOPPING_MIRROR:
                _currentRotateSpeed = StoppingSpin(StoppingSpinType.MIRROR);
                break;
            case SpinType.STOPPING_RANDOM:
                _currentRotateSpeed = StoppingSpin(StoppingSpinType.RANDOM);
                break;
            case SpinType.REVERSING:
                _currentRotateSpeed = ReversingSpin();
                break;
            case SpinType.ACCELERATING:
                _currentRotateSpeed = AcceleratingSpin();
                break;
            default:
                _currentRotateSpeed = SimpleSpin();
                break;
        }
        return _currentRotateSpeed;
    }

    float SimpleSpin()
    {
        return speedZ;
    }

    float StoppingSpin(StoppingSpinType type)
    {
        if (firstMove)
        {
            state = StoppingSpinState.MOVE;
            firstMove = false;
            return speedZ;
        }
        else
        {
            if (state == StoppingSpinState.MOVE)
            {
                if (moving < movePeriod)
                {
                    moving += Time.deltaTime;
                    return speedZ;
                }
                else
                {
                    state = StoppingSpinState.STOP;
                    moving = 0.0f;
                    return 0.0f;
                }
            }
            if (state == StoppingSpinState.STOP)
            {
                if (stopping < stopPeriod)
                {
                    stopping += Time.deltaTime;
                    return 0.0f;
                }
                else
                {
                    state = StoppingSpinState.MOVE;
                    stopping = 0.0f;
                    if (type == StoppingSpinType.MIRROR)
                    {
                        speedZ = -speedZ;
                    }
                    else if (type == StoppingSpinType.RANDOM)
                    {
                        var randValue = Random.Range(1,2);
                        if (randValue % 2 == 0)
                            speedZ = -speedZ;
                    }
                    return speedZ;
                }
            }
            else
            {
                return speedZ;
            }
        }
    }

    float ReversingSpin()
    {
        var speed = Mathf.Lerp(-speedZ, speedZ, t);

        t += reversingIndex * Time.deltaTime;
        if (t >= Mathf.Abs(speedZ))
        {
            speedZ = -speedZ;
            t = 0.0f;
        }

        return speed;
    }

    float AcceleratingSpin()
    {
        var rotateSpeed = Mathf.Lerp(minSpeed, maxSpeed, t2);
        
        t2 += acelerationIndex * Time.deltaTime;

        if (t2 >= Mathf.Abs(maxSpeed))
        {
            var currMin = minSpeed;
            minSpeed = maxSpeed;
            maxSpeed = currMin;
            t2 = 0.0f;
        }

        return rotateSpeed;
    }
}
