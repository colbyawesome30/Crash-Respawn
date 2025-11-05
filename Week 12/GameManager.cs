using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyOnePrefab;
    public GameObject cloudPrefab;
    public GameObject coinPrefab;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;

    public float horizontalScreenSize;
    public float verticalScreenSize;

    public int score;

    // Start is called before the first frame update
    void Start()
    {
        horizontalScreenSize = 10f;
        verticalScreenSize = 6.5f;
        score = 0;
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        CreateSky();
        InvokeRepeating("CreateEnemy", 1, 3);

        //Spawn coins every 5 seconds
        InvokeRepeating("SpawnCoin", 2f, 5f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateEnemy()
    {
        Instantiate(enemyOnePrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize) * 0.9f, verticalScreenSize, 0), Quaternion.Euler(180, 0, 0));
    }

    void CreateSky()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(cloudPrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), Random.Range(-verticalScreenSize, verticalScreenSize), 0), Quaternion.identity);
        }
        
    }
    void SpawnCoin()
    {
        float randomX = Random.Range(-horizontalScreenSize, horizontalScreenSize);
        float randomY = Random.Range(-verticalScreenSize, verticalScreenSize);
        Instantiate(coinPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
    }

    public void AddScore(int earnedScore)
    {
        score = score + earnedScore;
        scoreText.text = "Score: " + score;
        
    }

    public void ChangeLivesText (int currentLives)
    {
        livesText.text = "Lives: " + currentLives;
    }
}
