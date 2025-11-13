using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int lives;
    private float speed;
    private int weaponType;

    private bool shieldActive = false;
    private GameObject shieldInstance; // Shield attached to the player

    private float horizontalInput;
    private float verticalInput;

    private GameManager gameManager;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject thrusterPrefab;
    public GameObject shieldPrefab;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        lives = 3;        
        speed = 6f;
        weaponType = 1;

        // Update UI immediately
        gameManager.ChangeLivesText(lives);

        // Setup shield prefab but keep inactive
        if (shieldPrefab != null)
        {
            shieldInstance = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
            shieldInstance.transform.localPosition = Vector3.zero;
            shieldInstance.SetActive(false);
        }
    }

    void Update()
    {
        Movement();
        Shooting();
    }

    public void LoseALife()
    {
        if (shieldActive)
        {
            // Shield absorbs damage then goes away
            shieldActive = false;
            if (shieldInstance != null)
                shieldInstance.SetActive(false);
            gameManager.ManagePowerupText(0);
            gameManager.PlaySound(2);
            return;
        }

        lives--;
        gameManager.ChangeLivesText(lives);

        if (lives <= 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            gameManager.GameOver();
            Destroy(this.gameObject);
        }
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        speed = 6f;
        if (thrusterPrefab != null)
            thrusterPrefab.SetActive(false);
        gameManager.ManagePowerupText(0);
        gameManager.PlaySound(2);
    }

    IEnumerator WeaponPowerDown()
    {
        yield return new WaitForSeconds(3f);
        weaponType = 1;
        gameManager.ManagePowerupText(0);
        gameManager.PlaySound(2);
    }

    IEnumerator ShieldPowerDown()
    {
        yield return new WaitForSeconds(5f);
        shieldActive = false;
        if (shieldInstance != null)
            shieldInstance.SetActive(false);
        gameManager.ManagePowerupText(0);
        gameManager.PlaySound(2);
    }

    private void OnTriggerEnter2D(Collider2D whatDidIHit)
    {
        if (whatDidIHit.CompareTag("Powerup"))
        {
            Destroy(whatDidIHit.gameObject);
            int whichPowerup = Random.Range(1, 5);
            gameManager.PlaySound(1);

            switch (whichPowerup)
            {
                case 1:
                    speed = 10f;
                    StartCoroutine(SpeedPowerDown());
                    if (thrusterPrefab != null)
                        thrusterPrefab.SetActive(true);
                    gameManager.ManagePowerupText(1); // Speed!
                    break;

                case 2:
                    weaponType = 2; // Double weapon
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerupText(2); // Double Weapon!
                    break;

                case 3:
                    weaponType = 3; // Triple weapon
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerupText(3); // Triple Weapon!
                    break;

                case 4:
                    if (!shieldActive)
                    {
                        shieldActive = true;
                        if (shieldInstance != null)
                            shieldInstance.SetActive(true);
                        gameManager.ManagePowerupText(4); // Shield!
                        StartCoroutine(ShieldPowerDown());
                    }
                    break;
            }
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (weaponType)
            {
                case 1:
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.Euler(0, 0, 45));
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.Euler(0, 0, -45));
                    break;
            }
        }
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);

        float horizontalScreenLimit = gameManager.horizontalScreenSize;
        float verticalScreenLimit = gameManager.verticalScreenSize;

        // Wrap horizontal
        if (transform.position.x > horizontalScreenLimit)
            transform.position = new Vector3(-horizontalScreenLimit, transform.position.y, 0);
        else if (transform.position.x < -horizontalScreenLimit)
            transform.position = new Vector3(horizontalScreenLimit, transform.position.y, 0);

        // Limit vertical to bottom half of the screen
        float camHalfHeight = Camera.main.orthographicSize;
        float camY = Camera.main.transform.position.y;

        float bottomLimit = camY - camHalfHeight;
        float topLimit = camY; // middle of screen
        float clampedY = Mathf.Clamp(transform.position.y, bottomLimit, topLimit);

        transform.position = new Vector3(transform.position.x, clampedY, 0);
    }
}
