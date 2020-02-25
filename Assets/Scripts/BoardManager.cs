using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    [SerializeField] private int columns = 8;
    [SerializeField] private int rows = 8;

    [SerializeField] private GameObject exit;
    [SerializeField] private GameObject[] floorTiles;
    [SerializeField] private GameObject[] foodTiles;
    [SerializeField] private GameObject[] wallTiles;
    [SerializeField] private GameObject[] outerWallTiles;
    [SerializeField] private GameObject[] enemyTiles;

    private Count wallCount = new Count(5, 9);
    private Count foodCount = new Count(1, 5);
    private Transform boardHolder;
    private List<Vector2> gridPositions = new List<Vector2>();

    
    public void SetupScene(int level)
    {
        int enemyCount = (int)Mathf.Log(level, 2f);

        BoardSetup();

        InitialList();

        LayoutObjectAtRandom(wallTiles, wallCount.Minimum, wallCount.Maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.Minimum, foodCount.Maximum);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector2(columns - 1, rows - 1), Quaternion.identity);

        
    }
    
   

    private void InitialList()
    {
        gridPositions.Clear();

        for(int x = 1; x<columns-1; x++)
        {
            for(int y = 1; y < rows -1; y++)
            {
                gridPositions.Add(new Vector2(x, y));
            }
        }
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for( int x = -1; x < columns + 1; x++ )
        {
            for( int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector2(x, y), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    private Vector2 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);

        Vector2 randomPosition = gridPositions[randomIndex];

        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tiles, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for(int i = 0; i < objectCount; i++)
        {
            Vector2 randomPosition = RandomPosition();
            GameObject tileChoice = tiles[Random.Range(0, tiles.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    [Serializable]
    public class Count
    {
        private int minimum;
        private int maximum;

        public int Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                minimum = value;
            }
        }
        public int Maximum
        {
            get
            {
                return maximum;
            }

            set
            {
                maximum = value;
            }
        }

        public Count(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }
    }
}
