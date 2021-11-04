using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float massPlusRatio = 1000.0f;
    public float massMinusRatio = 3.0f;
    public float timeToChangeMass = 0.5f;
    public float force = 100.0f;

    private bool _addedForce = false;

    private float _accumTime = 0;
    
    private Vector3 _prevPos;
    private Vector3 _currPos;
    private MoveType _currMove = MoveType.IDLE;
    private bool _playerIsOnPlatform = false;
    private bool _uppingPlayer = false;

    private float _playerNormalMass;
    private Rigidbody2D _playersBody;

    enum MoveType
    {
        IDLE = 0,
        TODOWN,
        DOWN,
        DOWNIDLE,        
        TOUP,
        UP
    }
    void Start()
    {
        _currPos = transform.position;
    }

    void Update()
    {
        if (_playerIsOnPlatform)
        {
            _accumTime += Time.deltaTime;
        }

        if (_accumTime >= timeToChangeMass)
        {
            if (_playersBody)
            {
                //_playersBody.mass = _playerNormalMass * (1 / massMinusRatio);
                _playersBody.mass = _playerNormalMass;

                if (!_addedForce)
                {
                    _playersBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                    _playersBody = null;
                    _addedForce = true;
                }
                
            }
        }

        _prevPos = _currPos;
        _currPos = transform.position;
        

        EvaluateMoveState();
    }

    private void EvaluateMoveState()
    {
        if (_currPos.y < _prevPos.y)
        {
            _currMove = MoveType.DOWN;
        }
        else if (_currPos.y > _prevPos.y)
        {
            if (_currMove == MoveType.IDLE && _playerIsOnPlatform)
            {
                //if (_playersBody)
                //{
                //    _playersBody.mass = _playerNormalMass;
                //    _playersBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                //}

            }
            _currMove = MoveType.UP;
        }
        else if (_currPos.y == _prevPos.y)
        {

            _currMove = MoveType.IDLE;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playersBody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (_playersBody)
            {
                _playerIsOnPlatform = true;
                _playerNormalMass = _playersBody.mass;
                _playersBody.mass = _playerNormalMass * massPlusRatio;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerIsOnPlatform = false;
            _addedForce = false;
        }
    }
}
