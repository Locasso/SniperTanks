using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bullet : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Vector3 direction;

    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;

    Vector3 startingPosition;
    RaycastHit2D hit;
    bool stop;

    private void Start()
    {
        direction = transform.up;
        hit = Physics2D.Raycast(transform.position, direction, maxStepDistance);
    }

    private Vector3 newDirect;

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position += direction * movementSpeed;

        hit = Physics2D.Raycast(transform.position, direction, maxStepDistance);

        if (hit.collider != null)
        {
            Vector3 newDirect = Vector3.Reflect(transform.up, hit.normal);
            direction = newDirect;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            stop = true;
        }
    }
}
