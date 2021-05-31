using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text Scoretext;
    [SerializeField] private TMP_Text Livetext;
    [SerializeField] private Button _button;

    

    public int Score { get; set; }

    private void Start()
    {
        Brick.OnBrickDestruction += OnBrickDestruction;
        BrickManager.Instance.OnLevelLoaded += OnLevelLoaded;
        GameManager.Instance.OnLiveLost += OnLiveLost;
        OnLiveLost(GameManager.Instance.AvailibleLives);
       
    }

    

    private void OnLiveLost(int remainingLives)
    {
        Livetext.text = Convert.ToString(remainingLives);
    }

    private void OnLevelLoaded()
    {
        UpdateScoreText(0);
    }

    private void UpdateScoreText(int increment)
    {
        this.Score += increment;
        string scoreString = this.Score.ToString().PadLeft(4, '0');
        Scoretext.text = $@"SCORE: {scoreString}";
    }

    private void OnBrickDestruction(Brick obj)
    {
        UpdateScoreText(10);
    }

    private void OnDisable()
    {
        Brick.OnBrickDestruction -= OnBrickDestruction;
        BrickManager.Instance.OnLevelLoaded -= OnLevelLoaded;
    }

    public void PauseGame()
    {
      
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused
                : GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);  

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
