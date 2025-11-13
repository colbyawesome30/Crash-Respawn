using UnityEngine;

public class Coin : MonoBehaviour
{
    public float lifetime = 2f; // Coin will disappear after this time
    public Vector3 coinScale = new Vector3(0.5f, 0.5f, 0.5f); 

    private GameManager gameManager;

    void Start()
    {
        // Get reference to GameManager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Scale the coin
        transform.localScale = coinScale;

        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.AddScore(1); // Add 1 point
            }
            Destroy(gameObject);
        }
    }
}
