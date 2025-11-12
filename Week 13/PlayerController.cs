using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int lives;
    private float speed;
    private int weaponType;

    private bool shieldActive = false;
    private GameObject shieldInstance; // shield attached to the player

    private GameManager gameManager;

    private float horizontalInput;
    private float verticalInput;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject thrusterPrefab;
    public GameObject shieldPrefab;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lives = 3;
        speed = 5.0f;
        weaponType = 1;
        gameManager.ChangeLivesText(lives);

        // Make shield a child of the player but keep it inactive until powerup
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
            shieldActive = false;
            if (shieldInstance != null)
            {
                shieldInstance.SetActive(false);
            }
            gameManager.ManagePowerupText(0);
            gameManager.PlaySound(2);
            return;
        }

        lives--;
        gameManager.ChangeLivesText(lives);

        if (lives == 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            gameManager.GameOver();
            Destroy(this.gameObject);
        }
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        speed = 5f;
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
        {
            shieldInstance.SetActive(false);
        }
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
                    thrusterPrefab.SetActive(true);
                    gameManager.ManagePowerupText(1);
                    break;

                case 2:
                    weaponType = 2;
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerupText(2);
                    break;

                case 3:
                    weaponType = 3;
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerupText(3);
                    break;

                case 4:
                    if (!shieldActive)
                    {
                        shieldActive = true;
                        if (shieldInstance != null)
                        {
                            shieldInstance.SetActive(true);
                        }
                        gameManager.ManagePowerupText(4);
                        gameManager.PlaySound(1);
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

        float horizontalScreenSize = gameManager.horizontalScreenSize;
        float verticalScreenSize = gameManager.verticalScreenSize;

        if (transform.position.x <= -horizontalScreenSize || transform.position.x > horizontalScreenSize)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }

        if (transform.position.y <= -verticalScreenSize || transform.position.y > verticalScreenSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
    }
}
