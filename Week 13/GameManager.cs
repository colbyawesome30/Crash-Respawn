using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyOnePrefab;
    public GameObject enemyTwoPrefab;
    public GameObject cloudPrefab;
    public GameObject coinPrefab;
    public GameObject gameOverText;
    public GameObject restartText;
    public GameObject powerupPrefab;
    public GameObject audioPlayer;

    public AudioClip powerupSound;
    public AudioClip powerdownSound;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI powerupText;

    public float horizontalScreenSize;
    public float verticalScreenSize;

    public int score;
    public int cloudMove;

    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        horizontalScreenSize = 10f;
        verticalScreenSize = 6.5f;
        score = 0;
        cloudMove = 1;
        gameOver = false;
        AddScore(0);

        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        CreateSky();

        // Existing enemy one spawn
        InvokeRepeating("CreateEnemy", 1, 3);

        // EnemyTwo spawn
        InvokeRepeating("CreateEnemyTwo", 3f, 4f);

        // Coin spawn
        InvokeRepeating("SpawnCoin", 2f, 5f);

        // Existing powerup spawning
        StartCoroutine(SpawnPowerup());

        powerupText.text = "No powerups yet!";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Existing EnemyOne spawn
    void CreateEnemy()
    {
        Instantiate(enemyOnePrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize) * 0.9f, verticalScreenSize, 0), Quaternion.Euler(180, 0, 0));
    }

    // EnemyTwo spawn
    void CreateEnemyTwo()
    {
        Instantiate(enemyTwoPrefab, new Vector3(Random.Range(-horizontalScreenSize * 0.9f, horizontalScreenSize * 0.9f), verticalScreenSize, 0), Quaternion.identity);
    }

    // Coin spawn
    void SpawnCoin()
    {
        // limit coins to bottom third of the screen where player moves
        float yPos = Random.Range(-verticalScreenSize, -verticalScreenSize / 3f);
        Instantiate(coinPrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), yPos, 0), Quaternion.identity);
    }

    void CreatePowerup()
    {
        // limit powerups to bottom third of the screen where player moves
        float yPos = Random.Range(-verticalScreenSize, -verticalScreenSize / 3f);
        Instantiate(powerupPrefab, new Vector3(Random.Range(-horizontalScreenSize * 0.8f, horizontalScreenSize * 0.8f), yPos, 0), Quaternion.identity);
    }

    void CreateSky()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(cloudPrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), Random.Range(-verticalScreenSize, verticalScreenSize), 0), Quaternion.identity);
        }
    }

    public void ManagePowerupText(int powerupType)
    {
        switch (powerupType)
        {
            case 1:
                powerupText.text = "Speed!";
                break;
            case 2:
                powerupText.text = "Double Weapon!";
                break;
            case 3:
                powerupText.text = "Triple Weapon!";
                break;
            case 4:
                powerupText.text = "Shield!";
                break;
            default:
                powerupText.text = "No powerups yet!";
                break;
        }
    }

    IEnumerator SpawnPowerup()
    {
        float spawnTime = Random.Range(3, 5);
        yield return new WaitForSeconds(spawnTime);
        CreatePowerup();
        StartCoroutine(SpawnPowerup());
    }

    public void PlaySound(int whichSound)
    {
        switch (whichSound)
        {
            case 1:
                audioPlayer.GetComponent<AudioSource>().PlayOneShot(powerupSound);
                break;
            case 2:
                audioPlayer.GetComponent<AudioSource>().PlayOneShot(powerdownSound);
                break;
        }
    }

    public void AddScore(int earnedScore)
    {
        score += earnedScore;
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void ChangeLivesText(int currentLives)
    {
        if (livesText != null)
            livesText.text = "Lives: " + currentLives;
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
        restartText.SetActive(true);
        gameOver = true;
        CancelInvoke();
        cloudMove = 0;
    }
}
