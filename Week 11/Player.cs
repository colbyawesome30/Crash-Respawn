using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //how to define a variable
    //1. access modifier: public or private
    //2. data type: int, float, bool, string
    //3. variable name: camelCase
    //4. value: optional

    private float playerSpeed;
    private float horizontalInput;
    private float verticalInput;

    private float horizontalScreenLimit = 9.5f;
    private float verticalScreenLimit = 6.5f;
    private float topLimit = 0f; // Player is now restricted

    public GameObject bulletPrefab;

    void Start()
    {
        playerSpeed = 6f;
        //This function is called at the start of the game

    }

    void Update()
    {
        //This function is called every frame; 60 frames/second
        Movement();
        Shooting();

    }

    void Shooting()
    {
        //if the player presses the SPACE key, create a projectile
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }

    void Movement()
    {
        //Read the input from the player
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //Move the player
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * playerSpeed);

        //Player leaves the screen horizontally — wrap around
        if (transform.position.x > horizontalScreenLimit)
        {
            transform.position = new Vector3(-horizontalScreenLimit, transform.position.y, 0);
        }
        else if (transform.position.x < -horizontalScreenLimit)
        {
            transform.position = new Vector3(horizontalScreenLimit, transform.position.y, 0);
        }

        //Limit the player to the bottom half of the screen
        float camHalfHeight = Camera.main.orthographicSize;
        float camY = Camera.main.transform.position.y;

        float bottomLimit = camY - camHalfHeight;  //Bottom edge of the screen
        float topLimit = camY; //Middle of the screen
        float clampedY = Mathf.Clamp(transform.position.y, bottomLimit, topLimit);

        transform.position = new Vector3(transform.position.x, clampedY, 0);

    }
    
}