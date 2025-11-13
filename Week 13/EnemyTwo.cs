using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : MonoBehaviour
{
    public GameObject explosionPrefab;

    private GameManager gameManager;

    public float speed = 3f;
    public float zigzagSpeed = 3f;
    public float zigzagWidth = 2f;

    private float startX;
    private float timeOffset;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startX = transform.position.x;
        timeOffset = Random.Range(0f, 2f * Mathf.PI);

        transform.rotation = Quaternion.Euler(180f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float newX = startX + Mathf.Sin((Time.time + timeOffset) * zigzagSpeed) * zigzagWidth;
        float newY = transform.position.y - speed * Time.deltaTime;

        transform.position = new Vector3(newX, newY, transform.position.z);

        if (transform.position.y < -gameManager.verticalScreenSize)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D whatDidIHit)
    {
        if (whatDidIHit.tag == "Player")
        {
            whatDidIHit.GetComponent<PlayerController>().LoseALife();
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (whatDidIHit.tag == "Weapons")
        {
            Destroy(whatDidIHit.gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            gameManager.AddScore(5);
            Destroy(this.gameObject);
        }
    }
}
