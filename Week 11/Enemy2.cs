using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : MonoBehaviour
{
    void Update()
    {
        // Move diagonally down-right
        transform.Translate(new Vector3(0.5f, -1f, 0) * Time.deltaTime * 3f);

        // Destroy when off-screen (bottom)
        if (transform.position.y < -6.5f)
        {
            Destroy(gameObject);
        }
    }
}
