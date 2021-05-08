using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Bullet : MonoBehaviour
{
    //  Events
    public delegate void HitPlayer(int damage, string objName);
    public static event HitPlayer OnHitPlayer;

    public delegate void SendSound(string soundName);
    public static event SendSound OnSendSound;

    [Header("Bullet Attributes")]
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float stepDistance;
    [SerializeField] protected float maxReflection;
    [SerializeField] protected int damage;

    [Header("Control parameters")]
    Vector3 direction;
    Vector3 startingPosition;
    RaycastHit2D hit;
    bool stop;

    private void Start()
    {
        direction = transform.up;
        hit = Physics2D.Raycast(transform.position, direction, stepDistance);
        OnSendSound?.Invoke("shoot");
    }

    void Update()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        if (!stop)
        {
            transform.position += direction * movementSpeed;

            hit = Physics2D.Raycast(transform.position, direction, stepDistance);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag != null && hit.collider.gameObject.tag == "player")
                {
                    OnHitPlayer?.Invoke(damage, hit.collider.gameObject.transform.parent.name);
                    Debug.Log(hit.collider.gameObject.transform.parent.name);
                    OnSendSound?.Invoke("hit");
                    TurnControl(2);
                    DestroyBullet(.47f);
                }
                else
                {
                    Vector3 newDirect = Vector3.Reflect(transform.up, hit.normal);
                    direction = newDirect;
                    float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    maxReflection--;
                    OnSendSound?.Invoke("reflect");
                    if (maxReflection <= 0)
                    {
                        OnSendSound?.Invoke("miss");
                        GameManager.turnMoment = 1;
                        TurnControl(2);
                        DestroyBullet(.47f);
                    }
                }
            }
        }
    }

    protected virtual void TurnControl(short id)
    {
        GameManager.turnMoment = id;
    }

    protected virtual void DestroyBullet(float seconds)
    {
        stop = true;
        this.gameObject.GetComponentInChildren<Image>().color = Color.white;
        this.gameObject.GetComponent<Animator>().SetTrigger("destroy");
        Destroy(gameObject, seconds);
    }
}
