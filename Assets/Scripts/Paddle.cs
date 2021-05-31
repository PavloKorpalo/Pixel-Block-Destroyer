using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    #region Singleton

    private static Paddle _instance;

    public static Paddle Instance => _instance;




    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;
    }

    #endregion

    [SerializeField] private float _speed;
    private float _playerY;
    private Camera _mainCamera;


    private void Start()
    {
        _playerY = this.transform.position.y;
        _mainCamera = FindObjectOfType<Camera>();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float moveHorizontal = _mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x,0)).x;
        this.transform.position = new Vector2(moveHorizontal, _playerY) * _speed;
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

            if (hitPoint.x < paddleCenter.x)
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
