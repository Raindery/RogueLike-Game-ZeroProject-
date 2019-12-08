using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    [SerializeField]
    public int colums = 8;

    [SerializeField]
    public int rows = 8;

    
    public Count wallCount = new Count(5, 9);

    
    public Count foodCount = new Count(5, 9);

    //Декларирование игровых объектов (Declare game objects)
    [SerializeField]
    public GameObject exit;

    [SerializeField]
    public GameObject[] floorTiles;

    [SerializeField]
    public GameObject[] foodTiles;

    [SerializeField]
    public GameObject[] wallTiles;

    [SerializeField]
    public GameObject[] enemyTiles;

    [SerializeField]
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector2> gridPos = new List<Vector2>();
    


    //Инициализация списка для хранения объектов на Board
    void InitialList()
    {
        gridPos.Clear();

        for(int x = 1; x< colums -1; x++)
        {
            for(int y = 1; y < rows; y++)
            {
                gridPos.Add(new Vector2(x, y));
            }
        }
    }


    //Функция для установки границ и поля
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < colums + 1; x++)
        {
            for(int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == colums || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector2(x, y), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }

    }


    //Функция для установки рандомной позиции
    Vector2 RandomPostion()
    {
        int randomIndex = Random.Range(0, gridPos.Count);
        Vector2 randomPosition = gridPos[randomIndex];
        gridPos.RemoveAt(randomIndex);
        return randomPosition;
    }


    //Функция для рандомной расстоновки тайлов объектов и врагов
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for(int i = 0; i < objectCount; i++)
        {
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, RandomPostion(), Quaternion.identity);
        }
    }


    //Установка сцены
    public void SetupScene(int level)
    {
        BoardSetup();
        InitialList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector2(colums - 1, rows - 1), Quaternion.identity);
    }


}
