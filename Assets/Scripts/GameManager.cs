using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance => _instance;


    

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;
    }

    #endregion

    
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private GameObject _hud;

    public int AvailibleLives;
    public int Lives { get; set; }
    public bool IsGameStarted { get; set; }

    public event Action<int> OnLiveLost;
    private void Start()
    {
        this.Lives = this.AvailibleLives;
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }

    private void OnBrickDestruction(Brick obj)
    {
       if(BrickManager.Instance.RemainingBricks.Count <= 0)
        {
            BallManager.Instance.RestartBalls();
            GameManager.Instance.IsGameStarted = false;
            BrickManager.Instance.LoadNextLevel();
        }
    }

   /* public void RestartGame()
    {
        
       StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }
   */

    private void OnBallDeath(Ball obj)
    {
        if (BallManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;
            if(this.Lives < 1)
            {
                _hud.SetActive(false);
                _gameOverScreen.SetActive(true);
            }
            else
            {
                OnLiveLost?.Invoke(this.Lives);
                BallManager.Instance.RestartBalls();
                IsGameStarted = false;
                BrickManager.Instance.LoadLevel(BrickManager.Instance.CurrentLevel);
            }
        }
           
    }

    public void ShowVictoryScreen()
    {
        _hud.SetActive(false);
        _victoryScreen.SetActive(true);
       
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }

   /* IEnumerator LoadLevel(int levelIndex)
    {
        _transition.SetTrigger("Start");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(levelIndex);
    }*/
}
