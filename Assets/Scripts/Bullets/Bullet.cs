using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bullet : MonoBehaviour
{
    //  Events
    public delegate void HitPlayer(int damage, string objName);
    public static event HitPlayer OnHitPlayer;

    [Header("Bullet Attributes")]
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float maxStepDistance;
    [SerializeField] protected float maxReflection;
    [SerializeField] protected int damage;

    [Header("Control parameters")]
    Vector3 direction;
    Vector3 startingPosition;
    RaycastHit2D hit;

    private void Start()
    {
        direction = transform.up;
        hit = Physics2D.Raycast(transform.position, direction, maxStepDistance);
    }

    void Update()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        transform.position += direction * movementSpeed;

        hit = Physics2D.Raycast(transform.position, direction, maxStepDistance);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag != null && hit.collider.gameObject.tag == "player")
            {
                OnHitPlayer?.Invoke(damage, hit.collider.gameObject.transform.parent.name);
                Debug.Log(hit.collider.gameObject.transform.parent.name);
                Destroy(gameObject);
                TurnControl(2);
            }
            else
            {
                Vector3 newDirect = Vector3.Reflect(transform.up, hit.normal);
                direction = newDirect;
                float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                maxReflection--;
                if (maxReflection <= 0)
                {
                    Destroy(gameObject);
                    GameManager.turnMoment = 1;
                    TurnControl(2);
                }
            }
        }
    }

    protected virtual void TurnControl(short id)
    {
        GameManager.turnMoment = id;
    }
}
