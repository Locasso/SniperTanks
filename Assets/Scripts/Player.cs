using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed;

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (transform.position.y < Screen.height && transform.position.y > 0)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector2.left * moveSpeed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector2.right * moveSpeed);
            }
        }
        else
        {
            if (transform.position.y >= Screen.height)
                transform.position = new Vector2(transform.position.x, Screen.height - 1);
            else if (transform.position.y <= 0)
                transform.position = new Vector2(transform.position.x, 0.1f);
        }
    }
}
