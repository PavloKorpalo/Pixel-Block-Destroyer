using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    #region Singleton

    private static BrickManager _instance;

    public static BrickManager Instance => _instance;




    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;
    }

    #endregion

    

    public event Action OnLevelLoaded;

    private int _maxRows = 17;
    private int _maxCols = 12;
    private GameObject _bricksContainer;
    [SerializeField] private float _initialBrickSpawnPositionX;
    [SerializeField] private float _initialBrickSpawnPositionY;
    [SerializeField] private float _shiftAmount;



    public Sprite[] Sprites;
    public Brick BrickPrefab;
    public Color[] BrickColors;
    public List<Brick> RemainingBricks { get; set; }
    public List<int[,]> LevelData { get; set; }

    public int InitialBricksCount { get; set; }

    public int CurrentLevel;

    private void Start()
    {
        this.LevelData = this.LoadLevelData();
        this._bricksContainer = new GameObject("BricksContainer");
        this.RemainingBricks = new List<Brick>();
        this.LevelData = this.LoadLevelData();
        this.GeneraterBricks();
        this.OnLevelLoaded?.Invoke();
    }

    public void LoadNextLevel()
    {
        this.CurrentLevel++;
        if(this.CurrentLevel >= this.LevelData.Count)
        {
            GameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.CurrentLevel);
        }
    }

    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.GeneraterBricks();
    }

    private void ClearRemainingBricks()
    {
        foreach(Brick brick in this.RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    private void GeneraterBricks()
    {
        this.RemainingBricks = new List<Brick>();
        int[,] currentLevelData = this.LevelData[this.CurrentLevel];
        float currentSpawnX = _initialBrickSpawnPositionX;
        float currentSpawnY = _initialBrickSpawnPositionY;
        float zShift = 0;

        for(int row = 0; row < this._maxRows; row++)
        {
            for(int col= 0; col< this._maxCols; col++)
            {
                int brickType = currentLevelData[row, col];
                if(brickType > 0)
                {
                    Brick newBrick = Instantiate(BrickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(_bricksContainer.transform, this.Sprites[brickType - 1], this.BrickColors[brickType], brickType);
                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += _shiftAmount;
                if(col + 1 == this._maxCols)
                {
                    currentSpawnX = _initialBrickSpawnPositionX;
                }
            }
            currentSpawnY -= _shiftAmount;
        }

        this.InitialBricksCount = this.RemainingBricks.Count;
    }

    private List<int[,]> LoadLevelData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;
        Debug.Log(text.text);
        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        
        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[_maxRows, _maxCols];
        int currentRow = 0;
        for(int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];
            if(line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for(int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[_maxRows, _maxCols];
            }
        }

        return levelsData;
    }

   
}
