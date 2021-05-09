using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableAgent : Player
{
    [Header("Move Params")]
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private int currentDestination;
    [SerializeField] private bool rotate;

    void Update()
    {
        Rotate();
    }

    void FixedUpdate()
    {
        Movement();
    }

    override protected void Movement()
    {
        if (waypoints.Length > 0)
        {
            Vector3 moveDir = (waypoints[currentDestination].transform.position - transform.position);

            if (moveDir.magnitude <= moveSpeed)
            {
                transform.position = waypoints[currentDestination].transform.position;
                currentDestination++;
                if (currentDestination >= waypoints.Length)
                    currentDestination = 0;
            }
            else
            {
                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
        }
    }

    void Rotate()
    {
        if (rotate)
            transform.Rotate(new Vector3(0, 0, 1 * angularSpeed  * Time.deltaTime), Space.Self);
    }
}
