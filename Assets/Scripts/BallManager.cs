using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Singleton

    private static BallManager _instance;

    public static BallManager Instance => _instance;




    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;
    }

    #endregion

    [SerializeField] private Ball _ballPrefab;

    [SerializeField] private float _offset;

    public float InitialBallSpeed;

    private Ball _initialBall;

    private Rigidbody2D _initialBallRB;
        
    public List<Ball> Balls { get; set; }

    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        InitBall();
        
    }

     

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted)
        {
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + _offset, 0);
            _initialBall.transform.position = ballPosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _initialBallRB.isKinematic = false;
            _initialBallRB.AddForce(new Vector2(0, InitialBallSpeed));
            GameManager.Instance.IsGameStarted = true;
        }
    }

    

  

    public void RestartBalls()
    {
        foreach (var ball in this.Balls.ToList())
        {
            Destroy(ball.gameObject);
        }
        InitBall();
    }

    private void InitBall()
    {
        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startPosition = new Vector3(paddlePosition.x, paddlePosition.y + _offset, 0);
        _initialBall = Instantiate(_ballPrefab, startPosition, Quaternion.identity);
        _initialBallRB = _initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<Ball>
        {
            _initialBall
        };
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRB = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballRB.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitPoint.x;

            if(hitPoint.x < paddleCenter.x)
            {
                ballRB.AddForce(new Vector2(-Mathf.Abs(difference * 200), BallManager.Instance.InitialBallSpeed));
            }
            else
            {
                ballRB.AddForce(new Vector2(Mathf.Abs(difference * 200), BallManager.Instance.InitialBallSpeed));
            }
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
