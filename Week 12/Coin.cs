using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float lifetime = 2f;
    public float speed = 3f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only give points if the player touches the coin
        if (other.CompareTag("Player"))
        {
            gameManager.AddScore(1); // Add 1 to score
            Destroy(gameObject);     // Destroy the coin
        }
    }
}
