using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum NumberPlayer
    {
        player_1,
        player_2
    }
    // Events
    // public delegate void ShootPlayer(Vector2 initialPos, Quaternion initialRotation);
    // public static event ShootPlayer OnShootPlayer;

    [Header("Player Status")]
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float startAngule, angularLimit;

    [Header("References")]
    [SerializeField] private GameObject spriteObj;
    [SerializeField] private GameObject bulletObj, bulletParent;

    void Start()
    {
        startAngule = spriteObj.transform.rotation.eulerAngles.z;
    }
    void Update()
    {
        Movement();
        Shoot(transform.position, spriteObj.transform.rotation);
    }

    void Movement()
    {
        if (transform.position.y < Screen.height && transform.position.y > 0)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector2.up * moveSpeed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector2.down * moveSpeed);
            }
        }
        else
        {
            if (transform.position.y >= Screen.height)
                transform.position = new Vector2(transform.position.x, Screen.height - 1);
            else if (transform.position.y <= 0)
                transform.position = new Vector2(transform.position.x, 0.1f);
        }

        if (spriteObj.transform.rotation.eulerAngles.z < startAngule + angularLimit 
        && spriteObj.transform.rotation.eulerAngles.z > startAngule - angularLimit )
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                spriteObj.transform.Rotate(new Vector3(0, 0, 1 * angularSpeed), Space.Self);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                spriteObj.transform.Rotate(new Vector3(0, 0, -1 * angularSpeed), Space.Self);
            }
        }
        else
        {
            if (spriteObj.transform.rotation.eulerAngles.z >= startAngule + angularLimit)
            {
                spriteObj.transform.Rotate(new Vector3(0f, 0f, -1f));
            }
            if (spriteObj.transform.rotation.eulerAngles.z <= startAngule - angularLimit)
            {
                spriteObj.transform.Rotate(new Vector3(0f, 0f, 1));
            }
        }
    }

    void Shoot(Vector2 pos, Quaternion rotation)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Instantiate(bulletObj, pos, rotation, bulletParent.transform);
    }
}
