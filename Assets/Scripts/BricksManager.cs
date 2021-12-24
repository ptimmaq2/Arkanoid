using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Lukee levels.txt tiedoston, käsittelee tiedot ja luo listan matrixeja.
public class BricksManager : MonoBehaviour
{

    #region Singleton
    private static BricksManager _instance;
    public static BricksManager Instance => _instance;
    public static event Action OnLevelLoaded; 

    private void Awake()
    {
        //Tarkistetaan, että on vain yksi instanssi. Jos löytyy jo olemassaoleva, se tuhotaan.
        if (_instance != null)
        {
            Destroy(gameObject);

        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    private int maxRows = 17;
    private int maxColumns = 12;
    private GameObject bricksContainer;
    private float initialBrickSpawnPosX = -1.96f;
    private float initialBrickSpawnPosY = 3.325f;

    private float shiftAmount = 0.365f;
    public Brick brickPrefab;
    public Sprite[] Sprites;
    public Color[] brickColor;
    public List<Brick> remainingBricks { get; set; }
    public List<int[,]> levelData { get; set; }

    public int initBricksCount { get; set; }

    public int CurrentLevel;
    public Text nextLevelText;


    private void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");
     
        this.levelData = this.loadLevelsData();
        this.GenerateBricks();
        Debug.Log("Tasojen määrä: " + this.levelData.Count);
    }

    public void loadNextLevel()
    {
        //Ladataan seuraava taso.
        CurrentLevel++;

        if(this.CurrentLevel >= this.levelData.Count)
        {
            GameManager.Instance.ShowVictory();
        }
        else
        {
            this.loadLevel(this.CurrentLevel);
        }

        //Lisää tekstin ajastus.
    }

    private void GenerateBricks()
    {
        this.remainingBricks = new List<Brick>();

        int[,] currentLevelData = this.levelData[this.CurrentLevel];
        float currentSpawnX = initialBrickSpawnPosX;
        float currentSpawnY = initialBrickSpawnPosY;
        float zShift = 0;

        for(int row = 0; row < this.maxRows; row++)
        {
            for(int column = 0; column < this.maxColumns; column++)
            {
                int brickType = currentLevelData[row, column];

                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, 
                        currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1],
                        this.brickColor[brickType], brickType);

                    this.remainingBricks.Add(newBrick);
                    zShift += 0.001f;
                }

                currentSpawnX += shiftAmount;

                if(column + 1 == this.maxColumns) //Tarkistetaan ollaanko jo vikalla sarakkeella.
                {
                    currentSpawnX = initialBrickSpawnPosX;

                }
            }

            currentSpawnY -= shiftAmount;
        }
        this.initBricksCount = this.remainingBricks.Count;
        OnLevelLoaded?.Invoke();
    }

    public void loadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks(); //Tuhotaan edellisen kierroksen jäljiltä olevat tiilet.
        this.GenerateBricks(); //luodaan tiilet.
    }

    private void ClearRemainingBricks()
    {
        foreach (Brick brick in this.remainingBricks.ToList())
        {
            Destroy(brick.gameObject);
                
        }
    }

    private List<int[,]> loadLevelsData()
    {
       TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] { Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxColumns];
        int currentRow = 0;

        //Iteroidaan kaikki rivit
        for(int row = 0; row < rows.Length; row++)
        {
            //Tarkistetaan löytyykö riviltä "--", 
            //-- erottaa tason toisistaan. Numeroarvot vaihtelevat 0-3. 0 = ei palikkaa. 1-3 on palikan hp.
            string line = rows[row];
            if(line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for(int column = 0; column < bricks.Length; column++)
                {
                    currentLevel[currentRow, column] = int.Parse(bricks[column]);
                }
                currentRow++;
            }
            else
            {
                //Taso loppuu. Tallennetaan matrixi ja jatketaan looppia.
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel =  new int[maxRows, maxColumns];   
            }
        }

        return levelsData;
    }
}
